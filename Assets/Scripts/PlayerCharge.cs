using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharge : MonoBehaviour
{
    // Players battery, decreases when not attached, when shooting gun, when it reaches 0 the player dies 

    private float charge;
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float chargeDecreaseRate = 4f;

    [SerializeField] private Slider chargeBar;

    public delegate void OnDeath();
    public static event OnDeath onDeath;

    void Start()
    {
        charge = maxCharge;
    }

    void Update()
    {
        if (charge > 0 && Plug.isAttached == false)
        {
            charge -= chargeDecreaseRate * Time.deltaTime;
        }
        else if (charge > 0 && Plug.isAttached == true)
        {
            if (charge < maxCharge)
            {
                charge += chargeDecreaseRate * Time.deltaTime;
            }
            else if (charge > maxCharge)
            {
                charge = maxCharge;
            }
        }
        else
        {
            onDeath?.Invoke();
        }
        chargeBar.value = charge;
    }

    public void DecreaseCharge(float damage)
    {
        charge -= damage;
    }
        
    public void UpdateCharge(float newMax)
    {
        maxCharge = newMax;
        charge = maxCharge;
        chargeBar.maxValue = newMax;
    } 
}

