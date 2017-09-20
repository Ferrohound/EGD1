using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
	
	public Animator anim;
	public Text words;
	public bool playOnStart = false;

	// Use this for initialization
	void Start () {
		if(playOnStart)
		{
			ShowPopup();
		}
	}
	
	// Update is called once per frame
	public void ShowPopup () {
		anim.Play("Popup");
	}
	
	public void setText(string text)
	{
		words.text = text;
	}
}
