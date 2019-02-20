using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour {
    // Use this for initialization
    public bool pausemode;
    public Vector3 forward;
    private float speed = 10f;
    private Camera camera;
    private bool controlable;
    private Vector3 moveDirection;
    private bool isGrounded;
    public bool grabmode;
    private float gravity = 10f;
    private RaycastHit cube;
    private GameObject grabbed;
    private bool add;
    private bool sub;
    public bool called;

    private ObjectGridAlignment script_al;
        
    void Start () {
        camera = Camera.main;
        Vector3 forward = transform.forward;
        controlable = true;

	}
	
	// Update is called once per frame
	void Update () {

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), 1.1f))
        {
            //Debug.Log ("isGrounded");
            isGrounded = true;
        }
        else
        {
            //Debug.Log ("Airborn");
            isGrounded = false;
        }


    }

    public void Grabbed(bool check , GameObject cube, RaycastHit hit)
    {
        if (check == true)
        {
            script_al = cube.GetComponent<ObjectGridAlignment>();
            script_al.enabled = false;
            cube.transform.gameObject.isStatic = false;
            cube.transform.gameObject.transform.parent = this.transform;
            transform.GetChild(0).transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).transform.gameObject.transform.position = transform.position + transform.forward;
            transform.GetChild(0).transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            grabbed = transform.GetChild(0).transform.gameObject;
        }
        else
        {
            script_al = cube.GetComponent<ObjectGridAlignment>();
            script_al.enabled = true;
            cube.transform.parent = null;
            cube.transform.position = hit.point;
            cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            cube.GetComponent<BoxCollider>().enabled = true;
            
            grabbed = null;

        }
       
    }

    public void shoot (bool check, GameObject cube)
    {
        print("Whats up nigga?");
        cube.transform.parent = null;
        cube.GetComponent<BoxCollider>().enabled = true;
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        cube.GetComponent<Rigidbody>().useGravity = true;
        cube.GetComponent<Rigidbody>().AddForce(camera.transform.forward * 700f);
        grabbed = null;

    }

    private void FixedUpdate()
    {

        
        Vector3 camf = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        Debug.DrawRay(transform.position, camera.transform.forward, Color.green);
        if (controlable == true)
        {

                if (Physics.Raycast(transform.position, camera.transform.forward, out cube, 5f))
                {
                    if (cube.collider.tag.Contains("Cube") == true)
                    {

                        if (grabmode == true)
                        {
                            if (cube.collider.tag == grabbed.tag)
                            {
                                
                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                
                                add = cube.collider.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<PowerLevelScript>().setPower(1);
                                    if (add == true)
                                    {
                                        called = true;
                                        grabmode = false;
                                        print("Increment power level");
                                        Destroy(grabbed);
                                    }
                                    else
                                    {
                                        print("Nigga you can't do that");
                                    }
                                        // Não esquecer de chamar a função do cubo especifico    
                                }

                               
                            }


                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                            grabmode = true;
                            print("Grab a reactonary cube");
                            Grabbed(grabmode, cube.collider.gameObject, cube );
                                
                            }
                            if (Input.GetKeyDown(KeyCode.Mouse1))
                            {

                            sub = cube.collider.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<PowerLevelScript>().setPower(-1);

                            if (sub == true)
                            {
                                called = true;
                                grabmode = true;
                                print("Decrease the power level");
                                Grabbed(grabmode, Instantiate(cube.collider.gameObject, cube.collider.gameObject.transform.position, cube.collider.gameObject.transform.rotation), cube);
                            }
                            else
                            {
                                print("Nigga you can't do that sub");
                            }
                            //Não esquecer de chamar a função do cubo especifico

                        }

                        }
                  }
                 else
                 {
                    if(grabmode == true)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            grabmode = false;
                            print("Get down the object");
                            Grabbed(grabmode, grabbed, cube);
                            
                        }

                    }

                 }

            }else{
                if(grabmode == true)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        grabmode = false;
                        shoot(grabmode, grabbed);
                    }
                }
            }
            

            

            if (Input.GetKeyDown(KeyCode.P))
            {
                pausemode = !pausemode;

            }

         



            //Movement transform 
            moveDirection = (camf * Input.GetAxis("Vertical")) + (Quaternion.Euler(0, 90, 0) * camf * Input.GetAxis("Horizontal"));  
            transform.forward = camf;
            
            moveDirection *= speed;
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0f, 300f, 0f)); 

            }

            if (!isGrounded)
            {
                moveDirection.y -= gravity * Time.fixedDeltaTime;
            }


            

            transform.position += moveDirection * Time.fixedDeltaTime;  
           
        }

        else
        {


        }

        

    }
}
