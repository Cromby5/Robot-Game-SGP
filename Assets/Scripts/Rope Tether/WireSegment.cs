using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSegment : MonoBehaviour
{
    private GameObject belowConnection, aboveConnection;
    

    // Start is called before the first frame update
    void Start()
    {
        //ResetAnchor();
    }

    public void ResetAnchor()
    {
        aboveConnection = GetComponent<HingeJoint>().connectedBody.gameObject;
        if (aboveConnection.TryGetComponent<WireSegment>(out var aboveWireSegment))
        {
            aboveWireSegment.belowConnection = gameObject;
            //get the size of the above object
            
        }
    }



}
