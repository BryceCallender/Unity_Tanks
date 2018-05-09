using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mine Class.
/// This mine can either be a proximity mine
/// or it can just be a timed mine based on 
/// whether or not a tank enters its blast radius.
/// 
/// It lasts as long as activation time and 
/// blinking time is the time itll blink before blowing up
/// </summary>
public class Mine : MonoBehaviour
{
    public float radiusOfExplosion;
    public float activationTime;
    public float blinkingTime;
    public GameObject owner;

    //Booleans for mine logic
    [SerializeField]
    private bool isActivated;
    [SerializeField]
    private bool isBlinking;
    private bool explosiveOverriden;

    private Material material;
    private Color normalColor;
    private Color redBlinkingColor;

    private RaycastHit hitInfo;

    private float blinkDuration = 1f;

    private LayerMask tankLayerMask;
    [SerializeField]
    private Collider[] colliders;
    private Coroutine mineTimer;

    // Use this for initialization
    void Start ()
    {
        material = GetComponent<Renderer>().material;
        normalColor = material.color;
        redBlinkingColor = Color.red;
        owner = gameObject;
        tankLayerMask = 1 << 11;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!isActivated)
        {
            mineTimer = StartCoroutine(SetMineTimerCountDown(activationTime));
        }

        colliders = Physics.OverlapSphere(transform.position, radiusOfExplosion,tankLayerMask);
        //Check for any tanks of some sort and if one enters it start the activation sequence
        if (colliders.Length > 0)
        {
            //Check for unfriendly tanks to the colliders
            //TODO::maybe make enum to keep track of team
            if(HasUnfriendlyTank(colliders))
            {
                if(isActivated && !explosiveOverriden)
                {
                    Debug.Log("Stopped");
                    StopCoroutine(mineTimer);
                    isActivated = false;
                    explosiveOverriden = true;
                    //Its got something in its proximity so lets start blinking
                    if(!isActivated)
                    {
                        StartCoroutine(SetMineTimerCountDown(0));
                    }
                }
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
        isActivated = true;
        yield return new WaitForSeconds(secsToActivation);
        if(!isBlinking)
        {
            StartCoroutine(MineBlinking()); 
        }
    }

    private IEnumerator MineBlinking()
    {
        Debug.Log("Blinking begins");
        float blinkTime = blinkingTime;
        isBlinking = true;
        while(blinkTime > 0)
        {
            material.color = redBlinkingColor;
            yield return new WaitForSeconds(blinkDuration);
            material.color = normalColor;
            blinkTime -= blinkDuration;
        }
        Detonate();
    }

    private void Detonate()
    {
        Collider[] hitInfoArray;
        //do explosion animation and kill anything in radius rooNya
        hitInfoArray = Physics.OverlapSphere(transform.position, radiusOfExplosion, tankLayerMask);

        foreach (var item in hitInfoArray)
        {
            Destroy(item.gameObject);
        }

        Destroy(gameObject);

        isActivated = false;
        isBlinking = false;
    }

    public bool HasUnfriendlyTank(Collider[] colliders)
    {
        foreach (var item in colliders)
        {
            if(item.gameObject.name != owner.name)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radiusOfExplosion);
    }
}
