using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public PlayerPlatformerController player;
    public Energy energy;
    public Transform spawnLocator;
    
    void Update()
    {
        if (energy.Empty)
        {
            energy.Restore();
            player.Respawn(spawnLocator);
        }
    }
}
