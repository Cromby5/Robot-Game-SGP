using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRotation : MonoBehaviour
{
    public float minZRotation = -30.0f;
    public float maxZRotation = 30.0f;

    private void LateUpdate()
    {
        float tempMinZRotation = minZRotation;
        float tempMaxZRotation = maxZRotation;
        bool flip = false;
        // If either value is -, add 360 and invert directions
        if (minZRotation < 0)
        {
            flip = !flip;
            tempMinZRotation += 360;
        }
        if (maxZRotation < 0)
        {
            flip = !flip;
            tempMaxZRotation += 360;
        }

        Vector3 currentRotation = transform.eulerAngles;

        float targetZRotation;
        if (flip)
        {
            if (currentRotation.z > 180)
                targetZRotation = Mathf.Max(currentRotation.z, tempMinZRotation);
            else
                targetZRotation = Mathf.Min(currentRotation.z, tempMaxZRotation);
        }
        else
            targetZRotation = Mathf.Clamp(currentRotation.z, tempMinZRotation, tempMaxZRotation);


        Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetZRotation);
        transform.rotation = newRotation;
    }
}
