using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;              
    public AudioSource m_ShootingAudio;     
    public AudioClip m_FireClip;        

    public List<GameObject> bullets;
        

    private TankMovement tankMovement;

    private void Start()
    {
        tankMovement = gameObject.GetComponent<TankMovement>();
        bullets = new List<GameObject>(Tank.MAX_BULLETS);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && bullets.Count < Tank.MAX_BULLETS)
        {
            // ... launch the shell.
            Fire();
            StartCoroutine(tankMovement.PauseTank());
        }
    }


    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        bullets.Add(shellInstance.gameObject);

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}