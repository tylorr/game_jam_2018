using UnityEngine;

public class Commands : MonoBehaviour
{
    public Energy energy;
    public CheckpointInterface checkpointInterface;
    
    private IActivatable holdingObject;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F))
        {
            if (holdingObject != null)
            {
                holdingObject.Activate(energy);
            }
            else
            {
                checkpointInterface.Activate(energy);
            }
        }
    }
}
