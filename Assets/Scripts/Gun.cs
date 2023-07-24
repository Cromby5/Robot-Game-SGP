using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    private float damage = 10f; // if raycasts are used, bullet has the values at the moment
    private float range = 100f; // if raycasts are used

    private float fireRate = 15f;

    [SerializeField] private GameObject bulletPrefab;
    private void OnEnable()
    {
        InputManager.inputActions.Gameplay.ShootGun.performed += ctx => ShootBullet();
        InputManager.inputActions.Gameplay.DetachShootWire.performed += ctx => ShootWire();
    }
    private void OnDisable()
    {
        InputManager.inputActions.Gameplay.ShootGun.performed -= ctx => ShootBullet();
        InputManager.inputActions.Gameplay.DetachShootWire.performed -= ctx => ShootWire();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
    }

    void ShootWire()
    {
        // Direct the wire end to the point of aim
    }
}
