using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DoorInterface))]
[RequireComponent(typeof(CheckpointInterface))]
public class PlayerInput : MonoBehaviour
{
    public Energy energy;
    
    private Player _player;
    private DoorInterface _doorInterface;
    private CheckpointInterface _checkpointInterface;
    
    private IActivatable holdingObject;

    void Awake()
    {
        _player = GetComponent<Player>();
        _doorInterface = GetComponent<DoorInterface>();
        _checkpointInterface = GetComponent<CheckpointInterface>();
    }

    void Update()
    {
        var directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.OnJumpInputDown();
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _player.OnJumpInputUp();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _doorInterface.OnOpenDoorInput();
        }
        
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F))
        {
            if (holdingObject != null)
            {
                holdingObject.Activate(energy);
            }
            else
            {
                _checkpointInterface.Activate(energy);
            }
        }
    }
}
