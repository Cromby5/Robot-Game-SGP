using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePhysics : MonoBehaviour
{
    // Not actually for wire physics at the moment, might never be.
    // more for setting up joints which can be seen as controlling the physics
    
    // Length of wire, unity slider
    [Range(0f, 50.0f)]
    public int length;
    [Range(0f, 1.0f)]
    public float yOffset;

    [SerializeField] private List<GameObject> wirePoints;

    public GameObject wirePointPrefab;
    
    public GameObject startPrefab; // Player
    public GameObject endPrefab; // Plug end

    public void GenerateWire()
    {
        wirePoints = new List<GameObject>(length);
        // Destroy old wire, all children objects
        foreach (GameObject child in transform)
        {
            Destroy(child);
            wirePoints.Remove(child);
        }
        // Generate Start (Player)
        // Generate wire points
        for (int i = 0; i < length; i++)
        {
            wirePoints.Add(Instantiate(wirePointPrefab, transform));
            wirePoints[i].transform.parent = transform;
            wirePoints[i].name = "WirePoint" + i;
            wirePoints[i].transform.localPosition = new Vector3(0, i * yOffset, 0);
            //Add configurable joint
            if (i > 0)
            {
                ConfigurableJoint cj = wirePoints[i].AddComponent<ConfigurableJoint>();
                ConfigureJoint(cj, wirePoints[i - 1].GetComponent<Rigidbody>());
            }
        }
        // Generate End (Plug)
    }

    void Start()
    {
        GenerateWire();
    }


    //https://www.youtube.com/watch?v=Psq8rICishw&t
    public void ConfigureJoint(ConfigurableJoint j, Rigidbody c)
    {
        j.xMotion = ConfigurableJointMotion.Locked;
        j.yMotion = ConfigurableJointMotion.Locked;
        j.zMotion = ConfigurableJointMotion.Locked;

        j.angularZMotion = ConfigurableJointMotion.Limited;
        j.angularYMotion = ConfigurableJointMotion.Limited;
        j.angularXMotion = ConfigurableJointMotion.Limited;

        j.linearLimitSpring = new SoftJointLimitSpring { spring = 1f, damper = 1f };

        j.projectionMode = JointProjectionMode.PositionAndRotation;
        j.enableCollision = true;
        j.enablePreprocessing = false;
        j.connectedBody = c;
    }
}
