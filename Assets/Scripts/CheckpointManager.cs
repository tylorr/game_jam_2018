using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public PlayerAnimator playerAnimator;
    public Energy energy;
    public Checkpoint checkpoint;
    
    void Update()
    {
        if (energy.Empty)
        {
            energy.Restore();
            playerAnimator.Respawn(checkpoint.transform);
        }
    }

    public void Use(Checkpoint checkpoint)
    {
        checkpoint.next.TurnOn();
        this.checkpoint = checkpoint;
    }
}
