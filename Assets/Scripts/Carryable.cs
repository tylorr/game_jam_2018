using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Carryable : MonoBehaviour
{
    protected BoxCollider2D _collider;
    public BoxCollider2D Collider => _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public abstract bool Activate(Player player, Energy energy);
    public abstract bool Deactivate();
    
    public void Pickup()
    {
        OnPickup();
    }
    
    public void PutDown()
    {
        Deactivate();
        OnPutDown();
    }
    
    protected virtual void OnPickup() { }
    protected virtual void OnPutDown() { }
    
    public virtual void SetDirectionalInput(Controller2D controller, Vector2 directionalInput) { }
}
