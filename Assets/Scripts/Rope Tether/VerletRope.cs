using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private List<RopeSegment> nodes = new List<RopeSegment>();
    [SerializeField] private float nodeLength = 0.25f; // length between each point
    [Range(1, 50)] // more than 50 breaks?
    [SerializeField] private int segmentLength = 35; // number of segments to create
    private float lineWidth = 0.1f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float maxDistanceBetweenPoints;

    private Vector3 forceGravity = new Vector3(0f, -1f, 0f);
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position;

        for (int i = 0; i < segmentLength; i++)
        {
            nodes.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= nodeLength;
        }
    }

    void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();

        for (int i = 0; i < 60; i++)
        {
            ApplyConstraint();
            DistanceCheck();
            if (i % 2 == 1)
                CollisionCheck();
        }
       
    }

    private void Simulate()
    {
        
        for (int i = 0; i < segmentLength; i++)
        {
            RopeSegment firstSegment = nodes[i];
            Vector3 velocity = firstSegment.currPos - firstSegment.prevPos;
            firstSegment.prevPos = firstSegment.currPos;
            
            firstSegment.currPos += velocity;
            firstSegment.currPos += forceGravity * Time.deltaTime;
            nodes[i] = firstSegment;

            // start collision here? start from direction line 79
            
        }
    }
    private void CollisionCheck()
    {

    }

    private void DistanceCheck()
    {
        RopeSegment first = nodes[0];
        RopeSegment last = nodes[segmentLength - 1];
        // Same distance calculation as above, but less optimal.
        float distance = Vector3.Distance(first.currPos, last.currPos);
        if (distance > 0 && distance > segmentLength * nodeLength)
        {
            Vector3 dir = (last.currPos - first.currPos).normalized;
            last.currPos = first.currPos + segmentLength * nodeLength * dir;
            endPoint.position = last.currPos;
        }
    }

    private void ApplyConstraint()
    {
        // First Point Lock
        RopeSegment firstSegment = nodes[0];
        firstSegment.currPos = startPoint.position;
        nodes[0] = firstSegment;
        // Second Point Lock
        RopeSegment endSegment = nodes[segmentLength - 1];
        endSegment.currPos = endPoint.position;
        nodes[segmentLength - 1] = endSegment;

        for (int i = 0; i < segmentLength - 1; i++)
        {
            RopeSegment firstSeg = nodes[i];
            RopeSegment secondSeg = nodes[i + 1];

            float dist = (firstSeg.currPos - secondSeg.currPos).magnitude;
            float error = Mathf.Abs(dist - nodeLength);
            Vector3 changeDir = Vector3.zero;

            if (dist > nodeLength)
            {
                changeDir = (firstSeg.currPos - secondSeg.currPos).normalized;
            }
            else if (dist < nodeLength)
            {
                changeDir = (secondSeg.currPos - firstSeg.currPos).normalized;
            }

            Vector3 changeAmount = changeDir * error;
            // Apply correction???
            if (i != 0)
            {
                firstSeg.currPos -= changeAmount * 0.5f;
                nodes[i] = firstSeg;
                secondSeg.currPos += changeAmount * 0.5f;
                nodes[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.currPos += changeAmount;
                nodes[i + 1] = secondSeg;
            }
        }
    }
    
    private void DrawRope()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[segmentLength];
        for (int i = 0; i < segmentLength; i++)
        {
            ropePositions[i] = nodes[i].currPos;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
        
    }
    // Lets you see each segment clearly
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < segmentLength - 1; i++)
        {
            if (i % 2 == 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            Gizmos.DrawLine(nodes[i].currPos, nodes[i + 1].currPos);
        }
    }

    public class RopeSegment
    {
        public Vector3 currPos;
        public Vector3 prevPos;

        public RopeSegment(Vector3 pos)
        {
            this.currPos = pos;
            this.prevPos = pos;
        }
    }

    enum ColliderType
    {
        Circle,
        Box,
        None,
    }

    class CollisionInfo
    {
        public int id;

        public ColliderType colliderType;
        public Vector3 colliderSize;
        public Vector3 position;
        public Vector3 scale;
        public Matrix4x4 wtl;
        public Matrix4x4 ltw;
        public int numCollisions;
        public int[] collidingNodes; // You probably want to use byte[] here instead, unless you have >255 nodes.

        public CollisionInfo(int maxCollisions)
        {
            id = -1;
            colliderType = ColliderType.None;
            colliderSize = Vector3.zero;
            position = Vector3.zero;
            scale = Vector3.zero;
            wtl = Matrix4x4.zero;
            ltw = Matrix4x4.zero;

            numCollisions = 0;
            collidingNodes = new int[maxCollisions];
        }
    }


}
