using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRotation : MonoBehaviour
{
    public Vector3 rotationMin;
    public Vector3 rotationMax;

    private void FixedUpdate()
    {
        Debug.Log("max: " + rotationMax.z);
        Debug.Log("curr: " + transform.eulerAngles.z);
        Vector3 currLocation = transform.localEulerAngles;
        float x = Mathf.Clamp(currLocation.x, rotationMin.x + 360, rotationMax.x );
        float y = Mathf.Clamp(currLocation.y, rotationMin.y + 360, rotationMax.y);
        float z = Mathf.Clamp(currLocation.z, rotationMin.z + 360, rotationMax.z);
        transform.localRotation = Quaternion.Euler(new Vector3(x,y,z));
    }
}
