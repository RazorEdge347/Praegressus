using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	
	///mf = GetComponent<MeshFilter> ();
	///vector3[0] = Vector3 (minX-ClippingAmmount, minY-ClippingAmmount, minZ-ClippingAmmount);
	///vector3[1] = Vector3 (maxX+ClippingAmmount, maxY+ClippingAmmount, maxZ+ClippingAmmount);
	static public Vector3[] getBoundsOfMesh(MeshFilter mf, float ClippingAmmount){
		

		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;
		float minZ = Mathf.Infinity;
		float maxZ = Mathf.NegativeInfinity;

		Vector3[] MeshListverts;
		MeshListverts = mf.mesh.vertices;

		foreach (Vector3 v in MeshListverts) {
			if (v.x < minX) {
				minX = v.x;
			}
			if (v.x > maxX) {
				maxX = v.x;
			}

			if (v.y < minY) {
				minY = v.y;
			}
			if (v.y > maxY) {
				maxY = v.y;
			}

			if (v.z < minZ) {
				minZ = v.z;
			}
			if (v.z > maxZ) {
				maxZ = v.z;
			}
		}

		Vector3[] MeshBounds = { new Vector3 (minX-ClippingAmmount, minY-ClippingAmmount, minZ-ClippingAmmount), new Vector3 (maxX+ClippingAmmount, maxY+ClippingAmmount, maxZ+ClippingAmmount) };
		Debug.Log (MeshBounds);
		return MeshBounds;
	}

	static public float SnapValue(float input, float snapValue){
		float snapedValue = snapValue * (Mathf.Round (input / snapValue));
		return snapedValue;
	}

	static public int SnapIntValue(int input, int snapValue){
		int snapedValue = snapValue * (int)(Mathf.Round (input / snapValue));
		return snapedValue;
	}
}
