using System;
using UnityEngine;

[Serializable]
public class TankManager
{         
    public Transform m_SpawnPoint;                     
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;
    [HideInInspector] public bool IsAi;


    //Tank properties
    private TankMovement m_Movement;       
    private TankShooting m_Shooting;

    //Ai properties
    private AITankAiming m_AiTankAiming;
    private AITankMovement m_AiTankMoving;
    private AITankRayCasting m_AiTankRayCasting;
    private AITankShooting m_AiTankShooting;

    public void Setup()
    {
        if (!IsAi)
        {
            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();
        }
        else
        {
            m_AiTankAiming = m_Instance.GetComponent<AITankAiming>();
            m_AiTankMoving = m_Instance.GetComponent<AITankMovement>();
            m_AiTankRayCasting = m_Instance.GetComponent<AITankRayCasting>();
            m_AiTankShooting = m_Instance.GetComponent<AITankShooting>();
        } 
    }


    public void DisableControl()
    {
        if (!IsAi)
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;
        }
        else
        {
            m_AiTankAiming.enabled = false;
            m_AiTankMoving.enabled = false;
            m_AiTankRayCasting.enabled = false;
            m_AiTankShooting.enabled = false;
        }
    }


    public void EnableControl()
    {
        if (!IsAi)
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;
        }
        else
        {
            m_AiTankAiming.enabled = true;
            m_AiTankMoving.enabled = true;
            m_AiTankRayCasting.enabled = true;
            m_AiTankShooting.enabled = true;
        }
    }

    public void DisableScript()
    {
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
