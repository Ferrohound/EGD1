using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	
	public Transform target;
	public Transform openTarget;
	public float closeSpeed;
	public bool move;
	public bool open = false;
	public bool close = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target!=null && open)
		{
			Open();
		}
		else if (openTarget!=null && close)
		{
			Close();
		}
	}
	
	void Close()
	{
		float step = closeSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		if(Vector3.Distance(transform.position, target.position) < 0.1)
			close = false;
	}
	
	void Open()
	{
		float step = closeSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, openTarget.position, step);
		if(Vector3.Distance(transform.position, openTarget.position)<0.1)
			open = false;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag!="Door")
			return;
		
		close = false;
	}
}
