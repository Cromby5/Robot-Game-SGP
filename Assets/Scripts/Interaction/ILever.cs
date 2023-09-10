using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
public class ILever : AddDelegate
{
    [Header("Interactable")]
    [SerializeField] bool flipActive;

    bool active = false;
    private void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (!flipActive)
        {
            if (transform.eulerAngles.z < 180)
                ActivateLever();
            else
            {
                DeactivateLever();
            }
        }
        else
        {
            if (transform.eulerAngles.z > 180)
                ActivateLever();
            else
                DeactivateLever();
        }
    }

    private void ActivateLever()
    {
        if (active == true)
            return;

        active = true;
        Debug.Log("Lever Activated");
        CallDelegate();
    }

    private void DeactivateLever()
    {
        if (active == false)
            return;

        active = false;
        Debug.Log("Lever Deactivated");
        CallDelegate();
    }


    /*
    public Rigidbody leverRigidbody;

    void Update()
    {
        // Detect input (e.g., mouse click) to activate the lever
        if (Input.GetKey(KeyCode.Q))
        {
            leverRigidbody.AddTorque(10, 10, 10);
        }
        if (Input.GetKey(KeyCode.E))
            leverRigidbody.AddTorque(-10, -10, -10);
    }*/
}