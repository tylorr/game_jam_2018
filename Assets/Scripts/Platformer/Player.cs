using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    public Energy energy;
    
    private Vector3 _velocity;
    public Vector3 Velocity => _velocity;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;

    Controller2D controller;

    private Vector2 _directionalInput;
    public Vector2 DirectionalInput => _directionalInput;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    public void Update()
    {
        CalculateVelocity();
        
        if (_directionalInput.x > 0.0f)
        {
            energy.Move();
        }

        controller.Move(_velocity * Time.deltaTime, _directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                _velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                _velocity.y = 0;
            }
        }
    }
    
    public void Stop()
    {
        _velocity = Vector2.zero;
        _directionalInput = Vector2.zero;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            energy.Jump();
            
            if (controller.collisions.slidingDownMaxSlope)
            {
                // not jumping against max slope
                if (_directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                {
                    _velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    _velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                _velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (_velocity.y > minJumpVelocity)
        {
            _velocity.y = minJumpVelocity;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = _directionalInput.x * moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        _velocity.y += gravity * Time.deltaTime;
    }
}
