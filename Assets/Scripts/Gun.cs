using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private float timeSinceLastShot = 0f;
    
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float chargeCost = 1f;
    [SerializeField] private float plugShootForce = 1000f; 
    
    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private GameObject plugEnd;
    private Plug plugEndScript;
    private Rigidbody plugEndRB;

    [SerializeField] private PlayerCharge chargeBar;

    [SerializeField] private Transform wireHoldPoint;

    private bool isWireHeld = false;

    private void OnEnable()
    {
        InputManager.inputActions.Gameplay.ShootGun.performed += ctx => ShootBullet();
        InputManager.inputActions.Gameplay.DetachShootWire.performed += ctx => WireHold();
        InputManager.inputActions.Gameplay.DetachShootWire.canceled += ctx => ShootWire();

    }
    private void OnDisable()
    {
        InputManager.inputActions.Gameplay.ShootGun.performed -= ctx => ShootBullet();
        InputManager.inputActions.Gameplay.DetachShootWire.performed -= ctx => WireHold();
        InputManager.inputActions.Gameplay.DetachShootWire.canceled -= ctx => ShootWire();
    }
    
    void Start()
    {
        plugEndRB = plugEnd.GetComponent<Rigidbody>();
        plugEndScript = plugEnd.GetComponent<Plug>();
    }

    void Update()
    {
        if (timeSinceLastShot <= fireRate)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isWireHeld)
        {
            plugEnd.transform.position = wireHoldPoint.position;
        }    
    }

    void ShootBullet()
    {
        if (timeSinceLastShot < fireRate) return;
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
        // Decrease Charge
        chargeBar.DecreaseCharge(chargeCost);
        timeSinceLastShot = 0f;
    }

    void WireHold()
    {
        // Move wire end to Hold Point OR detach from generator
        if (Plug.isAttached)
        {
            plugEndScript.Detach();
            plugEndRB.isKinematic = false;
            return;
        }   
        Debug.Log("Wire Hold");
        plugEndRB.isKinematic = true;

        plugEnd.transform.position = wireHoldPoint.position;
        isWireHeld = true;
    }

    void ShootWire()
    {
        if (!isWireHeld) return;
        // Forward Force on wire end if in hand
        Debug.Log("Wire Release");
        isWireHeld = false;
        plugEndRB.isKinematic = false;
        plugEndRB.AddForce(transform.forward * plugShootForce);

    }
}
