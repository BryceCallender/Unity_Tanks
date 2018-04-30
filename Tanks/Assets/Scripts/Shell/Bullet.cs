using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletSpeed
{
    NORMAL,
    HIGH
};

public class Bullet : MonoBehaviour 
{
    public int bulletCount;
    public int ricochetCount;
    public float speed;
    public BulletSpeed bulletSpeed;


    private RaycastHit hit;
    private Vector3 reflectedVector;
    private Ray ray;
    private Rigidbody rb;

	// Use this for initialization
	void Start () 
    {
        ray = new Ray(transform.position, transform.forward);
        rb = gameObject.GetComponent<Rigidbody>();
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
        //from the speed enum
        rb.AddForce(transform.forward * speed * GetMultiplier(bulletSpeed));
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
        ray.direction = transform.forward;
        Debug.DrawRay(transform.position,ray.direction,Color.blue,Mathf.Infinity);
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
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 hitNorm = hit.normal;
                    Vector3 incomingVector = transform.position - hit.transform.position;
                    reflectedVector = Vector3.Reflect(incomingVector, hitNorm);
                    transform.rotation = Quaternion.LookRotation(reflectedVector);
                    ricochetCount--;
                } 
            }
            else
            {
                //It cant ricochet anymore so destroy it
                Destroy(gameObject);
            }
        }
        else
        {
            //Hit something other than wall, kill it and the object it hit
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
