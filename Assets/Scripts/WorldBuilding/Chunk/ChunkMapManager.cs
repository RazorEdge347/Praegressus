using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OcclusionArea))]
[RequireComponent(typeof(ObjectGridAlignment))]

public class ChunkMapManager : MonoBehaviour {

	public int XgridSize = 4;
	public int ZgridSize = 4;
	public int YgridSize = 4;
	public int chunkXZWidth = 20;
	public int chunkYHeight = 20;
	public int voxelScale = 1;
	public GameObject LVLMesh;
	public GameObject Chunkprefab;
	public bool GenerateChunks = false;
	public bool ClearChunks = false;
	public bool toggleGizmos = true;

	private List<Vector3> ChunkCoords;
	private GameObject ReadyChunk;
	public List<GameObject> ChunkLst;
	private List<GameObject> ChunkReactionCubesLst;

	void Update(){
		CheckReactionCubes ();
	}

	public void GenerateMAPChunks(){
		ChunkClear ();
		ReadyChunk = Chunkprefab;
		ReadyChunk.GetComponent<Chunk> ().SetupChunk(chunkXZWidth,chunkYHeight,voxelScale,LVLMesh);
		ChunkCoords = new List<Vector3> ();
		ChunkLst = new List<GameObject> ();
		int i = 0;
		for (int x = 0; x < chunkXZWidth * XgridSize; x += chunkXZWidth) {
			for (int z = 0; z < chunkXZWidth * ZgridSize; z += chunkXZWidth) {
				for (int y = 0; y < chunkYHeight * YgridSize; y += chunkYHeight) {
					ChunkCoords.Add(new Vector3 (transform.position.x + x, transform.position.x + y, transform.position.z + z));
					ChunkLst.Add(Instantiate (ReadyChunk, ChunkCoords[i], Quaternion.identity, transform));
					i++;
				}
			}
		}
		GenerateChunks = false;
	}

	void ChunkClear(){
		foreach (GameObject Child in ChunkLst) {
			Child.GetComponent<Chunk> ().CalculateMesh ();
			DestroyImmediate(Child.gameObject);
		}
		ChunkLst.Clear ();
		ClearChunks = false;
	}

	void CheckReactionCubes(){

		foreach (GameObject Chunk in ChunkLst) {
			ChunkReactionCubesLst = Chunk.GetComponent<Chunk> ().getReactionCubes();
			foreach (GameObject ReactionCube in ChunkReactionCubesLst) {

				Vector3 CubePos = ReactionCube.transform.position;

				if(ReactionCube.GetComponent<Collider>().tag.Contains("Displacement")){
					if(ReactionCube.GetComponent<DisplacementCube>().powerlevel != 0){
						//ReactionCube.GetComponent<DisplacementCube>().radius
					}
				}

				/*if(ReactionCube.GetComponent<Collider>().tag.Contains("Displacement")){
					if(ReactionCube.GetComponent<DisplacementCube>().powerlevel != 0){
						ReactionCube.GetComponent<DisplacementCube>().radius
					}
				}*/
			}
		}
	}

	[ExecuteInEditMode]
	void OnDrawGizmos(){
		GetComponent<OcclusionArea> ().center = new Vector3((chunkXZWidth*(XgridSize))/2f,(chunkYHeight*YgridSize)/2f,(chunkXZWidth*(ZgridSize))/2f);
		GetComponent<OcclusionArea>().size = new Vector3((chunkXZWidth*(XgridSize)),(chunkYHeight*YgridSize),(chunkXZWidth*(ZgridSize)));


		if (GenerateChunks) {
			GenerateMAPChunks ();
		}

		if (ClearChunks) {
			ChunkClear();
		}

		if (toggleGizmos) {
			ChunkCoords = new List<Vector3> ();
			int i = 0;
			for (int x = 0; x < chunkXZWidth * XgridSize; x += chunkXZWidth) {
				for (int z = 0; z < chunkXZWidth * ZgridSize; z += chunkXZWidth) {
					for (int y = 0; y < chunkYHeight * YgridSize; y += chunkYHeight) {
						ChunkCoords.Add(new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z + z));
						Gizmos.color = Color.cyan;
						Gizmos.DrawCube(ChunkCoords[i],Vector3.one);
						Gizmos.DrawLine (ChunkCoords[i], new Vector3 (ChunkCoords[i].x + chunkXZWidth, ChunkCoords[i].y + chunkYHeight , ChunkCoords[i].z + chunkXZWidth));
						//Gizmos.DrawLine (ChunkCoords[i], new Vector3 (ChunkCoords[i].x + chunkXZWidth, ChunkCoords[i].y + chunkYHeight , ChunkCoords[i].z + chunkXZWidth));
						//Gizmos.DrawLine (ChunkCoords[i], new Vector3 (ChunkCoords[i].x + chunkXZWidth, ChunkCoords[i].y , ChunkCoords[i].z + chunkXZWidth));
						i++;
					}
				}
			}

			Gizmos.color = Color.red;
			Gizmos.DrawLine (new Vector3 (transform.position.x,transform.position.y,transform.position.z), new Vector3 (transform.position.x + chunkXZWidth * XgridSize, transform.position.y + chunkYHeight * YgridSize, transform.position.z + chunkXZWidth * ZgridSize));
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(ChunkCoords[0],Vector3.one);
			Gizmos.DrawLine (new Vector3 (transform.position.x,transform.position.y,transform.position.z), new Vector3 (transform.position.x , transform.position.y + chunkYHeight * YgridSize, transform.position.z));
		}
	}
}
