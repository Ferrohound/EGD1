using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadFloor : MonoBehaviour {
	
	public string Unload;
	public Elevator elevator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//unload the scene and close the doors
	//probably fade out some lights too
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		SceneManager.UnloadSceneAsync(Unload);
		
		elevator.closeDoors();
	}
}
