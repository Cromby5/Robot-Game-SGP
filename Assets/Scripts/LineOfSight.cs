using System.Collections.Generic;
using UnityEngine;

// TODO: REMOVE IF LAG
[ExecuteInEditMode]
public class LineOfSight : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform head;
    [Header("Settings")]
    [SerializeField] Color sensorColour = Color.green;
    [SerializeField] float angle = 30;
    [SerializeField] float distance = 10;
    [SerializeField] float height = 1.0f;
    [SerializeField] int segments = 10;
    [SerializeField] int scanFrequency = 30;
    [Header("Layers")]
    [SerializeField] LayerMask targetLayers;
    [SerializeField] LayerMask blockLayers;
    [SerializeField] bool drawLOS = false;
    [SerializeField] bool drawHit = false;

    private int count;
    private float scanInterval;
    private float scanTimer;
    private Mesh Sensor;

    [HideInInspector] public Collider[] colliders = new Collider[40];
    private List<GameObject> objs = new List<GameObject>();
    public List<GameObject> Objs { get => objs; }

    private void OnValidate()
    {
        Sensor = CreateSensorMesh();
    }

    private void Awake()
    {
        // Set starting scan interval
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        // Follow head
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, head.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }
    private bool InSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        // If the object is above or below line of sight
        if (direction.y < 0 || direction.y > height)
            return false;

        // Reset y direction so that our delta angle is not affected
        direction.y = 0;
        // Calculate delta angle
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        // If object is outside line of sight. 
        if (deltaAngle > angle)
            return false;

        // Place origin in the center of object
        origin.y += height / 2;
        // Reflect this in destination
        destination.y = origin.y;
        // Shoot a linecast from our origin to our destination and check for any block layers
        if (Physics.Linecast(origin, destination, blockLayers))
            return false;

        return true;
    }

    private void Scan()
    {
        objs.Clear();

        // Check in sphere for colliders
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayers, QueryTriggerInteraction.Collide);

        // Loop through objects
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;

            // Check if in light of sight
            if (InSight(obj))
            {
                // Add to list
                objs.Add(obj);
            }
        }
    }

    private Mesh CreateSensorMesh()
    {
        Mesh mesh = new Mesh();

        // Declare
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int index = 0;

        // Create left side
        vertices[index++] = bottomCenter;
        vertices[index++] = bottomLeft;
        vertices[index++] = topLeft;

        vertices[index++] = topLeft;
        vertices[index++] = topCenter;
        vertices[index++] = bottomCenter;

        // Create right side
        vertices[index++] = bottomCenter;
        vertices[index++] = topCenter;
        vertices[index++] = topRight;

        vertices[index++] = topRight;
        vertices[index++] = bottomRight;
        vertices[index++] = bottomCenter;

        // Flip angle and generate deltaAngle
        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            // Reset
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // Create far side
            vertices[index++] = bottomLeft;
            vertices[index++] = bottomRight;
            vertices[index++] = topRight;

            vertices[index++] = topRight;
            vertices[index++] = topLeft;
            vertices[index++] = bottomLeft;

            // Create top side
            vertices[index++] = topCenter;
            vertices[index++] = topLeft;
            vertices[index++] = topRight;

            // Create bottom side
            vertices[index++] = bottomCenter;
            vertices[index++] = bottomRight;
            vertices[index++] = bottomLeft;

            // Increment angle
            currentAngle += deltaAngle;
        }
        
        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        // Set
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnDrawGizmos()
    {
        // If we have generated the mesh
        if (Sensor && drawLOS)
        {
            // Draw mesh
            Gizmos.color = sensorColour;
            Gizmos.DrawMesh(Sensor, transform.position, transform.rotation);
        }

        if (drawHit)
        {
            // Render all spheres nearby
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distance);
            for (int i = 0; i < count; ++i)
            {
                Gizmos.DrawSphere(colliders[i].transform.position, 1.0f);
            }

            Gizmos.color = sensorColour;

            // Render Spheres at all objects within Line of Sight.
            objs.RemoveAll(s => s == null);
            foreach (var Object in objs)
            {
                if (Object == null)
                {
                    objs.Remove(Object);
                    continue;
                }

                Gizmos.DrawSphere(Object.transform.position, 1.0f);
            }
        }
    }
}
