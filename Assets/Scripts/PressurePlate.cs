using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private float height;

    enum PlateType
    {
        Wall,
        Floor,
    }

    void Start()
    {
        height = 0.1057129f; // use Renderer bounds? / height of the plate, change to find this instead of set value
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (height / 2), transform.position.z);
            // Colour / State Change
            // Activate
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (height / 2), transform.position.z);
            // Colour / State Change
            // Deactivate
        }
    }


}
