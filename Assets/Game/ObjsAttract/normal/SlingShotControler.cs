using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotControler : MonoBehaviour {
	public GameObject objInstante;
	GameObject objPop;
	bool enInput1;
	Vector3 posMouseOnClic;
	[Range (0,100)]
	public float multiForce;
	public bool sens;
	public GameObject pointilles;
	public GameObject pointillesSprite;
	GameObject pointillesPop;
	List<GameObject> listPointilles = new List<GameObject> ();
	Vector3 directionForce;
	float stepUIPointilles = 1.5f;

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, 2000f);

		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider.tag == "Fond" && Input.GetKeyDown (KeyCode.Mouse1)) {
				objPop = Instantiate (objInstante, GravePop.posCursorOnTable, Quaternion.identity) as GameObject;
				pointillesPop = Instantiate (pointilles, GravePop.posCursorOnTable, Quaternion.identity) as GameObject;
				enInput1 = true;
				posMouseOnClic = GravePop.posCursorOnTable;
			}
		}

		if (Input.GetKey (KeyCode.Mouse1) && enInput1) {
			if(sens)
				directionForce = posMouseOnClic - GravePop.posCursorOnTable;
			else 
				directionForce = GravePop.posCursorOnTable - posMouseOnClic;
			float diffDistance = Vector3.Distance (posMouseOnClic, GravePop.posCursorOnTable);

			float nbrPoint = diffDistance / stepUIPointilles;
			nbrPoint = Mathf.Round (nbrPoint);
			Debug.Log (nbrPoint);
			if (nbrPoint > listPointilles.Count) {
				GameObject instPointSprite = Instantiate (pointillesSprite, pointillesPop.transform.position, Quaternion.identity);
				instPointSprite.transform.SetParent (pointillesPop.transform);
				listPointilles.Add (instPointSprite);
			} else if (nbrPoint < listPointilles.Count) {
				Destroy(listPointilles[listPointilles.Count - 1]);
				listPointilles.RemoveAt (listPointilles.Count - 1);
			}
			for (int i = 0; i < listPointilles.Count; i++) {
				listPointilles [i].transform.right = -pointillesPop.transform.up;
				float yFocus = i * stepUIPointilles;
				if (sens)
					listPointilles [i].transform.localPosition = new Vector3 (0, -yFocus, 0);
				else {
					listPointilles [i].transform.Rotate (0, 0, 180);
					listPointilles [i].transform.localPosition = new Vector3 (0, yFocus, 0);
				}
			}
			float angle = Mathf.Atan2 (directionForce.x, directionForce.y);
			pointillesPop.transform.rotation = Quaternion.Lerp (pointillesPop.transform.rotation, Quaternion.EulerAngles (0, 0, -angle), 0.3333f);
		}

		if (Input.GetKeyUp (KeyCode.Mouse1)) {
			if(sens)
				directionForce = posMouseOnClic - GravePop.posCursorOnTable;
			else 
				directionForce = GravePop.posCursorOnTable - posMouseOnClic;
			float diffDistance = Vector3.Distance (posMouseOnClic, GravePop.posCursorOnTable);
			objPop.GetComponent<Rigidbody> ().AddForce (directionForce * diffDistance *100);
			for (int i = listPointilles.Count-1; i >= 0; i--) {
				Destroy (listPointilles [i]);
				listPointilles.RemoveAt (i);
			}
			Destroy (pointillesPop);
			enInput1 = false;
		}
	}
}
