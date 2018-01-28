using System;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
    public float spawnDelay = 0.5f;

    public Energy energy;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public bool CanMove { get; set; } = true;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public async void Respawn(Transform location)
    {
        Stop();
        transform.position = location.position;
        CanMove = false;
        await new WaitForSeconds(spawnDelay);
        CanMove = true;
    }

    protected override void ComputeVelocity()
    {
        var move = Vector2.zero;

        if (CanMove)
        {
            move.x = Input.GetAxis("Horizontal");

            if (Mathf.Abs(move.x) > Mathf.Epsilon)
            {
                energy.Move();
            }

            if (Input.GetButtonDown("Jump") && _grounded)
            {
                energy.Jump();
                _velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (_velocity.y > 0)
                {
                    _velocity.y = _velocity.y * 0.5f;
                }
            }

            bool flipSprite = (_spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
            if (flipSprite)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }

        _animator.SetBool("grounded", _grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / maxSpeed);

        _targetVelocity = move * maxSpeed;
    }
}