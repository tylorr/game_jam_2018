using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PdaMessager : MonoBehaviour {
    public GameObject root;
    public Text text;

	public void Show(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        
        root.gameObject.SetActive(true);
        text.text = message;
    }
    
    public void Hide()
    {
        root.gameObject.SetActive(false);
    }
}
