using UnityEngine;
using UniRx;

public interface IActivatable
{
    bool Activate(Energy energy);
    bool Deactivate();
}

public class CheckpointInterface : MonoBehaviour, IActivatable
{
    public float chargeDuration = 5.0f;
    public float pluginDuration = 1.5f;
    public Animator animator;
    public PlayerPlatformerController player;
    
    private Checkpoint _checkpoint;
    private bool _animating;

    public bool Activate(Energy energy)
    {
        if (_checkpoint != null && !_animating)
        {
            Animate(energy);
            return true;
        }
        return false;
    }

    private async void Animate(Energy energy)
    {
        _animating = true;
        player.CanMove = false;
        
        animator.SetTrigger("plugin");
        
        await new WaitForSeconds(pluginDuration);

        // TODO: Animate energy
        energy.AddEnergy(energy.maxEnergy);
        await new WaitForSeconds(chargeDuration - pluginDuration);

        animator.SetTrigger("unplug");
        var unplugDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        await new WaitForSeconds(unplugDuration);

        player.CanMove = true;
        _animating = false;
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
