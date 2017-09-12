using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class GameItem : MonoBehaviour, Interactable {
	
	Rigidbody rb;
	bool held = false;
	bool initialized = false;
    private List<Collider> cols;

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
		}
	}
}
