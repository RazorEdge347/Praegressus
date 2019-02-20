using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(BoxCollider))]

[ExecuteInEditMode]
public class Chunk : MonoBehaviour {

	public int widthXZ = 20, heightY = 20;
	public int[,,] MapMatrix;
	public GameObject levelMesh;
	public int Voxelscale = 1;
	public bool ToggleBlockGizmos = false;
	public List<GameObject> ReactionCubesLst = new List<GameObject>();

	private List<Vector3> verts = new List<Vector3>();
	private List<int> tris = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();


	private Mesh mesh;

	void Awake () {
		GetComponent<BoxCollider> ().isTrigger = true;
		GetComponent<BoxCollider> ().size = new Vector3 (widthXZ, heightY, widthXZ);
		GetComponent<BoxCollider> ().center = new Vector3 ((widthXZ/ 2f)-1, (heightY / 2f), (widthXZ / 2f)-1);

		mesh = new Mesh ();
		MapMatrix = new int[widthXZ, heightY, widthXZ];

		for (int x = 0; x < widthXZ; x += Voxelscale) {
			for (int y = 0; y < heightY; y += Voxelscale) {
				for (int z = 0; z < widthXZ; z += Voxelscale) {
					
					if (MeshScanner(x, y, z,levelMesh)) {
						MapMatrix [x, y, z] = 1;
					}

				}
			}
		}
		/*MapMatrix [1, 7, 1] = 1;
		MapMatrix [1, 6, 1] = 1;
		MapMatrix [1, 5, 1] = 1;*/
		levelMesh.GetComponent<MeshRenderer> ().enabled = false;
		CalculateMesh ();
	}

	public List<GameObject> getReactionCubes (){
		return ReactionCubesLst;
	}

	public void SetupChunk(int wxz, int y, int vxlScl, GameObject LVLmesh){
		widthXZ = wxz;
		heightY = y;
		Voxelscale = vxlScl;
		levelMesh = LVLmesh;
	}

	public void CalculateMesh (){
		for (int x = 0; x < widthXZ; x++) {
			for (int y = 0; y < heightY; y++) {
				for (int z = 0; z < widthXZ; z++) {
					if (MapMatrix [x, y, z] != 0) {

						if (isBlockTransparent(x,y,z+Voxelscale)) {
							AddCubeFaceFront (x,y,z);
						}

						if (isBlockTransparent(x,y,z-Voxelscale)) {
							AddCubeFaceBack (x,y,z-Voxelscale);
						}

						if (isBlockTransparent(x,y+Voxelscale,z)) {
							AddCubeFaceTop (x,y,z-Voxelscale);
						}

						if (isBlockTransparent(x,y-Voxelscale,z)) {
							AddCubeFaceBottom (x,y,z);
						}

						if (isBlockTransparent(x+Voxelscale,y,z)) {
							AddCubeFaceRight(x,y,z);
						}
						if (isBlockTransparent(x-Voxelscale,y,z)) {
							AddCubeFaceLeft(x,y,z);
						}
							
					}
				}
			}
		}

		mesh.vertices = verts.ToArray ();
		mesh.triangles = tris.ToArray ();
		mesh.uv = uvs.ToArray ();

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		MeshUtility.Optimize(mesh);

		GetComponent<MeshCollider>().sharedMesh = mesh;
		GetComponent<MeshFilter>().mesh = mesh;

	}  

	public void AddCubeFaceFront(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (3 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (1 - offset + verts.Count);

		tris.Add (4 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (1 - offset + verts.Count);

		verts.Add (new Vector3 (x + 0, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z + 0));
		verts.Add (new Vector3 (x + 0, y + Voxelscale, z + 0));

	}

	public void AddCubeFaceBack(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (1 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);

		tris.Add (1 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (4 - offset + verts.Count);

		verts.Add (new Vector3 (x + 0, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z + 0));
		verts.Add (new Vector3 (x + 0, y + Voxelscale, z + 0));

	}

	public void AddCubeFaceTop(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (1 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);

		tris.Add (1 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (4 - offset + verts.Count);

		verts.Add (new Vector3 (x + 0, y + Voxelscale, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z + Voxelscale));
		verts.Add (new Vector3 (x + 0, y + Voxelscale, z + Voxelscale));

	}

	public void AddCubeFaceBottom(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (1 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);

		tris.Add (1 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (4 - offset + verts.Count);

		verts.Add (new Vector3 (x + 0, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + 0, z - Voxelscale));
		verts.Add (new Vector3 (x + 0, y + 0, z - Voxelscale));

	}

	public void AddCubeFaceRight(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (1 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);

		tris.Add (1 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (4 - offset + verts.Count);

		verts.Add (new Vector3 (x + 0, y + 0, z + 0));
		verts.Add (new Vector3 (x + 0, y + 0, z - Voxelscale));
		verts.Add (new Vector3 (x + 0, y + Voxelscale, z - Voxelscale));
		verts.Add (new Vector3 (x + 0, y + Voxelscale, z + 0));

	}

	public void AddCubeFaceLeft(int x,int y,int z){

		x = Utils.SnapIntValue (x, Voxelscale);// x + Mathf.FloorToInt (transform.position.x);
		y = Utils.SnapIntValue (y, Voxelscale);//y = y + Mathf.FloorToInt (transform.position.y);
		z = Utils.SnapIntValue (z, Voxelscale);//z = z + Mathf.FloorToInt (transform.position.z);

		int offset = 1;
		tris.Add (3 - offset + verts.Count);
		tris.Add (2 - offset + verts.Count);
		tris.Add (1 - offset + verts.Count);

		tris.Add (4 - offset + verts.Count);
		tris.Add (3 - offset + verts.Count);
		tris.Add (1 - offset + verts.Count);

		verts.Add (new Vector3 (x - Voxelscale, y + 0, z + 0));
		verts.Add (new Vector3 (x - Voxelscale, y + 0, z - Voxelscale));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z - Voxelscale));
		verts.Add (new Vector3 (x - Voxelscale, y + Voxelscale, z + 0));

	}

	bool isBlockTransparent(int x,int y,int z){
		if (x >= widthXZ || y >= heightY || z >=  widthXZ || x < 0 || y < 0 || z < 0) {
			return true;
		}

		if (MapMatrix [x, y, z] == 0) {
			return true;
		}

		return false;
	}

	bool MeshScanner(int x, int y, int z, GameObject mesh){
		RaycastHit hit;

		Vector3 newVoxelpoint = transform.TransformPoint(new Vector3 (x, y, z));
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

		if (//CHECK MESH TO PLACE VOXEL
			(mesh.GetComponent<MeshCollider> ().Raycast (upray, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (downray, out hit, Voxelscale)
			|| mesh.GetComponent<MeshCollider> ().Raycast (leftray, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (rightray, out hit, Voxelscale)
			|| mesh.GetComponent<MeshCollider> ().Raycast (leftray90, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (rightray90, out hit, Voxelscale))

			||

			(mesh.GetComponent<MeshCollider> ().Raycast (INVupray, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (INVdownray, out hit, Voxelscale)
			|| mesh.GetComponent<MeshCollider> ().Raycast (INVleftray, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (INVrightray, out hit, Voxelscale)
			|| mesh.GetComponent<MeshCollider> ().Raycast (INVleftray90, out hit, Voxelscale) || mesh.GetComponent<MeshCollider> ().Raycast (INVrightray90, out hit, Voxelscale))) 
		{
			return true;
		} else {
			return false;
		}

	}

	void OnTriggerEnter(Collider other){
		/*if(other.tag.Contains("Cube"))
		ReactionCubesLst.Add (other.gameObject);*/
	}

	void OnTriggerStay(Collider other){
		if (other.tag.Contains ("Cube"))
		if (!ReactionCubesLst.Contains(other.gameObject))
			ReactionCubesLst.Add (other.gameObject);
	}

	void OnTriggerExit(Collider other){
		ReactionCubesLst.Remove (other.gameObject);
	}

	void OnDrawGizmos(){
		if (ToggleBlockGizmos) {
			for (int x = 0; x < widthXZ; x++) {
				for (int y = 0; y < heightY; y++) {
					for (int z = 0; z < widthXZ; z++) {
						if (MapMatrix [x, y, z] == 0) {
							Gizmos.color = Color.cyan;
							Gizmos.DrawCube (new Vector3 (transform.position.x + x - .5f, transform.position.y + y + .5f, transform.position.z + z - .5f), Vector3.one * .05f);
						}
						if (MapMatrix [x, y, z] != 0) {
							Gizmos.color = Color.green;
							Gizmos.DrawCube (new Vector3 (transform.position.x + x - .5f, transform.position.y + y + .5f, transform.position.z + z - .5f), Vector3.one * .05f);
						}
					}
				}
			}
		} else {
			/*Gizmos.color = Color.green;
			//Gizmos.DrawWireMesh (new Vector3 (transform.position.x + widthXZ / 2f, transform.position.y + heightY / 2f, transform.position.z + widthXZ / 2f), Vector3.one * heightY);
			Gizmos.DrawLine (new Vector3 (transform.position.x,transform.position.y,transform.position.z), new Vector3 (transform.position.x + widthXZ,transform.position.y + heightY, transform.position.z + widthXZ));*/

		}

	}
}

/*public class Block{
	public string ChunkName;
	public int ChunkID;

	public Block(){
		ChunkID = -1;	
	}

	public Block(string name, int id){
		ChunkName = name;
		ChunkID = id;
	}

	public int getBlock(string name){
		switch (name) {
		case "ground":
			return 1;
		default:
			return -1;
		}
	}
}*/
