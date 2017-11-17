using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabAttractV2 : MonoBehaviour {
	
	public bool additionForce;
	[HideInInspector]
	public List<Vector3> ListForces = new List<Vector3>();
	[HideInInspector]
	public List<GameObject> ListPts = new List<GameObject>();
	Rigidbody rb;
	public float reducVitesse;
	[HideInInspector]
	public int direction;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		
		if (!additionForce) {
			rb.velocity = CalculeForce (ListPts)*4;
		} else if (additionForce)
			rb.AddForce (CalculeForce (ListPts)*50, ForceMode.Force);

		if (rb.velocity != Vector3.zero)
			ReductionForce ();
	}

	void ReductionForce(){
		Vector3 ForceOpposition = -rb.velocity * 0.05f*reducVitesse;
		rb.velocity += ForceOpposition;
	}

	void OnDrawGuizmos(){
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (CalculeForce (ListPts), 10);
	}

	Vector3 CalculeForce(List<GameObject> listGo){
		Vector3 forceTotale = new Vector3 ();
		foreach (GameObject go in listGo) {
			if (go.GetComponent<AttractV2> ().attractionMode == AttractV2.AttractionMode.Lineaire) {
				Vector3 Direction = go.transform.position - transform.position;
				float distance = Vector3.Distance (transform.position, go.transform.position);
				float forceBase = go.GetComponent<AttractV2> ().forceBase;
				if (distance < 1)
					distance = 1;
				forceTotale += (Direction * forceBase)/(distance);
			} else 
				
			if (go.GetComponent<AttractV2> ().attractionMode == AttractV2.AttractionMode.Rotation){
//				Vector3 directionVelo = rb.velocity.normalized;
//				if (directionVelo != Vector3.zero) {
//					Vector3 normalPercut = ;
//					float prodScal = Vector3.Dot (directionVelo, centrePosObj);
//					Debug.Log (prodScal);
//				}
				float vitesseAttraction = go.GetComponent<AttractV2> ().vitesseAttraction;
				float vitesseRotation = go.GetComponent<AttractV2> ().vitesseRotation;
				Vector3 Centre = go.transform.position;
				Vector3 PosActu = transform.position;
				float distance = Vector3.Distance (Centre, PosActu);
				float rayon = Vector3.Distance (transform.position, Centre);
				float y = PosActu.y - Centre.y;
				float x = PosActu.x - Centre.x;
				float radActuel = Mathf.Atan2 (y, x);
				Vector3 PointCible = Vector3.zero;
					Mathf.Clamp(vitesseAttraction /= Vector3.Distance (Centre, PosActu),1,1000);
				float modifRayon = rayon - vitesseAttraction;
				if (modifRayon < 0)
					modifRayon = 0f;
				PointCible = new Vector3 (Centre.x + modifRayon * Mathf.Cos (radActuel + vitesseRotation*direction), Centre.y + modifRayon * Mathf.Sin (radActuel + vitesseRotation*direction), PosActu.z);
				Vector3 Direction = (PointCible - PosActu).normalized;
				forceTotale += Direction;
			}
		}
		return forceTotale;
	}
}
