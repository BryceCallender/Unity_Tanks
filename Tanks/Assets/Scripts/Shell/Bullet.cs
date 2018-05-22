using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public enum BulletSpeed
{
    NORMAL,
    HIGH
};

public class Bullet : MonoBehaviour 
{ 
    public float speed;
    public int ricochetCount;
    public LayerMask layerMask;
    public GameObject owner;

    private RaycastHit hit;
    private Vector3 reflectedVector;
    private Ray ray;
    private float rotation;
    
    private Tank tank;
    private TankShooting tankShoot;

    private EnemyStats stats;
    private AITankShooting aiTankShoot;
    
    private float multiplier;

    // Use this for initialization
    void Start () 
    {
        //This assumes the bullet only comes from a tank
        rotation = transform.rotation.eulerAngles.y;
        if (owner.gameObject.name.Contains("AI"))
        {
            stats = owner.GetComponent<EnemyStats>();
            aiTankShoot = owner.GetComponent<AITankShooting>();
        }
        else
        {
            tank = owner.GetComponent<Tank>();
            tankShoot = owner.GetComponent<TankShooting>();
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        RayCast();
	}

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Adds the speed forwards and increases speed based on multiplier 
        //from the speed enum relative to its local transform  
        if (tank != null)
        {
            multiplier = GetMultiplier(tank.bulletSpeed);
        }
        else
        {
            multiplier = GetMultiplier(stats.bulletSpeed);
        }
        
        transform.Translate(Time.deltaTime * Vector3.forward * speed * multiplier ,Space.Self);
        //Keeps the rotation the same 
        transform.eulerAngles = new Vector3(0, rotation, 0);
    }

    private float GetMultiplier(BulletSpeed bulletSpd)
    {
        float multiplier;
        //If normal mult its 1x, if its high its gonna be 1.5x speed 
        switch(bulletSpd)
        {
            case BulletSpeed.NORMAL: multiplier = 1;
                break;
            case BulletSpeed.HIGH: multiplier = 1.5f;
                break;
            default: multiplier = 0;
                break;
        }
        return multiplier;
    }

    private void RayCast()
    {
        ray = new Ray(transform.position, transform.forward);
        //Raycast forward to keep track of how we are doing
        if (Physics.Raycast(ray, out hit,speed,layerMask))
        {
            reflectedVector = Vector3.Reflect(transform.forward,hit.normal);
        } 
    }


    public void SetRicochetCount(int ricoCount)
    {
        ricochetCount = ricoCount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If we hit a wall lets reflect the bullet along the normal of the 
        //wall based on how it hits
        if(collision.gameObject.CompareTag("Wall"))
        {
            //If we can bounce off walls anymore
            if(ricochetCount > 0)
            {
                rotation = 90 - Mathf.Atan2(reflectedVector.z, reflectedVector.x) * Mathf.Rad2Deg;
                //Weird corner stuff so im just gonna make it explode
                if (Math.Abs(rotation - 90) < 0.1)
                {
                    RemoveBullet();
                    Explode();
                   
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, rotation, 0);
                    ricochetCount--;
                }
               
            }
            else
            {
                //It cant ricochet anymore so destroy it
                RemoveBullet();
                Explode();

            }
        }
        else if(collision.gameObject.layer != 12)
        {
            //Hit something other than wall, kill it and the object it hit
            RemoveBullet();
            Destroy(collision.gameObject);
            Explode();

        }
    }

    private void Explode()
    {
        var hbPs = GetComponent<HyperbitProjectileScript>();
        
        hbPs.Explode();
        
        Destroy(gameObject);
    }

    private void RemoveBullet()
    {
        if (tankShoot != null)
        {
            tankShoot.bullets.Remove(gameObject);
        }
        else
        {
            aiTankShoot.bullets.Remove(gameObject);
        }
    }

    public void SetKillStatusForTank()
    {
        
    }
    
}
