using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Carryable
{
    enum Direction
    {
        Left,
        Right,
        Up   
    }

    public Animator animator;

    private Direction _direction;
    
    public override bool Activate(Energy energy)
    {
        throw new System.NotImplementedException();
    }

    public override bool Deactivate()
    {
        //throw new System.NotImplementedException();
        return false;
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
