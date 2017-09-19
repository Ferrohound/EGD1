using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour {
	
	//which floor to load and which to unload (current)
	public string LoadFloor;
	public string CurrentFloor;

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
		
		//when the player enters, shut the doors, asynchronously load the next level
		//then unload the current one
		//open the doors back up
		
		SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
	}
}
