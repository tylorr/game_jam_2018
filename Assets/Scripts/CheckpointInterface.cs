using UnityEngine;
using System.Collections;

public interface IActivatable
{
    bool Activate(Energy energy);
    bool Deactivate();
}

[RequireComponent(typeof(PlayerAnimator))]
public class CheckpointInterface : MonoBehaviour, IActivatable
{
    public CheckpointManager checkpointManager;
    private Checkpoint _checkpoint;
    private bool _animating;
    
    private PlayerAnimator _playerAnimator;
    
    private void Awake()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
    }

    public bool Activate(Energy energy)
    {
        if (_checkpoint != null && _checkpoint.active)
        {
            checkpointManager.Use(_checkpoint);
            return _playerAnimator.Plugin(energy);
        }
        return false;
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
