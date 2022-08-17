using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colGameObject = col.gameObject;

        if (colGameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _animator.SetTrigger("death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
