using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float radiusOfExplosion;
    public float activationTime;
    public float blinkingTime;
    public GameObject owner;

    //Booleans for mine logic
    private bool hitByBullet;
    private bool isInProximity;
    private bool isBlinking;

    private Material material;
    private Color normalColor;
    private Color redBlinkingColor;

    private RaycastHit hitInfo;

    private float blinkDuration = 2f;

    private LayerMask tankLayerMask;
    private Collider[] colliders;

    // Use this for initialization
    void Start ()
    {
        material = GetComponent<Renderer>().material;
        normalColor = material.color;
        redBlinkingColor = Color.red;
        //owner = gameObject;
        tankLayerMask = 1 << 11;
    }
	
	// Update is called once per frame
	void Update ()
    {
        colliders = Physics.OverlapSphere(transform.position, radiusOfExplosion,tankLayerMask);
        //Check for any tanks of some sort and if one enters it start the activation sequence
        if (colliders.Length > 0)
        {
            if(colliders[0].gameObject.name != owner.name)
            {
                //Its got something in its proximity so lets start blinking
                StartCoroutine(SetMineTimerCountDown(0));
            }
            else
            {
                //Just start the countdown of the mine itself as if nothing is near it 
                StartCoroutine(SetMineTimerCountDown(activationTime));
            }
        }
	} 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            //Blow up immediately
            Detonate();
        }
    }

    private IEnumerator SetMineTimerCountDown(float secsToActivation)
    {
        Debug.Log("Countdown begins");
        yield return new WaitForSeconds(secsToActivation);
        StartCoroutine(MineBlinking());
    }

    private IEnumerator MineBlinking()
    {
        Debug.Log("Blinking begins");
        material.color = redBlinkingColor;
        yield return new WaitForSeconds(blinkDuration);
        material.color = normalColor;
    }

    private void Detonate()
    {
        RaycastHit[] hitInfoArray;
        //do explosion animation and kill anything in radius rooNya
        hitInfoArray = Physics.SphereCastAll(transform.position, radiusOfExplosion, Vector3.forward,Mathf.Infinity,tankLayerMask);

        foreach (var item in hitInfoArray)
        {
            Destroy(item.collider.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radiusOfExplosion);
    }
}
