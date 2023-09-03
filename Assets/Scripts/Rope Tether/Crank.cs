using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will pull in / pull out the rope
public class Crank : MonoBehaviour
{
    public float rotationSpeed;
    
    private WirePhysics wire;
    
    void Awake()
    {
        wire = transform.parent.GetComponent<WirePhysics>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
    }

    public void Rotate (int direction)
    {
        if (direction > 0)
        {
            //transform.Rotate(0, 0, direction * rotationSpeed);
            wire.AddSegment();
        }
        else if (direction < 0)
        {
            //transform.Rotate(0, 0, direction * rotationSpeed);
            wire.RemoveSegment();
        }
    }
        
    
}
