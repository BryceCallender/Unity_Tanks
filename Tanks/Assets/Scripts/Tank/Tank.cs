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

    
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    private void LayMine()
    {
        if(Input.GetKeyDown("Space"))
        {
            //LayMine
            mineCount--;
        }
    }
}
