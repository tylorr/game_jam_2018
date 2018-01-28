using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class GravityController : MonoBehaviour
{
    public float gravity = 6;

    public bool EnableGravity { get; set; } = true;

    private Vector3 _velocity;

    [HideInInspector]
    public Controller2D _controller;

    void Awake()
    {
        gravity = -gravity;
        _controller = GetComponent<Controller2D>();
    }

    public void Update()
    {
        CalculateVelocity();
        _controller.Move(_velocity * Time.deltaTime, Vector2.zero);

        if (_controller.collisions.above || _controller.collisions.below)
        {
            _velocity.y = 0;
        }
    }

    public void Stop()
    {
        _velocity = Vector2.zero;
    }

    void CalculateVelocity()
    {
        if (EnableGravity)
        {
            _velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            _velocity.y = 0;
        }
    }
}
