using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChasePlayer : BaseAIBehaviour
{
    public float distanceToInRange = 1.0f;
    public float moveSpeed = 2.0f;

    Transform player;
    Rigidbody aiRb;

    public Transform SetPlayer { set => player = value; }

    private void Awake()
    {
        aiRb = GetComponent<Rigidbody>();
    }
    public override bool Action()
    {
        if (player == null)
            Debug.Log("Chase Player: Player not Found");

        if (InPlayerRange())
            return false;
        MoveTowardsPlayer();
        return true;
    }

    private bool InPlayerRange()
    {
        return Vector3.Distance(transform.position, player.position) < distanceToInRange;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 moveDirection = player.position - transform.position;
        aiRb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Impulse); 

        aiRb.velocity = Vector3.ClampMagnitude(aiRb.velocity, moveSpeed);
    }
}
