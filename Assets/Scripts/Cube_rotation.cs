﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
                     
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles+=new Vector3 (0, 2, 0);
	}
}
