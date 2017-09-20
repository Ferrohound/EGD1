using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]

public class PlayerController : MonoBehaviour {
	
	public fade f;
	
	//persistant player
	public static PlayerController Instance;
	
	//transforms for left and right hands
	public Transform leftT, rightT, left, right;
	
	public float turnSpeed = 200f;
	
	//bool if both hands are in use
	public bool bothHands = false;
	public bool combining = false;
	
	//camera transform
	public Transform cam;
	
    private int callFlag = -1;           //so update can call functions in fixedupdate
	float carryDistance = .75f;
	Rigidbody rb;
	
	//variables for holding items
	RaycastHit rhit;
	bool fDown, f2Down, fUp, f2Up, action;
	bool negateLeft = false;
	bool negateRight = false;
	
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
	
	//persistant player, don't destroy this object on load
	void Awake(){
		if(Instance == null){
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if(Instance!=this) {
			Destroy(gameObject);
		}
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
		
		if(combining && ( left.parent != leftT || right.parent != rightT))
		{
			if(left.parent!=leftT)
			{
				negateLeft = !negateLeft;
				left = null;
			}
			else
			{
				negateRight = !negateRight;
				right = null;
			}
			//Debug.Log("Not combining right now!");
			combining = false;
		}
		
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
		fUp = Input.GetButtonUp("Fire1");
		f2Up = Input.GetButtonUp("Fire2");
		action = Input.GetKeyDown("space");
		
		float turn = turnSpeed * Input.GetAxis("Mouse ScrollWheel"); //* Time.deltaTime;
		turn *= turnSpeed;
		//Debug.Log(turn);
		
		if(left!=null)
		{
			left.Rotate(Vector3.up * turn);
		}
		
		
		if (left != null && fDown && negateLeft) 
		{//Left throw is being charged
			callFlag = 0;
        } 
		
		if(right != null && f2Down && negateRight) 
		{ //WILL HAVE TO CHANGE
			callFlag = 1;
		}
		
		//combine
		if(right!=null && left!=null && action)
		{
			combining = true;
			
			right.GetComponent<GameItem>().Interact(this, 3);
			left.GetComponent<GameItem>().Interact(this, 3);
		}
		
		//figure out whether there is an interactable in front of you
		if(cam!=null && Physics.Raycast(cam.position, cam.forward, out rhit, 3.0f))
		{
			if (rhit.transform.GetComponent<GameItem>() != null && 
				!rhit.transform.GetComponent<GameItem>().held)
			{
				if (!negateLeft && fDown && left == null && rhit.transform != right)
				{//pick up item
						//turn off left holding UI element and on leftThrow
						negateLeft = !negateLeft;
					if(right!=null)
						bothHands = true;
						
					left = rhit.transform;
					left.GetComponent<GameItem>().Interact(this, 1);//ignore collisions
                } 
				else if (!negateRight && f2Down && right == null && rhit.transform!= left)
				{//pick up item
							//turn off right holding UI and on rightThrow
					negateRight = !negateRight;
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
			else if ((fDown || f2Down) 
				&& rhit.transform.GetComponent<GameItem>() == null
				&& rhit.transform.GetComponent<Interactable>() != null) 
			{
				rhit.transform.GetComponent<Interactable>().Interact(this, 0); //default behaviour
			}
			else
			{
				//Debug.Log("Nothing there.");
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
				negateLeft = !negateLeft;
                break;
            case 1:
                Throw(false);
                callFlag = -1;
				negateRight = !negateRight;
                break;
            default: break;
        }
		
		if (left != null && !combining) 
		{
			if(left.transform.parent == null)
			{
				left.transform.SetParent(leftT);
				left.localPosition = Vector3.zero;
			}
        }
        if (right != null && !combining) 
		{
			if(right.transform.parent == null)
			{
				right.transform.SetParent(rightT);
				right.localPosition = Vector3.zero;
			}
        }
	}
	
	//throw an item along camera direction
	//true for left, false for right
	void Throw (bool side) {
		//1.5 for now since i don't think we'll need to charge the throw for this one
        if (side) 
		{ 
			if(left == null)
				return;
            left.GetComponent<Rigidbody>().velocity = cam.forward.normalized * 1.5f + Velocity;
            left.GetComponent<GameItem>().Interact(this, 0);
            left = null;
        } 
		else 
		{
			if(right == null)
				return;
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
	
	//0 - place left
	//1 - place right
	public void Place(int f, Transform t)
	{	
		Transform tmp;
		if(f == 0)
		{
			tmp = left;
			left.GetComponent<Rigidbody>().velocity = cam.forward.normalized * 1.5f + Velocity;
            left.GetComponent<GameItem>().Interact(this, 0);
            left = null;
			callFlag = 0;
		}
		else
		{
			tmp = right;
			right.GetComponent<Rigidbody>().velocity = cam.forward.normalized * 1.5f + Velocity;
            right.GetComponent<GameItem>().Interact(this, 0);
            right = null;
			callFlag = 1;
		}
		
		tmp.SetParent(t);
		tmp.localPosition = Vector3.zero;
		
	}
}
