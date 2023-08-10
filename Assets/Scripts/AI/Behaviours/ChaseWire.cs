using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChaseWire : BaseAIBehaviour
{
    public float distanceToInRange = 1.0f;
    public float moveSpeed = 2.0f;

    Transform wire;
    Rigidbody aiRb;

    public Transform SetWire { set => wire = value; }

    private void Awake()
    {
        aiRb = GetComponent<Rigidbody>();
    }
    public override bool Action()
    {
        if (wire == null)
            Debug.Log("Chase Wire: Wire not Found");

        if (InWireRange())
            return false;
        MoveTowardsWire();
        return true;
    }

    private bool InWireRange()
    {
        return Vector3.Distance(transform.position, wire.position) < distanceToInRange;
    }

    private void MoveTowardsWire()
    {
        Vector3 moveDirection = wire.position - transform.position;
        aiRb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Impulse);

        aiRb.velocity = Vector3.ClampMagnitude(aiRb.velocity, moveSpeed);
    }
}
