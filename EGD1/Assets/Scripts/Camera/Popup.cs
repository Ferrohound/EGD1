using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
	
	public Canvas canvas;
	GameObject popup;

	// Use this for initialization
	void Start () {
		if(canvas.transform.Find("Popup")!=null)
		{
			popup = canvas.transform.Find("Popup").gameObject;
		}
	}
	
	// Update is called once per frame
	public void ShowPopup () {
		Debug.Log(popup);
		popup.GetComponent<Animator>().Play("Popup");
	}
	
	public void setText(string text)
	{
		if (popup == null)
			popup = canvas.transform.Find("Popup").gameObject;
		
		popup.transform.Find("[popupText]").gameObject.GetComponent<Text>().text = text;
	}
}
