using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool active = false;
    public Sprite outletOn;
    public Checkpoint next;

    public string message;
    
    public void Start()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().sprite = outletOn;
            FindObjectOfType<SystemMessager>().Show(message);
        }
    }
    
    public void TurnOn()
    {
        active = true;
        GetComponent<SpriteRenderer>().sprite = outletOn;
    }
}
