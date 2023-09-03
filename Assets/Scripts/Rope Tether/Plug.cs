using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public static bool isAttached { get; private set; } = false; // 3 States for needed for this, big = restore, small = keep current charge, detached = decrease charge

    private Anchor currentAnchor;
    
    private void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attach Point"))
        {
            isAttached = true;
            currentAnchor = other.GetComponent<Anchor>();
            currentAnchor.AnchorWire(gameObject);
            // Set this anchor as the current checkpoint
            Debug.Log("Plug Attached");
        }

    }
    public void Detach()
    {
        currentAnchor.DetachWire(gameObject);
        isAttached = false;
        currentAnchor = null;
        Debug.Log("Plug Detached");
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Attach Point"))
        {
            isAttached = false;
            other.GetComponent<Anchor>().DetachWire(gameObject);
            Debug.Log("Plug Detached");
        }           
    }
    */
    
}
