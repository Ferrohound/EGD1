using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTrigger : MonoBehaviour {
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		col.GetComponent<PlayerMovement>().backwards = 
			!col.GetComponent<PlayerMovement>().backwards;
			
		Destroy(gameObject);
	}
}
