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
	
	//button movement
	float forwardMov;
	float sideMov;
	
	
	float movSpeed = 4;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		forwardMov = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;
		sideMov = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
		
		//Debug.Log("forwardMove " + forwardMov);
		//Debug.Log("sideMove " + sideMov);
		
		//transform.position += (transform.forward * forwardMov * movSpeed)
			//+ (transform.right * sideMov * movSpeed);
		
		//maybe apply acceleration later
		transform.position += transform.forward*forwardMov+transform.right*sideMov;
		//rb.AddForce(((transform.forward * forwardMov) + 
			//(transform.right * sideMov)));
		
	}
}
