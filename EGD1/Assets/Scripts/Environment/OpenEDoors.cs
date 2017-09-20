using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEDoors : MonoBehaviour {
	
	public Elevator e;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		Debug.Log("Open the doors!");
		
		e.openFrontDoors();
		Destroy(this);
		
	}
}
