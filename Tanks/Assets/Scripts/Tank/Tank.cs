using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Intelligence
{
    PLAYER,
    LOW,
    NORMAL,
    HIGH
};

public class Tank : MonoBehaviour 
{
    public int mineCount;
    public int bulletCount;
    [Header("Tank Enums")]
    public BulletSpeed bulletSpeed;
    public Intelligence tankIntelligence;
    public TankSpeed tankSpeed;


    //Can only have 2 mins active at a time
    public List<GameObject> mines;


    [SerializeField]
    private GameObject minePrefab;

    public static int MAX_BULLETS = 5;
    public static int MAX_MINES = 2;
    
	// Use this for initialization
	void Start () 
    {
        mines = new List<GameObject>(MAX_MINES);
	}
	
	// Update is called once per frame
	void Update () 
    {
        LayMine();
	}

    private void LayMine()
    {
        if(Input.GetKeyDown("space"))
        {
            //LayMine
            mineCount--;
            //Make mine
            Instantiate(minePrefab, transform.position, Quaternion.identity);

            //Make sure only 2 active
        }
    }

    private void OnDestroy()
    {
        ParticleManager.Instance.playExplosion(gameObject.transform);
    }
}
