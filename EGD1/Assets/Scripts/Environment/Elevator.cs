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
	
	public Transform center, front, back;
	
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
		if(light!=null)
			Destroy(light.gameObject);
		player.GetComponent<PlayerMovement>().target = null;
		//elevator starts to move
		StartCoroutine(Move());
		//SceneManager.UnloadSceneAsync(CurrentFloor);
	}
	
	//open the door and flash popup
	public void openFrontDoors()
	{
		frontDoors[0].open = true;
		frontDoors[1].open = true;
	}
	
	public void closeFrontDoors()
	{	
		frontDoors[0].close = true;
		frontDoors[1].close = true;
		//riding = true;
		//StartCoroutine(Move());
	}
	
	public void openBackDoors()
	{
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
		Vector3 lookPos;
		Quaternion rotation;
		while(currentTime < rideTime)
		{
			
			transform.position = Vector3.Lerp(start, target.position, currentTime/rideTime);
			transform.rotation = Quaternion.Lerp(sR, target.rotation, currentTime/rideTime);
			if(center!=null)
				player.transform.position = center.position;
			if(front!=null)
			{
				lookPos = front.position - player.transform.position;
				lookPos.y = 0;
				rotation = Quaternion.LookRotation(lookPos);
				//player.transform.LookAt(front);
				player.transform.rotation = rotation;
			}
			currentTime+=Time.deltaTime;
			yield return null;
		}
		
		start = transform.position;
		
		//because HOLY FUCK.
		Transform totem = GameObject.Find("Totem").transform;
		lookPos = totem.position - player.transform.position;
		lookPos.y = 0;
		rotation = Quaternion.LookRotation(lookPos);
		//player.transform.LookAt(front);
		player.transform.rotation = rotation;
		//player.transform.LookAt(totem);
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
