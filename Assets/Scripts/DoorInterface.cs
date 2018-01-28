using UnityEngine;

public class DoorInterface : MonoBehaviour
{
    public new DeadzoneCamera camera;
    private Door _door;
    
    void Update()
    {
        if (_door != null && Input.GetKeyDown(KeyCode.DownArrow))
        {
            var room = _door.room;
            camera.limits.Add(room.GetBoundingRect());
            GetComponent<PlayerPlatformerController>().Respawn(room.spawnLocation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Started touching door");
        _door = collision.GetComponent<Door>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Door>() != null)
        {
            Debug.Log("Stopped touching door");
            _door = null;
        }
    }
}
