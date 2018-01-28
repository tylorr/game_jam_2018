using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDA : Carryable
{
    public string message;
    
    public override bool Activate(Player player, Energy energy)
    {
        FindObjectOfType<PdaMessager>().Show(message);
        return true;
    }

    public override bool Deactivate()
    {
        FindObjectOfType<PdaMessager>().Hide();
        return true;
    }
}
