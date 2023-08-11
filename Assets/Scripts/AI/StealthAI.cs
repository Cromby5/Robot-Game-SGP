using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthAI : BaseAI
{
    [SerializeField] ChaseWire chaseWire;
    [SerializeField] RunAway runAway;

    GameObject player;
    Plug playerPlug;
    LineOfSight playerLineOfSight;
    LineOfSight aiLineOfSight;

    bool runningAway = false;
    bool active = false;
    private void Awake()
    {
        FindPlayer();
    }

    private void FindPlayer()
    { 
        playerPlug = FindObjectOfType<Plug>();
        if (playerPlug == null)
            throw new Exception("Player not found.");

        player = playerPlug.transform.root.GetChild(0).gameObject;

        chaseWire.SetWire = playerPlug.transform;
        runAway.SetPlayer = player.transform;

        playerLineOfSight = player.GetComponentInChildren<LineOfSight>();
        if (playerLineOfSight == null)
            throw new Exception("Player Line of Sight not found.");

        aiLineOfSight = GetComponentInChildren<LineOfSight>();
        if (aiLineOfSight == null)
            throw new Exception("AI Line of Sight not found.");
    }
    private void TryActivate()
    {
        if (Plug.isAttached && aiLineOfSight.Objs.Count >= 1)
            active = true;
        else if (PlayerIsLooking())
        {
            runAway.Action();
            runningAway = true;
        }
    }
    private void TryDeactivate()
    {
        if (!Plug.isAttached || aiLineOfSight.Objs.Count == 0)
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
        if (playerLineOfSight.Objs.Contains(gameObject))
        {
            return true;
        }
        return false;
    }
}
