using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour {
	
	bool riding = false;
	
	//which floor to load and which to unload (current)
	[Tooltip("Floor to load")]
	public string LoadFloor;
	
	public GameObject player;
	public Light light;

	public GameObject popup;
	
	//where the elevator is actually moving to
	public Transform target;
	public Vector3 start;
	public Quaternion sR;
	
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
	
	//the trigger for the elevator is ~halfway in
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Player" || riding)
			return;
		
		//when the player enters, shut the doors, asynchronously load the next level
		//then unload the current one
		player = col.gameObject;
		SceneManager.LoadSceneAsync(LoadFloor, LoadSceneMode.Additive);
		Destroy(light.gameObject);
		
		//elevator starts to move
		StartCoroutine(Move());
		//SceneManager.UnloadSceneAsync(CurrentFloor);
	}
	
	//open the door and flash popup
	public void openFrontDoors()
	{
		Debug.Log("Opening front");
		frontDoors[0].open = true;
		frontDoors[1].open = true;
	}
	
	public void closeFrontDoors()
	{	
		Debug.Log("Closing front doors");
		frontDoors[0].close = true;
		frontDoors[1].close = true;
		//riding = true;
		//StartCoroutine(Move());
	}
	
	public void openBackDoors()
	{
		Debug.Log("Opening back");
		backDoors[0].open = true;
		backDoors[1].open = true;
		//execute(LoadFloor);
	}
	
	public void closeBackDoors()
	{
		backDoors[0].close = true;
		backDoors[1].close = true;
	}
	
	public IEnumerator Move()
	{
		riding = true;
		yield return new WaitForSeconds(2);
		closeFrontDoors();
		float currentTime = 0;
		sR = transform.rotation;
		Quaternion playerLocal = player.transform.localRotation;
		while(currentTime < rideTime)
		{
			
			transform.position = Vector3.Lerp(start, target.position, currentTime/rideTime);
			transform.rotation = Quaternion.Lerp(sR, target.rotation, currentTime/rideTime);
			player.transform.position = transform.position;
			player.transform.localRotation = playerLocal;
			currentTime+=Time.deltaTime;
			yield return null;
		}
		
		start = transform.position;
		
		//because HOLY FUCK.
		Transform totem = GameObject.Find("Totem").transform;
		player.transform.LookAt(totem);
		Destroy(totem.gameObject);
		
		
		//execute(LoadFloor);
		//open the door back up
		//openFrontDoors();
		openBackDoors();
	}
	
	void execute(string text)
	{
	/*	if(popup == null)
			popup = GameObject.Find("Popup");
		popup.setText(text);
		popup.ShowPopup();*/
	}
}
