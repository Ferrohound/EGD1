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
	public void Interact(PlayerController pc, int flag) {
		switch (flag) { 
			case 0:
				rb.isKinematic = false;
				
				for(int i = 0; i<cols.Count; i++)
				{
					Physics.IgnoreCollision(cols[i], pc.col, false);
				}
				transform.SetParent(null);
			
				held = false; 
			break;
			
			case 1:
				rb.isKinematic = true;
				
				for(int i = 0; i<cols.Count; i++)
				{
					Physics.IgnoreCollision(cols[i], pc.col, true);
				}
				transform.SetParent(pc.LeftHand);
				transform.localPosition = Vector3.zero;
				held = true; 
			break;
			
			case 2:
				rb.isKinematic = true;
				
				for(int i = 0; i<cols.Count; i++)
				{
					//Physics.IgnoreCollision(cols[i], pc.m_Capsule, true);
				}
				transform.SetParent(pc.RightHand);
				transform.localPosition = Vector3.zero;
				held = true; 
				
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
		if ( other == null || !other.held )
			return;
		
		if(transform.parent == null)
		{
			transform.SetParent(other.transform);
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
				| RigidbodyConstraints.FreezePositionZ;
			held = false;
		}
		
		//probably stop it from being grabbable? idk
		
		target = null;
		
	}
}
