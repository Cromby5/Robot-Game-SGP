using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    
    public GameObject startPoint; // Player? / backpack
    public GameObject endPrefab; // Plug end

   
    public HingeJoint top; // Current top most rope segment 

    public enum JointType
    {
        Configurable,
        Hinge
    }
    [Header("Joint Type To Use")]
    public JointType selectedJoint = 0;

    public void GenerateWire()
    {
        // Clear wire
        if (wirePoints != null)
        {
            foreach (GameObject wirePoint in wirePoints)
            {
                DestroyImmediate(wirePoint);
            }
        }
        wirePoints = new List<GameObject>(length);
        // Generate wire points
        for (int i = 0; i < length; i++)
        {
            if (i == length - 1)
            {
                wirePoints.Add(Instantiate(endPrefab, transform));
            }
            else
            {
                wirePoints.Add(Instantiate(wirePointPrefab, transform));
            }
            wirePoints[i].transform.parent = transform;
            wirePoints[i].name = "WirePoint" + i;
            wirePoints[i].transform.localPosition = new Vector3(0, i * yOffset, 0);
            //wirePoints[i].transform.position = transform.position;

            switch (selectedJoint)
            {
                case JointType.Configurable:
                    // Add configurable joint
                    ConfigurableJoint cj = wirePoints[i].AddComponent<ConfigurableJoint>();
                    if (i > 0)
                    {
                        ConfigureJointSet(cj, wirePoints[i - 1].GetComponent<Rigidbody>());
                    }
                    else
                    {
                        ConfigureJointSet(cj, startPoint.GetComponent<Rigidbody>());
                        //top = cj;
                    }
                break;
                case JointType.Hinge:
                    HingeJoint hj = wirePoints[i].AddComponent<HingeJoint>();
                    if (i > 0)
                    {
                        HingeJointSet(hj, wirePoints[i - 1].GetComponent<Rigidbody>());
                        //hj.connectedAnchor = new Vector3(0f, 2f, 0f);
                  
                        
                    }
                    else
                    {
                        HingeJointSet(hj, startPoint.GetComponent<Rigidbody>());
                        //hj.connectedAnchor = new Vector3(0f, 2f, 0f);
         
                        
                        //top = hj;
                    }
                break;
            }

            // Add hinge joint
           

        }
    }

    // Adding / Removing segments when rope in pulled in / out
    public void AddSegment() // Add at the top
    {
       wirePoints.Insert(0, Instantiate(wirePointPrefab));
 
       wirePoints[0].transform.parent = transform;
       wirePoints[0].name = "WirePoint";
       wirePoints[0].transform.position = transform.position;
       
       switch (selectedJoint)
       {
            case JointType.Configurable:
                
                break;

            case JointType.Hinge:
                HingeJoint hj = wirePoints[0].AddComponent<HingeJoint>();
                HingeJointSet(hj, startPoint.GetComponent<Rigidbody>());
                wirePoints[1].GetComponent<HingeJoint>().connectedBody = hj.GetComponent<Rigidbody>();
                hj.connectedAnchor = new Vector3(0f, 1.7f, 0f);
                break;
        }

       // connected below = top
       // top connects to the new link
       // reset the anchor point!!!!
       //wirePoints[1].GetComponent<HingeJoint>().connectedAnchor = new Vector3(0f, 0f, 0f);
       //top = hj;

    }
    public void RemoveSegment() // Remove from the top
    {
        HingeJoint newTop = wirePoints[1].GetComponent<HingeJoint>();
        // set newtop to connect to the hook
        newTop.connectedBody = startPoint.GetComponent<Rigidbody>();
        // set the newtop pos to the hook pos
        newTop.transform.position = startPoint.transform.position;
        // reset the newtops anchor!!!!!
        //newTop.connectedAnchor = new Vector3(0f, 0f, 0f);
        
        // destroy the top object
        Destroy(wirePoints[0]);
        wirePoints.RemoveAt(0);
        //top = newTop;
    }

    //https://www.youtube.com/watch?v=Psq8rICishw&t
    public void ConfigureJointSet(ConfigurableJoint j, Rigidbody c)
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

    public void HingeJointSet(HingeJoint j, Rigidbody c)
    {
        j.axis = new Vector3(0f, 0f, 1f);
        j.useSpring = true;
        j.spring = new JointSpring { spring = 1f, damper = 1f, targetPosition = 1f};

        j.autoConfigureConnectedAnchor = true;
       
        j.enableCollision = true;
        j.enablePreprocessing = false;
        j.connectedBody = c;
    }
}

[CustomEditor(typeof(WirePhysics))]
public class WireEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WirePhysics wire = (WirePhysics)target;

        if (GUILayout.Button("Generate Wire"))
        {
            wire.GenerateWire();
        }
    }
}
