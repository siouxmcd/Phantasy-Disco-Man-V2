using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgSkills : MonoBehaviour {

    private static bool created = false;

    public int improv;
    public int fluidity;
    public int balance;

    public float basicMoveValue = 1;
    public float finishMoveValue = 4;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
