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
    public Intelligence tankIntelligence;
    public int mineCount;

    public ParticleSystem explosion;

    //Can only have 2 mins active at a time
    public List<GameObject> mines;

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


            //Make sure only 2 active

            //
        }
    }

    private void OnDestroy()
    {
        //Fix explosion
        explosion.Play();
    }
}
