using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MeshRenderer>().enabled)
        {

            Debug.Log("Beat Start");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Beat Stop");
    }
}
