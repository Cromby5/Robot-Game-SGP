using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RunAway : BaseAIBehaviour
{
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
            Debug.Log("Run Away: Player not Found");
        RunAwayFromPlayer();
        return true;
    }

    private void RunAwayFromPlayer()
    {
        Vector3 moveDirection = transform.position - player.position;
        aiRb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Impulse);

        aiRb.velocity = Vector3.ClampMagnitude(aiRb.velocity, moveSpeed);
    }
}
