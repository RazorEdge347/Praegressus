using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_controll : MonoBehaviour {
    private float axis_x;
    private float axis_y;
    private Player_controller player; 
     
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_controller>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        

        
	}

    private void FixedUpdate()
    {

        axis_x = -Input.GetAxis("Mouse X");
        axis_y = Input.GetAxis("Mouse Y");

        transform.position = player.GetComponent<Rigidbody>().transform.position;
        transform.eulerAngles += new Vector3(-axis_y, axis_x, 0) * 120f * Time.fixedDeltaTime;
        

    }
}
