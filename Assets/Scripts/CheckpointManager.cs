using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public PlayerAnimator playerAnimator;
    public Energy energy;
    public Transform spawnLocator;
    
    void Update()
    {
        if (energy.Empty)
        {
            energy.Restore();
            playerAnimator.Respawn(spawnLocator);
        }
    }
}
