using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class MeshToVoxelConverter : MonoBehaviour {

	public float Voxelscale = 1f; 
	public float clippingAmmount = 5f;
	public GameObject Voxel;
	public LayerMask LayerMsk;// = 1 << 8;
	//public Vector3 FinalRot;

	private Vector3 newVoxelpoint;

	void Awake(){
		LayerMsk = 1 << gameObject.layer;
		Vector3[] MeshBounds = Utils.getBoundsOfMesh (GetComponent<MeshFilter>(),clippingAmmount);

		for (float x = MeshBounds [0].x; x < MeshBounds [1].x; x += Voxelscale) {
			for (float y = MeshBounds [0].y; y < MeshBounds [1].y; y += Voxelscale) {
				for (float z = MeshBounds [0].z; z < MeshBounds [1].z; z += Voxelscale) {
					newVoxelpoint = transform.TransformPoint(new Vector3 (x, y, z));
					RaycastHit hitUp;
					RaycastHit hitDown;
					RaycastHit hitLeft;
					RaycastHit hitRight;
					/*if(GetComponent<MeshCollider>().bounds.Contains(newVoxelpoint))
						Instantiate (Voxel, newVoxelpoint, transform.rotation).transform.localScale = new Vector3 (Voxelscale,Voxelscale,Voxelscale);*/

					Ray INVupray = new Ray (newVoxelpoint, Vector3.up);
					INVupray.origin = INVupray.GetPoint (Voxelscale);
					INVupray.direction = -INVupray.direction;

					Ray INVdownray = new Ray (newVoxelpoint, Vector3.down);
					INVdownray.origin = INVdownray.GetPoint (Voxelscale);
					INVdownray.direction = -INVdownray.direction;

					Ray INVleftray = new Ray (newVoxelpoint, Vector3.left);
					INVleftray.origin = INVleftray.GetPoint (Voxelscale);
					INVleftray.direction = -INVleftray.direction;

					Ray INVrightray = new Ray (newVoxelpoint, Vector3.right);
					INVrightray.origin = INVrightray.GetPoint (Voxelscale);
					INVrightray.direction = -INVrightray.direction;

					Ray INVleftray90 = new Ray (newVoxelpoint, Quaternion.Euler(0,90,0)*Vector3.left);
					INVleftray.origin = INVleftray.GetPoint (Voxelscale);
					INVleftray.direction = -INVleftray.direction;

					Ray INVrightray90 = new Ray (newVoxelpoint, Quaternion.Euler(0,90,0)*Vector3.right); 
					INVrightray.origin = INVrightray.GetPoint (Voxelscale);
					INVrightray.direction = -INVrightray.direction;

					Ray upray = new Ray (newVoxelpoint, Vector3.up);
				
					Ray downray = new Ray (newVoxelpoint, Vector3.down);
		
					Ray leftray = new Ray (newVoxelpoint, Vector3.left);
				
					Ray rightray = new Ray (newVoxelpoint, Vector3.right);

					Ray leftray90 = new Ray (newVoxelpoint, Quaternion.Euler(0,90,0)*Vector3.left);

					Ray rightray90 = new Ray (newVoxelpoint, Quaternion.Euler(0,90,0)*Vector3.right);

					if( //CHECK MESH TO PLACE VOXEL
						(GetComponent<MeshCollider>().Raycast(upray,out hitUp,Voxelscale) || GetComponent<MeshCollider>().Raycast(downray,out hitDown,Voxelscale)
						|| GetComponent<MeshCollider>().Raycast(leftray,out hitLeft,Voxelscale) || GetComponent<MeshCollider>().Raycast(rightray,out hitRight,Voxelscale)
						|| GetComponent<MeshCollider>().Raycast(leftray90,out hitLeft,Voxelscale) || GetComponent<MeshCollider>().Raycast(rightray90,out hitRight,Voxelscale))

						||	

						(GetComponent<MeshCollider>().Raycast(INVupray,out hitUp,Voxelscale) || GetComponent<MeshCollider>().Raycast(INVdownray,out hitDown,Voxelscale)
						|| GetComponent<MeshCollider>().Raycast(INVleftray,out hitLeft,Voxelscale) || GetComponent<MeshCollider>().Raycast(INVrightray,out hitRight,Voxelscale)
						|| GetComponent<MeshCollider>().Raycast(INVleftray90,out hitLeft,Voxelscale) || GetComponent<MeshCollider>().Raycast(INVrightray90,out hitRight,Voxelscale))
					){
						/*GameObject newVoxel = Instantiate (Voxel, newVoxelpoint, Quaternion.Euler(0,0,0));
						newVoxel.transform.localScale = new Vector3 (Voxelscale, Voxelscale, Voxelscale);
						newVoxel.transform.parent = gameObject.transform;*/	
					}
				}
			}
		}

		/*MeshFilter[] VoxelsMesh = GetComponentsInChildren<MeshFilter> ();
		int Vxleng = VoxelsMesh.Length;
		CombineInstance[] combine = new CombineInstance[Vxleng];

		int i = 0;
		while (i < Vxleng) {
			combine [i].mesh = VoxelsMesh [i].sharedMesh;
			combine[i].transform = VoxelsMesh[i].transform.localToWorldMatrix;
			VoxelsMesh[i].gameObject.SetActive(false);
			if(VoxelsMesh [i].gameObject != gameObject)
			Destroy (VoxelsMesh [i].gameObject);
			i++;

		}

		GetComponent<MeshFilter>().mesh = new Mesh();
		GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		gameObject.SetActive(true);*/

		//transform.rotation = Quaternion.Euler (FinalRot);
		//UnityEditor.PrefabUtility.CreatePrefab ("Praegressus/Assets/Models/Map/Prefabs/VM.prefab",VoxelMaster);
		//Debug.Break();
	}

	void Start () {
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<MeshCollider> ().enabled = false;
	}

}

//http://answers.unity3d.com/questions/777855/bounds-finding-box.html