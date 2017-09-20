using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade : MonoBehaviour {

	public Texture fadeTexture;
	float fadeSpeed = 0.1f;
	int drawDepth = -1000;
 
	public float alpha = 1.0f; 
	public int fadeDir = -1;
 
	void OnGUI(){
		
		alpha += fadeDir * fadeSpeed * Time.deltaTime;  
		alpha = Mathf.Clamp01(alpha);   
		 
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.g, alpha);
		 
		GUI.depth = drawDepth;
		 
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}
}
