using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem tankExplosion;


    private ParticleSystem ps;

    private static ParticleManager instance;

    public static ParticleManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        tankExplosion = tankExplosion.GetComponent<ParticleSystem>();
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(ps != null)
        {
            Destroy(ps, ps.main.duration);
        }
       
    }

    public void playExplosion(Transform tank)
    {
        ps = Instantiate(tankExplosion, tank.position,tank.transform.rotation) as ParticleSystem;
        ps.Play();
    }
}
