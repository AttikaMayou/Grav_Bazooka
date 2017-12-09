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
	[HideInInspector]
	public bool estEntree;
	public bool frottement;
	Vector3[] veloPos = new Vector3[100];
	Vector3[] tabPos = new Vector3[100];

	public Vector3[] tabPoss = new Vector3[100];
	public Vector3 veloActu;
	public Vector3 veloSuivante;

	public Vector3 posSuivante;
	public Vector3 posActu;
	[HideInInspector]
	public float vitesseMax;

	void Start () {
		rb = GetComponent<Rigidbody> ();

		for (int i = 0; i< tabPos.Length;i++){
			if (i == 0) {
				tabPos [i] = transform.position;
				veloPos [i] = rb.velocity;
			} else {
				tabPos [i] = tabPos[i-1] + veloPos [i-1] * Time.fixedDeltaTime;
				veloPos[i] = new Vector3(veloPos[i-1].x + CalculeForce (ListPts,tabPos[i]).x, 
					veloPos[i-1].y + CalculeForce (ListPts,tabPos[i]).y,0);
			}
		}
		for (int i = 0; i < tabPos.Length; i++) {
			tabPoss [i] = tabPos [i];
		}
	}

	void FixedUpdate () {
		if (CalculeForce (ListPts,transform.position) == Vector3.zero && frottement)
			ReductionForce ();
		
		transform.up = rb.velocity.normalized;
		rb.velocity = rb.velocity + CalculeForce (ListPts,transform.position);

		posActu = transform.position;
		veloActu = rb.velocity;

		posSuivante = posActu + veloActu * Time.fixedDeltaTime;
		veloSuivante = new Vector3(veloActu.x + CalculeForce (ListPts,posSuivante).x, 
			veloActu.y + CalculeForce (ListPts,posSuivante).y,0);
//		if (estEntree) {
			for (int i = 0; i < tabPos.Length; i++) {
				if (i == 0) {
					tabPos [i] = transform.position; //posActuelle
					veloPos [i] = new Vector3 (rb.velocity.x + CalculeForce (ListPts, posSuivante).x, 
						rb.velocity.y + CalculeForce (ListPts, posSuivante).y, 0); //velocité +
				} else {
					tabPos [i] = tabPos [i - 1] + veloPos [i - 1] * Time.fixedDeltaTime;
					veloPos [i] = new Vector3 (veloPos [i - 1].x + CalculeForce (ListPts, tabPos [i]).x, 
						veloPos [i - 1].y + CalculeForce (ListPts, tabPos [i]).y, 0);
				}
			}
		for (int i = 0; i < tabPos.Length; i++) {
			tabPoss [i] = tabPos [i];
		}

		Vector3 veloNm = new Vector3 (rb.velocity.x, rb.velocity.y, 0);
		float vitesse = Mathf.Sqrt (rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
		veloNm /= vitesse;
		if (vitesse != vitesseMax)
			rb.velocity = veloNm*vitesseMax;
//		Debug.Log (vitesse);
//		}
	}

	Vector3 PosSuivante (Vector3 posPre, Vector3 veloPre){
		Vector3 posSuiv = posPre + veloPre * Time.fixedDeltaTime;
		return posSuiv;
	}

	Vector3 VeloSuivante (Vector3 posSuiv, Vector3 veloPre){
		Vector3 veloSuiv = new Vector3(veloPre.x + CalculeForce (ListPts,posSuiv).x, 
			veloPre.y + CalculeForce (ListPts,posSuiv).y,0);
		return veloSuiv;
	}

	void ReductionForce(){
		Vector3 ForceOpposition = -rb.velocity * 0.05f*reducVitesse;
		rb.velocity += ForceOpposition;
	}

	Vector3 CalculeForce(List<GameObject> listGo, Vector3 positionA){
		Vector3 forceTotale = new Vector3 ();
		foreach (GameObject go in listGo) {
			if (go.GetComponent<AttractV2> ().attractionMode == AttractV2.AttractionMode.Lineaire) {
				Vector3 Direction = (go.transform.position - positionA).normalized;
				float distance = Vector3.Distance (positionA, go.transform.position);
				distance /= (go.GetComponent<AttractV2> ().portee/2);
				float forceBase = go.GetComponent<AttractV2> ().forceBase;
				if (distance < 1)
					distance = 1;
				forceTotale += (Direction * forceBase)/(distance*distance);
			} else
				
			if (go.GetComponent<AttractV2> ().attractionMode == AttractV2.AttractionMode.Rotation){
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
