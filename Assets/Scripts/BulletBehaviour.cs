using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    public float bulletSpeed;
    public float secondsUntilDestory;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody RigidBullet = GetComponent<Rigidbody>();
        RigidBullet.velocity = transform.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        secondsUntilDestory -= Time.deltaTime;

        if (secondsUntilDestory < 1)
        {
            transform.localScale *= secondsUntilDestory;
        }

        if (secondsUntilDestory <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
