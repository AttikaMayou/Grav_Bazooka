using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractV2 : MonoBehaviour {
	
	public float portee;
	public float forceBase;
	[HideInInspector]
	public List<GameObject> ListGoAppliquee = new List<GameObject>();
	public enum AttractionMode {Rotation,Lineaire}
	public AttractionMode attractionMode;
	public float vitesseRotation;
	public float vitesseAttraction;
	public bool reductionScale;
	public float lapseTimeReduc;
	public float timeFreez;
	float timeInstance;

	void Start () {
		
		transform.localScale *= portee;
		Collider[] Colliders = Physics.OverlapSphere (transform.position, portee/2f, 1 << 8);
		foreach (Collider col in Colliders) {
			DetectionObjs (col);
		}
		timeInstance = Time.time;
		if(reductionScale)
			StartCoroutine ("Freezing");
	}

	IEnumerator Freezing(){
		yield return new WaitForSeconds (timeFreez);
		StartCoroutine ("ReductionScale");
	}

	IEnumerator ReductionScale(){
		if (transform.localScale.x < 0.3f) {
			StopCoroutine ("ReductionScale");
			Depop ();
		}
		transform.localScale -= new Vector3 (0.05f, 0.05f, 0.05f);
		yield return new WaitForSeconds (0.1f * lapseTimeReduc);
		StartCoroutine ("ReductionScale");
	}

	void OnTriggerEnter(Collider col){
		DetectionObjs (col);
	}

	void OnTriggerExit(Collider col){
		ObjetHorsZone (col);
	}


	void ObjetHorsZone(Collider col){
		for (int i = ListGoAppliquee.Count - 1; i >= 0; i--) {
			if(ListGoAppliquee[i] == col.gameObject)
				ListGoAppliquee.RemoveAt (i);
		}
		for(int i = col.GetComponent<tabAttractV2>().ListPts.Count -1; i >= 0; i--){
			if (col.GetComponent<tabAttractV2> ().ListPts [i] == gameObject)
				col.GetComponent<tabAttractV2> ().ListPts.RemoveAt (i);
		}
	}

	public void Depop(){
		for (int i = 0; i < ListGoAppliquee.Count; i++) {

			for(int j = ListGoAppliquee[i].GetComponent<tabAttractV2>().ListPts.Count -1; j >= 0; j--){
				
				if (ListGoAppliquee[i].GetComponent<tabAttractV2> ().ListPts [j] == gameObject)
					ListGoAppliquee[i].GetComponent<tabAttractV2> ().ListPts.RemoveAt (j);
			}
		}
		GameObject.Find ("fond").GetComponent<GravePop> ().nbrBlMax++;
		Destroy (gameObject.transform.parent.gameObject);
	}

	void DetectionObjs(Collider col){
		bool estDansLesModifs = false;
		foreach (GameObject go in ListGoAppliquee) {
			if (go == col.gameObject) {
				estDansLesModifs = true;
				bool estDsTab = false;

				for (int i = 0; i < col.GetComponent<tabAttractV2> ().ListPts.Count; i++) {
					if (col.GetComponent<tabAttractV2> ().ListPts [i] == gameObject)
						estDsTab = true;
				}
				if (!estDsTab)
					col.GetComponent<tabAttractV2> ().ListPts.Add (gameObject);
			}
		}
		if (!estDansLesModifs) {
			ListGoAppliquee.Add (col.gameObject);
			if (attractionMode == AttractionMode.Rotation) {
				//calcul de la direction
				Vector3 veloDir = col.GetComponent<Rigidbody> ().velocity.normalized;
				Vector3 objToCenter = (col.transform.position - transform.position).normalized;
				Vector3 prodVec = Vector3.Cross (veloDir, objToCenter);
				if (prodVec.z < 0)
					col.GetComponent<tabAttractV2> ().direction = 1;
				else 
					col.GetComponent<tabAttractV2> ().direction = -1;
			}
		}
	}
}
