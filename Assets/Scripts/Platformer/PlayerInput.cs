using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DoorInterface))]
[RequireComponent(typeof(CheckpointInterface))]
[RequireComponent(typeof(CarryInterface))]
public class PlayerInput : MonoBehaviour
{
    public Energy energy;
    
    private Player _player;
    private DoorInterface _doorInterface;
    private CheckpointInterface _checkpointInterface;
    private CarryInterface _carryInterface;

    private IActivatable _active = null;

    void Awake()
    {
        _player = GetComponent<Player>();
        _doorInterface = GetComponent<DoorInterface>();
        _checkpointInterface = GetComponent<CheckpointInterface>();
        _carryInterface = GetComponent<CarryInterface>();
    }

    void Update()
    {
        var directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.SetDirectionalInput(directionalInput);
        _carryInterface.SetDirectionalInput(directionalInput);

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
        
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.E))
        {
            _carryInterface.OnCarryInput();
        }
        
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F))
        {
            if (_active != null)
            {
                _active.Deactivate();
                _active = null;
            }
            else
            {
                if (_carryInterface.Activate(energy))
                {
                    _active = _carryInterface;
                }
                else if (_checkpointInterface.Activate(energy))
                {
                    _active = _checkpointInterface;
                }
            }
        }
    }
}
