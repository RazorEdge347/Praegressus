using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCube : MonoBehaviour
{

    [Range(0, 5)]
    public int powerlevel;
    private int timer;
    private float gravity = 25f;
    private float force;
    private bool ground;
    private bool chosenone;
    private float radius;
    private int frame;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        powerlevel = 0;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name != "Player")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<ObjectGridAlignment>().enabled = true;
        }

    }
/*
    public bool add(GameObject cube)
    {
        if (powerlevel < 5)
        {
            if (cube.tag == transform.tag)
            {
                powerlevel += 1;
                print(powerlevel);
                return true;
            }
            GetComponent<PowerLevelScript>().setPower(powerlevel);
        }
        
            return false;
        
    }

    public bool sub(GameObject cube)
    {
        if (powerlevel > 0)
        {
            if (cube.tag == transform.tag)
            {
                powerlevel -= 1;
                
                return true;
            }

            GetComponent<PowerLevelScript>().setPower(powerlevel);
        }

        return false;
    }

    */
    // Update is called once per frame

  
    void Update()
    {

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), 1.1f))
        {
            //Debug.Log ("isGrounded");
            ground = true;
        }
        else
        {
            //Debug.Log ("Airborn");
            ground = false;
        }



        powerlevel = transform.GetChild(0).transform.GetChild(0).GetComponent<PowerLevelScript>().PWLVL;
        Player_controller called = GameObject.Find("Player").GetComponent<Player_controller>();
        
        

        if (frame == 60)
        {
            frame = 0;
            timer -= 1;
            print("lets go");
        }
      

        switch (powerlevel)
        {
            case 0: print("Come on man"); break;
            case 1: if (called.called == true) { timer = 2; frame = 0; radius = 5f;  } called.called = false; if (timer == 0) {  GetComponent<ObjectGridAlignment>().enabled = false;  for (int i = 1; i> 0; i--) { Instantiate(this.gameObject); } GetComponent<Rigidbody>().AddExplosionForce(25f, transform.position, radius, 3.0f); transform.GetChild(0).transform.GetChild(0).GetComponent<PowerLevelScript>().setPower(-1); } break;
            case 2: if (called.called == true) { timer = 2; frame = 0; radius = 10f; } called.called = false; if (timer == 0) { for (int i = 2; i == 0; i--) Instantiate(this.gameObject); GetComponent<Rigidbody>().AddExplosionForce(35f, transform.position, radius, 3.0f); } break;
            case 3: if (called.called == true) { timer = 2; frame = 0; radius = 15f; } called.called = false; if (timer == 0) { for (int i = 3; i == 0; i--) Instantiate(this.gameObject); GetComponent<Rigidbody>().AddExplosionForce(45f, transform.position, radius, 3.0f); } break;
            case 4: if (called.called == true) { timer = 2; frame = 0; radius = 20f; } called.called = false; if (timer == 0) { for (int i = 4; i == 0; i--) Instantiate(this.gameObject); GetComponent<Rigidbody>().AddExplosionForce(55f, transform.position, radius, 3.0f); } break;
            case 5: if (called.called == true) { timer = 2; frame = 0; radius = 5f; } called.called = false; if (timer == 0) { for (int i = 5; i == 0; i--) Instantiate(this.gameObject); GetComponent<Rigidbody>().AddExplosionForce(65f, transform.position, radius, 3.0f); } break;
        };

        if (ground == false)
            GetComponent<Rigidbody>().velocity -= new Vector3(0, 2, 0);
        
        frame++;
    }
}