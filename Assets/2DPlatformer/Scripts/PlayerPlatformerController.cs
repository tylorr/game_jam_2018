using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Use this for initialization
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        var move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (_velocity.y > 0)
            {
                _velocity.y = _velocity.y * 0.5f;
            }
        }

        bool flipSprite = (_spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _animator.SetBool("grounded", _grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / maxSpeed);

        _targetVelocity = move * maxSpeed;
    }
}