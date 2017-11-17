using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morphing : MonoBehaviour {
	Vector3 posCursorOnTable;
	bool isMorphing;
	public GameObject plane;
	bool isMorphingDisplacement;

	void Awake () {
//		plane = GameObject.Find ("plane");
	}

	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, 200f);

		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider.tag == "MorphingPlane") {
				posCursorOnTable = hits [i].point;
				if (Input.GetKeyDown (KeyCode.Mouse0))
					isMorphing = true;
				if (Input.GetKeyDown (KeyCode.Mouse1)) {
					isMorphingDisplacement = true;
					GetVerticesDisplacement ();
				}
			}
		}

		if (Input.GetKeyUp (KeyCode.Mouse0))
			isMorphing = false;
		if (Input.GetKeyUp (KeyCode.Mouse1))
			isMorphingDisplacement = false;
		
		if (isMorphing)
			Morph ();
		if (isMorphingDisplacement)
			MorphDisplacement ();
	}

	void Morph(){
		Mesh meshPlane = plane.GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = meshPlane.vertices;
		for(int i = 0; i < vertices.Length ; i++){
			float distance = Vector3.Distance (posCursorOnTable, vertices [i]);
			if(distance < 1f)
				vertices[i] -= (Vector3.up / distance)*00.1f;
		}
		meshPlane.vertices = vertices;
		meshPlane.RecalculateBounds();
		meshPlane.RecalculateNormals();
		plane.GetComponent<MeshCollider> ().sharedMesh = meshPlane;
	}
	 
	void GetVerticesDisplacement(){
		
	}
	void MorphDisplacement(){
		
	}
}
