using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumGlow : MonoBehaviour {

public Color GlowColor;
	public float LerpFactor = 10;
	
	Transform player;
	PlayerController pc;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;

	void Start()
	{
		player = GameObject.Find("Player").transform;
		pc = player.gameObject.GetComponent<PlayerController>();
		
		Renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in Renderers)
		{	
			_materials.AddRange(renderer.materials);
		}
	}

	private void OnMouseEnter()
	{
		if(Vector3.Distance(transform.position, Camera.main.transform.position)<3.5f)
		{
			_targetColor = GlowColor;
			enabled = true;
		}
	}
	
	private void OnMouseOver()
	{
		if(Vector3.Distance(transform.position, Camera.main.transform.position)<3.5f)
		{
			Debug.Log("Let's glow.");
			_targetColor = GlowColor;
			enabled = true;
		}
		else
		{
			_targetColor = Color.black;
		}
	}

	private void OnMouseExit()
	{
		_targetColor = Color.black;
		enabled = true;
	}

	/// <summary>
	/// Loop over all cached materials and update their color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetColor("_GlowColor", _currentColor);
		}

		if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}
	}
}
