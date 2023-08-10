using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthAI : BaseAI
{
    [SerializeField] ChaseWire chaseWire;
    [SerializeField] RunAway runAway;

    Plug playerPlug;
    LineOfSight playerLineOfSight;

    bool runningAway = false;
    bool active = false;
    private void Awake()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        playerLineOfSight = FindObjectOfType<LineOfSight>();
        if (playerLineOfSight == null)
            throw new Exception("Player Line of Sight not found.");
        playerPlug = FindObjectOfType<Plug>();
        if (playerPlug == null)
            throw new Exception("Player not found.");

        chaseWire.SetWire = playerPlug.transform;
        runAway.SetPlayer = playerPlug.transform.root;
    }
    private void TryActivate()
    {
        if (Plug.isAttached)
            active = true;
        else if (PlayerIsLooking())
        {
            runAway.Action();
            runningAway = true;
        }
    }
    private void TryDeactivate()
    {
        if (!Plug.isAttached)
        {
            active = false;

            runningAway = false;
        }
    }
    public override void CooperativeArbitration()
    {
        if (!active)
        {
            TryActivate();
            return;
        }
        else 
            TryDeactivate();


        if (PlayerIsLooking())
            runningAway = true;

        if (runningAway)
        {
            runAway.Action();
            return;
        }

        if (!chaseWire.Action())
            EatCable();
    }

    private void EatCable()
    {
        playerPlug.Detach();
        runningAway = true;
    }

    private bool PlayerIsLooking()
    {
        if (playerLineOfSight.InSight(gameObject))
        {
            return true;
        }
        return false;
    }
}
