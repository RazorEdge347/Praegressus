using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastOcclusion : MonoBehaviour {

	public float Cliprange = 1f;
	public float renderRange = 50f;

	GameObject[] VisibleObjLst;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		VisibleObjLst = GameObject.FindGameObjectsWithTag ("VoxelTerrain");

		foreach(GameObject obj in VisibleObjLst){
			
			/*if (obj.GetComponent<Renderer> ().isVisible) {
				obj.GetComponent<Renderer> ().enabled = true;
				//obj.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				obj.gameObject.GetComponent<Renderer> ().enabled = false;

				//obj.GetComponent<MeshRenderer> ().enabled = false;
			}*/
			Ray ray = new Ray (transform.position, obj.transform.position - transform.position);
			RaycastHit hit; 
			if (Physics.Raycast (ray, out hit , renderRange) && (obj.transform.position - hit.transform.position).magnitude <= Cliprange ) {
				//hit.collider.gameObject == obj.gameObject
				Debug.DrawRay (transform.position, obj.transform.position - transform.position, Color.green);
				obj.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				//Debug.DrawRay (transform.position, obj.transform.position - transform.position, Color.red);
				obj.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}
}
