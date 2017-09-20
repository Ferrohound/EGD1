using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadFloor : MonoBehaviour {
	
	public string Unload;
	public GameObject E;
	public Elevator elevator;
	public Transform exit;
	
	public Popup popup;
	public string floorName;

	// Use this for initialization
	void Start () {
		E = elevator.gameObject;
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
		E.SetActive(true);
		elevator.closeBackDoors();
		col.transform.GetComponent<PlayerMovement>().target = exit;
		
		popup.setText(floorName);
		popup.ShowPopup();
	}
}
