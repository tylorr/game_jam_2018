using UnityEngine;
using System.Collections;

public interface IActivatable
{
    void Activate(Energy energy);
    bool Deactivate();
}

[RequireComponent(typeof(PlayerAnimator))]
public class CheckpointInterface : MonoBehaviour, IActivatable
{
    private Checkpoint _checkpoint;
    private bool _animating;
    
    private PlayerAnimator _playerAnimator;
    private Controller2D _controller;
    
    private void Awake()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void Activate(Energy energy)
    {
        if (_checkpoint != null)
        {
            _playerAnimator.Plugin(energy);
        }
    }

    public bool Deactivate()
    {
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _checkpoint = other.GetComponent<Checkpoint>();
    }

    private void OnTriggerExit(Collider other)
    {
        var checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            _checkpoint = null;
        }
    }
}
