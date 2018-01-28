using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool active = false;
    public Sprite outletOn;
    public Checkpoint next;
    
    public void Start()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().sprite = outletOn;
        }
    }
    
    public void TurnOn()
    {
        active = true;
        GetComponent<SpriteRenderer>().sprite = outletOn;
    }
}
