using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletSpeed
{
    NORMAL,
    HIGH
};

//TODO: fix smoke thing kinda looks weird the wii tanks one looks well done

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

    // Use this for initialization
    void Start () 
    {
        //This assumes the bullet only comes from a tank
        tankShoot = owner.GetComponent<TankShooting>();
        rotation = transform.rotation.eulerAngles.y;
        tank = owner.GetComponent<Tank>();
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
        transform.Translate(Time.deltaTime * Vector3.forward * speed * GetMultiplier(tank.bulletSpeed),Space.Self);
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
        if(collision.gameObject.tag == "Wall")
        {
            //If we can bounce off walls anymore
            if(ricochetCount > 0)
            {
                rotation = 90 - Mathf.Atan2(reflectedVector.z, reflectedVector.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rotation, 0);
                ricochetCount--;
            }
            else
            {
                //It cant ricochet anymore so destroy it
                tankShoot.bullets.Remove(gameObject);
                Destroy(gameObject);

            }
        }
        else
        {
            //Hit something other than wall, kill it and the object it hit
            tankShoot.bullets.Remove(gameObject);
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
