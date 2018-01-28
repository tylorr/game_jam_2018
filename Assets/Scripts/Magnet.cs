using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Carryable
{
    public float force = 10;
    enum Direction
    {
        Left,
        Right,
        Up
    }

    public Animator animator;
    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    const float dstBetweenRays = .25f;

    private Direction _direction;
    private Player _player;
    private bool _active;

    private int _rayCount;
    private float _raySpacing;
    private Vector2 _topLeft;
    private Vector2 _topRight;
    private bool _foundMetal;
    private Energy _energy;

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        _topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        var bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        _rayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);
        _raySpacing = bounds.size.x / (_rayCount - 1);
    }

    void FindMetal()
    {
        for (int i = 0; i < _rayCount; i++)
        {
            Vector2 origin = Vector2.zero;
            switch (_direction)
            {
                case Direction.Up:
                    origin = _topLeft;
                    origin += Vector2.right * (_raySpacing * i);
                    break;
                case Direction.Left:
                    origin = _topLeft;
                    origin += Vector2.down * (_raySpacing * i);
                    break;
                case Direction.Right:
                    origin = _topRight;
                    origin += Vector2.down * (_raySpacing * i);
                    break;
            }

            var direction = GetDirectionVec();
            var hit = Physics2D.Raycast(origin, direction, 600, collisionMask);

            Debug.DrawRay(origin, direction, Color.blue);

            if (hit)
            {
                _foundMetal = true;
                return;
            }
        }
        _foundMetal = false;
    }

    public override bool Activate(Player player, Energy energy)
    {
        _energy = energy;
        _player = player;
        _active = true;
        animator.SetBool("active", true);

        return true;
    }

    private Vector3 GetDirectionVec()
    {
        switch (_direction)
        {
            case Direction.Left: return Vector3.left;
            case Direction.Right: return Vector3.right;
            case Direction.Up: return Vector3.up;
        }
        return Vector3.zero;
    }

    public override bool Deactivate()
    {
        animator.SetBool("active", false);
        _player.force = Vector3.zero;
        _player.EnableGravity = true;
        _player.EnableMovement = true;
        _active = false;
        return false;
    }

    private void Update()
    {
        if (_active)
        {
            _energy.UseMagnet();
            UpdateRaycastOrigins();
            FindMetal();

            if (_foundMetal)
            {
                if (_player.controller.collisions.above)
                {
                    _player.EnableMovement = false;
                }
                else
                {
                    _player.EnableMovement = true;
                }

                if ((_player.controller.collisions.left || _player.controller.collisions.right)
                    )// && _player.controller.collisions.hitCollider.gameObject.layer == LayerMask.NameToLayer("Metal"))
                {
                    _player.EnableGravity = false;
                }
                else
                {
                    _player.EnableGravity = true;
                }

                _player.force = GetDirectionVec() * force;
            }
            else
            {
                _player.EnableMovement = true;
                _player.EnableGravity = true;
                _player.force = Vector3.zero;
            }
        }
    }

    protected override void OnPickup()
    {
    }

    protected override void OnPutDown()
    {
        animator.transform.localRotation = Quaternion.identity;
    }

    public override void SetDirectionalInput(Controller2D controller, Vector2 directionalInput)
    {
        float rotation = 0.0f;
        if (directionalInput.y > 0)
        {
            rotation = 0;
            _direction = Direction.Up;
        }
        else
        {
            if (controller.collisions.faceDir == 1)
            {
                rotation = -90;
                _direction = Direction.Right;
            }
            else
            {
                rotation = 90;
                _direction = Direction.Left;
            }
        }

        animator.transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
