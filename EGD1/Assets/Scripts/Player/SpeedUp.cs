using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {
	
	public Transform player;
	public Transform target;
	public float moveSpeed = 6;
	bool speed = false;
	float step;

	// Update is called once per frame
	void Update () {
		if(speed)
		{
			step = moveSpeed * Time.deltaTime;
			player.position = Vector3.MoveTowards(player.position, target.position, step);
			if(Vector3.Distance(player.position, target.position)<0.1)
				Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			player = col.transform;
			speed = true;
		}
	}
}
