using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]

public class PlayerController : MonoBehaviour {
	
	//transforms for left and right hands
	public Transform leftT, rightT, left, right;
	
	//bool if both hands are in use
	public bool bothHands; 
	
	//camera transform
	public Transform cam;
	
    private int callFlag = -1;           //so update can call functions in fixedupdate
	float carryDistance = .75f;
	Rigidbody rb;
	
	//variables for holding items
	RaycastHit rhit;
	bool fDown;
    bool f2Down;
	
	public Collider col;
	
	
	public Vector3 Velocity{
        get { return rb.velocity; }
    }
	
	public Transform RightHand
	{
		get { return rightT;}
	}
	
	public Transform LeftHand
	{
		get { return leftT; }
	}
	
	
	// Use this for initialization
	void Start () {
		if (cam == null)
			cam = Camera.main.transform;
		rb = GetComponent<Rigidbody>();
		
		foreach(Collider c in GetComponents<Collider>())
		{
            if (!c.isTrigger) 
			{
                col = c; break; //make sure to get the right capsule collider
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
		//update the y value of the hand transform's positions=======================
		Vector3 f = cam.forward;
		Vector3 up = cam.up;
		Vector3 RD = Vector3.Cross(up, f);
		Vector3 LD = Vector3.Cross(f, up);
		
		leftT.position = (cam.position/* - new Vector3(-3, 0, 0)*/) 
			+ cam.transform.forward * carryDistance + LD * 0.5f;
		rightT.position = (cam.position/* + new Vector3(3, 0, 0)*/) 
			+ cam.transform.forward * carryDistance + RD * 0.5f;
		//Debug.Log(cam.forward);
		
		//picking up items ==========================================================
		fDown = Input.GetButtonDown("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
		
		if (left != null && fDown) 
		{//Left throw is being charged
			callFlag = 0;
        } 
		if(right != null && f2Down) 
		{ //WILL HAVE TO CHANGE
			callFlag = 1;
		}
		
		//figure out whether there is an interactable in front of you
		if(cam!=null && Physics.Raycast(cam.position, cam.forward, out rhit, 3.0f))
		{
			if (rhit.transform.GetComponent<GameItem>() != null)
			{
				if (fDown && left == null)
				{//pick up item
						//turn off left holding UI element and on leftThrow
					if(right!=null)
						bothHands = true;
						
					left = rhit.transform;
					left.GetComponent<GameItem>().Interact(this, 1);//ignore collisions
                } 
				else if (f2Down && right == null)
				{//pick up item
							//turn off right holding UI and on rightThrow
					if(left != null)
						bothHands = true;
					right = rhit.transform;
					right.GetComponent<GameItem>().Interact(this, 2); //ignore collisions
                } 
				else 
				{//update ui
                        //like maybe highlight the item or have stuff pop up
						//if left is null
					if(left == null)
					{
							//HUD.LeftPickUp.SetActive(true);
						bothHands = false;
					}
					if(right == null)
					{
							//HUD.RightPickUp.SetActive(true);
						bothHands = false;
					}
                }
            } 
			else if ((fDown || f2Down) && rhit.transform.GetComponent<Interactable>() != null) 
			{
				rhit.transform.GetComponent<Interactable>().Interact(this, 0); //default behaviour
			}
			else
			{
				Debug.Log("Nothing there.");
			}
		}
	}
	
	void FixedUpdate()
	{
		//callFlag = 0 for throw left, 1 for right
		switch (callFlag) { 
            case 0:
                Throw(true);
                callFlag = -1;
                break;
            case 1:
                Throw(false);
                callFlag = -1;
                break;
            default: break;
        }
		
		if (left != null) 
		{
            left.localPosition = Vector3.zero;
        }
        if (right != null) 
		{
            right.localPosition = Vector3.zero;
        }
	}
	
	//throw an item along camera direction
	//true for left, false for right
	void Throw (bool side) {
		//1.5 for now since i don't think we'll need to charge the throw for this one
        if (side) 
		{ 
            left.GetComponent<Rigidbody>().velocity = cam.forward.normalized * 1.5f + Velocity;
            left.GetComponent<GameItem>().Interact(this, 0);
            left = null;
        } 
		else 
		{
            right.GetComponent<Rigidbody>().velocity = cam.forward.normalized * 1.5f + Velocity;
            right.GetComponent<GameItem>().Interact(this, 0);
            right = null;
        }
    }
	
	
	//coroutine to drop items
	private IEnumerator Drop(int item) {
		yield return new WaitForSeconds(0.3f);
		if (item == 0) {
			left.GetComponent<GameItem>().Interact (this, 0);
            //CheckItemTrigger(left.gameObject);
			left = null;
		} else {
			right.GetComponent<GameItem>().Interact (this, 0);
            //CheckItemTrigger(right.gameObject);
			right = null;
		}
		yield return null;

	}
}
