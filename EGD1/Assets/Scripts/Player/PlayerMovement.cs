using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]

public class PlayerMovement : MonoBehaviour {
	
	//actually implement use of acceleration later
	
	[Serializable]
    public class MovementSettings{
        public float maxSpeed = 4f;             //desired walking speed
        public float freeAcceleration = 4f;     //desired acceleration while not on the ground
        //constant that controls how quickly the controller goes to the desired speed
		public float restoringForce = 14f;   
    }
	
	//boolean for moving backwards
	public bool backwards = false;
	public Transform target;
	
	//button movement
	float forwardMov;
	float sideMov;
	
	
	public float movSpeed = 1;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		forwardMov = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;
		//prevent player from moving forwards
		if(backwards && forwardMov>0)
		{
			forwardMov = 0;
		}
		sideMov = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
		
		float step = (movSpeed/2) * Time.deltaTime;
		
		//Debug.Log("forwardMove " + forwardMov);
		//Debug.Log("sideMove " + sideMov);
		
		//transform.position += (transform.forward * forwardMov * movSpeed)
			//+ (transform.right * sideMov * movSpeed);
		
		//maybe apply acceleration later
		transform.position += transform.forward*forwardMov+transform.right*sideMov;
		//rb.AddForce(((transform.forward * forwardMov) + 
			//(transform.right * sideMov)));
		
		//we're constantly moving backwards through time
		if(backwards)
		{
			//transform.position -= (transform.forward * (movSpeed/4) * Time.deltaTime);
			if(target == null)
				transform.position -= (transform.forward * (movSpeed/4) * Time.deltaTime);
			else
				transform.position = Vector3.MoveTowards(transform.position, target.position,
				step);
		}
		
	}
}
