using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;              
    public AudioSource m_ShootingAudio;     
    public AudioClip m_FireClip;         

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
    

    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
    }


    private void Update()
    {
        if (Input.GetButtonDown(m_FireButton) && !m_Fired)
        {
            // ... launch the shell.
            Fire();
            StartCoroutine(gameObject.GetComponent<TankMovement>().PauseTank());
            m_Fired = false;
        }
    }


    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}