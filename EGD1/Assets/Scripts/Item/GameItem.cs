using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class GameItem : MonoBehaviour, Interactable {
	
	Rigidbody rb;
	public bool held = false;
	bool initialized = false;
    private List<Collider> cols;
	public Transform target = null;
	public float fusionSpeed = 3f;
	
	GameItem dad;

	// Use this for initialization
	void Start () {
		if(!initialized)
			Initialize();
	}
	
	void Initialize()
	{
		rb = GetComponent<Rigidbody>();
		cols = new List<Collider>(GetComponents<Collider>());
		initialized = true;
	}
	
	void Update()
	{
		if(target!=null)
		{
			float step = fusionSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		}
	}
	
	// Interact
	//Flag: 0 - drop item
	//		1 - pickup left
	//		2 - pickup right
	//		3 - combine
	//		4 - separate
	
	//worry about moving all of its children to its parent later
	public void Interact(PlayerController pc, int flag) {
		switch (flag) { 
			case 0:
				rb.isKinematic = false;
				
				for(int i = 0; i<cols.Count; i++)
				{
					Physics.IgnoreCollision(cols[i], pc.col, false);
				}
				transform.SetParent(null);
				
				//set all of its children's held to false as well
				int children = transform.childCount;
				for (int i = 0; i < children; ++i)
					transform.GetChild(i).GetComponent<GameItem>().held = false;
				held = false; 
			break;
			
			case 1:
				if(dad != null)
					dad.Interact(pc, flag);
					
					else 
					{
					rb.isKinematic = true;
					
					for(int i = 0; i<cols.Count; i++)
					{
						Physics.IgnoreCollision(cols[i], pc.col, true);
					}
					
					Debug.Log("This transform's parent is...-!");
					Debug.Log(transform.parent);
					
					//hold the parent object
					if(transform.parent == null)
					{
						transform.SetParent(pc.LeftHand);
						transform.localPosition = Vector3.zero;
						held = true; 
						pc.left = transform;
					}
					else
					{
						Transform t = transform;
						if(t.parent == pc.RightHand || t.parent == pc.LeftHand)
							return;
						//set all of the held items' held to true
						held = true;
						while(t.parent!=null)
						{
							t = t.parent;
							t.GetComponent<GameItem>().held = true;
						}
						t.GetComponent<GameItem>().rb.isKinematic = true;
						t.SetParent(pc.LeftHand);
						t.localPosition = Vector3.zero;
						t.GetComponent<GameItem>().held = true;
						pc.left = t;
					}
				}
			break;
			
			case 2:
				if(dad != null)
					dad.Interact(pc, flag);
				
				else
				{
					rb.isKinematic = true;
					
					for(int i = 0; i<cols.Count; i++)
					{
						Physics.IgnoreCollision(cols[i], pc.col, true);
					}
					
					//hold the parent object
					
					Debug.Log("This transform's parent is...-!");
					Debug.Log(transform.parent);
					
					if(transform.parent == null)
					{
						transform.SetParent(pc.RightHand);
						transform.localPosition = Vector3.zero;
						held = true; 
						pc.right = transform;
					}
					else
					{
						Transform t = transform;
						while(t.parent!=null)
						{
							t = t.parent;
							t.GetComponent<GameItem>().held = true;
						}
						
						t.GetComponent<GameItem>().rb.isKinematic = true;
						t.SetParent(pc.RightHand);
						t.localPosition = Vector3.zero;
						t.GetComponent<GameItem>().held = true;
						pc.right = t;
					}
				}
				
			break;
			
			//combining
			case 3:
				if(pc.left == transform)
				{
					target = pc.right;
				}
				else
				{
					target = pc.left;
				}
				transform.SetParent(null);
			break;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		//if they're both held, set one as the parent (if its parent is null i guess)
		//and negate the child's rigidbody
		GameItem other = col.GetComponent<GameItem>();
		if ( other == null || !other.held || /*Otherwise things just stick*/!held)
			return;
		
		if(transform.parent == null)
		{
			for(int i = 0; i<cols.Count; i++)
			{
				for(int j = 0; j<other.cols.Count; j++)
				{
					Physics.IgnoreCollision(cols[i], other.cols[j], true);
				}
			}
			
			transform.SetParent(other.transform);
			dad = other;
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
				| RigidbodyConstraints.FreezePositionZ;
		}
		
		//probably stop it from being grabbable? idk
		
		target = null;
		
	}
}
