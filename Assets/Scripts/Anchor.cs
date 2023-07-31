using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;

    [SerializeField] private HingeJoint extendPoint;
    [SerializeField] private Rigidbody extendRB;


    public void AnchorWire(GameObject plug)
    {
        plug.transform.SetPositionAndRotation(anchorPoint.position, anchorPoint.rotation);
        Rigidbody rb = plug.GetComponent<Rigidbody>();
        if (extendPoint != null && extendRB != null)
        {
            extendPoint.connectedBody = rb;
            extendRB.isKinematic = false;
        }
        else // Fallback
        {
            rb.isKinematic = true; 
        }
    }

    public void DetachWire(GameObject plug)
    {
        if (extendPoint != null && extendRB != null)
        {
            extendPoint.connectedBody = null;
            extendRB.isKinematic = true;
            extendRB.MovePosition(anchorPoint.position);
        }
        else // Fallback
        {
            Rigidbody rb = plug.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }
}
