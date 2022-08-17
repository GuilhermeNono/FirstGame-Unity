using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private BoxCollider2D _collider2D;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private LayerMask maskGround; 

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    private float dirX;
    
    private enum MoveState { idle, running, jumping, falling}

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        _rigidbody2D.velocity = new Vector2(dirX * moveSpeed, _rigidbody2D.velocity.y);
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MoveState state;
        
        if (dirX > 0f)
        {
            state = MoveState.running;
            _spriteRenderer.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MoveState.running;
            _spriteRenderer.flipX = true;
        }
        else
        {
            state = MoveState.idle;
        }

        if (_rigidbody2D.velocity.y > .1f)
        {
            state = MoveState.jumping;
        }
        else if(_rigidbody2D.velocity.y < -.1f)
        {
            state = MoveState.falling;
        }
        
        _animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_collider2D.bounds.center, _collider2D.bounds.size, 0f, Vector2.down, .1f, maskGround);
    }
}
