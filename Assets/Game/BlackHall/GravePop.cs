﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravePop : MonoBehaviour {
	
	bool popGrav;
	GameObject GravGo;
	public GameObject BouleGrav;
	public GameObject Cube;
	float timeClic;
	public static Vector3 posCursorOnTable;
	bool inDrag;
	GameObject GoInDrag;

	public int nbrBlMax;

	float timeDragDrop;
	bool timeDragDropSave;

	void Update(){
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, 2000f);


		List<GameObject> listBl = new List<GameObject> ();
		GameObject blFocus = null;

		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider.tag == "Fond")
				posCursorOnTable = hits [i].point;
			
			if (hits [i].collider.tag == "BlackHall") {
				if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
					listBl.Add (hits [i].collider.gameObject);
					foreach (GameObject bl in listBl) {
						if (blFocus == null)
							blFocus = bl;
						else if (Vector3.Distance (bl.transform.position, posCursorOnTable) < Vector3.Distance (blFocus.transform.position, posCursorOnTable))
							blFocus = bl;
					}
				}
			}
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && blFocus != null)
			blFocus.transform.localScale -= new Vector3 (0.5f, 0.5f, 0.5f);
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && blFocus != null)
			blFocus.transform.localScale += new Vector3 (0.5f, 0.5f, 0.5f);
		
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			bool blackHallTouche = false;
			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].collider.tag == "BlackHallCible") {
					blackHallTouche = true;
				}
			}

			if (!blackHallTouche) {
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider.tag == "Fond" && nbrBlMax > 0) {
						Instantiate (BouleGrav, hits [i].point, Quaternion.identity);
						nbrBlMax--;
					}
				}
			}
			else if (blackHallTouche)
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider.tag == "BlackHallCible") {
						timeClic = Time.time;
						inDrag = true;
						GoInDrag = hits [i].transform.gameObject;
					}
				}
		}
		if (inDrag) {
			GoInDrag.transform.position = Vector3.Lerp (GoInDrag.transform.position, posCursorOnTable, 0.33f);
			if (!timeDragDropSave) {
				timeDragDrop = Time.time;
				timeDragDropSave = true;
			} else if (timeDragDropSave && timeDragDrop + 0.13f < Time.time) {
				GoInDrag.transform.localScale -= new Vector3 (0.008f, 0.008f, 0.008f);
				timeDragDrop = Time.time;
			}
		} else if (!inDrag)
			timeDragDropSave = false;	


		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].collider.tag == "BlackHallCible") {
					inDrag = false;
					GoInDrag = null;
					if (Time.time - 0.5f < timeClic) {
						hits [i].collider.transform.GetChild (0).GetComponent<AttractV2> ().Depop ();
					}
				}
			}
		}
	}
}
