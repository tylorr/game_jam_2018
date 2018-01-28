using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class DoorInterface : MonoBehaviour
{
    private PlayerAnimator _animator;
    private Door _door;

    private void Awake()
    {
        _animator = GetComponent<PlayerAnimator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _door = collision.GetComponent<Door>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Door>() != null)
        {
            _door = null;
        }
    }

    public void OnOpenDoorInput()
    {
        if (_door != null)
        {
            _animator.EnterRoom(_door.room);
        }
    }
}
