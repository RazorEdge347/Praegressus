using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectGridAlignment : MonoBehaviour {

	public float GridSize = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//GRID
		transform.position = new Vector3 (Utils.SnapValue(transform.position.x,GridSize), Utils.SnapValue(transform.position.y,GridSize), Utils.SnapValue(transform.position.z,GridSize));
	}

}
