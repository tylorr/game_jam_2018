using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Door door;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerAnimator>();
        if (player != null)
        {
            player.Respawn(door.spawnLocator);
        }
    }
}
