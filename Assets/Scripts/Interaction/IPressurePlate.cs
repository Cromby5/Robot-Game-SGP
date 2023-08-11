using System.Collections;
using System.Reflection;
using UnityEngine;
public class IPressurePlate : AddDelegate
{
    private void OnTriggerEnter(Collider other)
    {
        PressurePlateEffect(other);

        CallDelegate();
    }

    private void OnTriggerExit(Collider other)
    {
        StopPressurePlateEffect(other);

        CallDelegate();
    }
    void PressurePlateEffect(Collider hit)
    {
        if (hit.tag.Equals("Player"))
        {
            Debug.Log("Player On PressurePlate");
        }
    }
    void StopPressurePlateEffect(Collider hit)
    {
        if (hit.tag.Equals("Player"))
        {
            Debug.Log("Player Off PressurePlate");
        }
    }
}