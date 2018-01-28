using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(BoxCollider2D))]
public class CarryInterface : MonoBehaviour, IActivatable
{
    private Carryable _holding;
    private Carryable _touching;

    private PlayerAnimator _playerAnimator;
    private Controller2D _controller;
    private BoxCollider2D _collider;

    private Vector2 _originalOffset;
    private Vector2 _originalSize;

    private void Awake()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        _controller = GetComponent<Controller2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
    }

    public bool Activate(Energy energy)
    {
        if (_holding != null)
        {
            return _holding.Activate(GetComponent<Player>(), energy);
        }
        return false;
    }

    public bool Deactivate()
    {
        if (_holding != null)
        {
            _holding.Deactivate();
            return true;
        }
        return false;
    }

    public void OnCarryInput()
    {
        if (_touching && !_holding)
        {
            _originalSize = _collider.size;
            _originalOffset = _collider.offset;

            _playerAnimator.Pickup(_touching);
            _holding = _touching;
            _holding.Pickup();
            var bounds = _collider.bounds;
            bounds.Encapsulate(_holding.Collider.bounds);
            ResizeCollider(bounds);
        }
        else if (_holding)
        {
            _playerAnimator.Putdown(_holding);
            _holding.PutDown();
            _holding = null;
            _collider.offset = _originalOffset;
            _collider.size = _originalSize;
            _controller.CalculateRaySpacing();
        }
    }

    private void ResizeCollider(Bounds bounds)
    {
        _collider.offset = bounds.center - transform.position;
        _collider.size = bounds.size;
        _controller.CalculateRaySpacing();
    }

    public void SetDirectionalInput(Vector2 directionalInput)
    {
        if (_holding)
        {
            _holding.SetDirectionalInput(_controller, directionalInput);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _touching = other.GetComponent<Carryable>();
        if (_touching != null)
        {
            Debug.Log("touching carryable");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var carryable = other.GetComponent<Carryable>();
        if (carryable == _touching)
        {
            _touching = null;
        }
    }
}
