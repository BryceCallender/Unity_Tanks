using System.Collections;
using UnityEngine;

public enum TankSpeed
{
    IMMOBILE,
    NORMAL,
    HIGH
};

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 6f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public Transform m_TurretTransform;
    public float m_PitchRange = 0.2f;

    public TankSpeed tankSpeed;
    private TankSpeed permTankSpeed;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;

    private float secondsToWait = 0.25f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        permTankSpeed = tankSpeed;
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }

    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // If Tank is not moving.
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // If Tank is moving.
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        MoveBody();
        MoveTurret();
        Turn();
    }

    private void MoveBody()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        movement *= GetMultiplier(tankSpeed);

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void MoveTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Vector3 currentMousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            currentMousePosition = hitInfo.point;
        }

        Vector3 direction = currentMousePosition - m_TurretTransform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        m_TurretTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

    }

    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        turn *= GetMultiplier(tankSpeed);

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    private float GetMultiplier(TankSpeed speed)
    {
        float multiplier;
        //If normal mult its 1x, if its high its gonna be 1.5x speed 
        switch (speed)
        {
            case TankSpeed.IMMOBILE:multiplier = 0;
                break;
            case TankSpeed.NORMAL:multiplier = 0.5f;
                break;
            case TankSpeed.HIGH: multiplier = 1f;
                break;
            default: multiplier = 0;
                break;
        }
        return multiplier;
    }

    public IEnumerator PauseTank()
    {
        TankSpeed newTankSpeed = TankSpeed.IMMOBILE;
        //Sets it immobile for the seconds to wait
        tankSpeed = newTankSpeed;
        yield return new WaitForSeconds(secondsToWait);
        //sets it back to its old permanent speed
        tankSpeed = permTankSpeed;
    }
}