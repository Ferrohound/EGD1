using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	
	public Transform target;
	public float closeSpeed;
	public bool move;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target!=null && move)
		{
			float step = closeSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Door")
			return;
		
		move = false;
	}
}
