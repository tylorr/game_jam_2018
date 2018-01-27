using System;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public float maxEnergy;
    public float jumpCost;
    public float moveCost;
    
    public Slider energyBar;
    
    private float _energy;
    
    public float Amount => _energy;
    public bool Empty => _energy <= 0;

    private void Awake()
    {
        _energy = maxEnergy;
    }

    public void Restore()
    {
        _energy = maxEnergy;
    }

    public void Jump()
    {
        AddEnergy(-jumpCost);
    }
    
    public void Move()
    {
        AddEnergy(-moveCost * Time.deltaTime);
    }
    
    private void AddEnergy(float amount)
    {
        _energy = Mathf.Clamp(_energy + amount, 0, maxEnergy);
    }
    
    private void Update()
    {
        energyBar.value = Mathf.InverseLerp(0, maxEnergy, _energy);
    }
}
