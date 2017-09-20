using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Podium : MonoBehaviour, Interactable {
	
	public Transform glory;
	public string currentLevel;
	public string level;
	public Transform resetLocation;
	
	//combine both items and place on the podium, pan around and fade to white
	//set position to start, set facing and load scene
	//set backwards to true
	public void Interact(PlayerController pc, int flag) {
		switch (flag)
		{
			case 0:
				Transform t1 = pc.left;
				Transform t2 = pc.right;
				
				if(t1 == null && t2 == null)
				{
					//fade to black
					StartCoroutine(softReset(pc));
				}
				
				else if (t1 == null && t2 != null)
				{
					pc.Place(1, glory);
					StartCoroutine(softReset(pc));
				}
				
				else if (t2 == null && t1 != null)
				{
					pc.Place(2, glory);
					StartCoroutine(softReset(pc));
				}
				
				//combine both
				else
				{
					pc.Place(1, glory);
					pc.Place(2, glory);
					StartCoroutine(softReset(pc));
				}
			
			break;
		}
	}
	
	//set location
	IEnumerator softReset(PlayerController pc)
	{
		float curr = 0;
		pc.f.fadeDir = 1;
		Camera cam = Camera.main;
		
		while(curr < 2)
		{
			float step = (2 * Time.deltaTime);
			cam.transform.LookAt(glory);
			cam.transform.position = Vector3.MoveTowards(transform.position, glory.position,
				step);
			
			cam.transform.position += (transform.right * step);
			//move camera in circle around podium
			curr += Time.deltaTime;
			yield return null;
		}
		//yield return new WaitForSeconds(2);
		//camera pan here
		
		
		SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
		//will this actually work-
		Destroy(pc.gameObject);
		//pc.transform.position = resetLocation.position;
		//pc.transform.rotation = resetLocation.rotation;
		//pc.GetComponent<PlayerMovement>().backwards = true;
		SceneManager.UnloadSceneAsync(currentLevel);
	}
}
