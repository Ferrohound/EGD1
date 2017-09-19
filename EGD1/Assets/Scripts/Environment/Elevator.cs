using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour {
	
	//which floor to load and which to unload (current)
	[Tooltip("Floor to load")]
	public string LoadFloor;
	public Popup popup;
	
	//where the elevator is actually moving to
	public Vector3 target;
	public Vector3 start;
	
	public float moveSpeed;
	public float rideTime;
	
	public Door[] frontDoors;
	public Door[] backDoors;
	
	//take ~5 seconds to complete the scene change
	//open doors and display popup

	// Use this for initialization
	void Start () {
		start = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//the trigger for the elevator is ~halfway in
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		//when the player enters, shut the doors, asynchronously load the next level
		//then unload the current one
		
		SceneManager.LoadSceneAsync(LoadFloor, LoadSceneMode.Additive);
		
		//elevator starts to move
		closeDoors();
		//SceneManager.UnloadSceneAsync(CurrentFloor);
	}
	
	//open the door and flash popup
	void openDoors()
	{
		backDoors[0].move = true;
		backDoors[1].move = true;
		execute(LoadFloor);
	}
	
	void closeDoors()
	{
		frontDoors[0].move = true;
		frontDoors[1].move = true;
		
		StartCoroutine(Move());
	}
	
	public IEnumerator Move()
	{
		float currentTime = 0;
		while(currentTime < rideTime)
		{
			
			transform.position = Vector3.Lerp(start, target, currentTime/rideTime);
			currentTime+=Time.deltaTime;
			yield return null;
		}
		
		execute(LoadFloor);
		//open the door back up
	}
	
	void execute(string text)
	{
		popup.setText(text);
		popup.ShowPopup();
	}
}
