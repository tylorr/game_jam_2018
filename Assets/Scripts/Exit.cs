using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerPlatformerController>();
        if (player != null)
        {
            var door = GetComponent<Door>();
            var room = door.room;
            player.Respawn(room.spawnLocation);
        }
    }
}
