using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class TankShooting : MonoBehaviour
{       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;              
    public AudioSource m_ShootingAudio;     
    public AudioClip m_FireClip;        

    public List<GameObject> bullets;
        

    private TankMovement tankMovement;
    private bool bulletHandling;

    private void Start()
    {
        tankMovement = gameObject.GetComponent<TankMovement>();
        bullets = new List<GameObject>(Tank.MAX_BULLETS);
        bulletHandling = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && bullets.Count < Tank.MAX_BULLETS && !bulletHandling)
        {
            // ... launch the shell.
            Fire();
            bulletHandling = true;
            StartCoroutine(tankMovement.PauseTank());
            bulletHandling = false;
            //Debug.Break();
        } 
    }


    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        bullets.Add(shellInstance.gameObject);
        //TODO::try to get rid of this getcomponent too taxing
        shellInstance.GetComponent<Bullet>().owner = gameObject;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}