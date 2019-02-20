using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementCube : MonoBehaviour
{

    [Range(0, 5)]
    public int powerlevel = 0;
    //private float gravity = 25f;
    //private float force;
	public float radius = 0f;

    // Use this for initialization
    void Start()
    {
		
    }

    void doyourthing(int powerlevel)
    {
      

    }

    void Update()
    {
        powerlevel = transform.GetChild(0).transform.GetChild(0).GetComponent<PowerLevelScript>().PWLVL;

		switch (powerlevel) {
		case 0:
			radius = 0f;
			break;
		case 1:
			radius = 5f;
			break;
		}	
    }

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, radius);
		switch (powerlevel) {
		case 0:
			radius = 0f;
			break;
		case 1:
			radius = 5f;
			break;
		}	
	}
}