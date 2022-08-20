using System;
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
    
    private float dashPower = 14f;
    private float dashingCooldown = 1f;
    private bool canDash = true;
    private float dashTime = .2f;
    private bool isDashing;

    private bool isDoubleJumping = false;
    private bool canDoubleJumpping = false;
    

    private float dirX;
    
    private enum MoveState { idle, running, jumping, falling, doubleJumpping}

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
        if (isDashing)
        {
            return;
        }

        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            canDoubleJumpping = true;
        }
        
        if (Input.GetButtonDown("Jump") && !IsGrounded() && canDoubleJumpping)
        {
            isDoubleJumping = true;
            canDoubleJumpping = false;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }
    

    if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        _rigidbody2D.velocity = new Vector2(dirX * moveSpeed, _rigidbody2D.velocity.y);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = new Vector2(transform.localScale.x * dirX * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        _rigidbody2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
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

        if (_rigidbody2D.velocity.y > .1f && isDoubleJumping)
        {
            state = MoveState.doubleJumpping;
        } 
        
        _animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        isDoubleJumping = false;
        return Physics2D.BoxCast(_collider2D.bounds.center, _collider2D.bounds.size, 0f, Vector2.down, .1f, maskGround);
    }
}
