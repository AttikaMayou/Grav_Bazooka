using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFps : MonoBehaviour {

	public float vitesse;
	public float vitesseMouse;
	public GameObject Cible;
	bool inputMouse;
	public GameObject BlackHall;
	public Transform Canon;
	public float forceProj;
	public GameObject proj;
	List<GameObject> ListProj = new List<GameObject>();
	List<GameObject> ListBL = new List<GameObject> ();
	GameObject blInDrag;
	bool inDrag;
	public Transform transfoDrag;

	void Awake () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		MouvementCamera ();
		RotationCamera ();
		//PopBlackHall ();
		GravityGunPop();
		RayCastForDrag ();
	}
	
	void MouvementCamera(){
		float hori = Input.GetAxis ("Horizontal");
		float verti = Input.GetAxis ("Vertical");
		if (hori != 0)
			transform.parent.position = Vector3.Lerp (transform.parent.position,transform.position + new Vector3(transform.right.x,0,transform.right.z)*hori, 0.33f);
		if (verti != 0)
			transform.parent.position = Vector3.Lerp (transform.parent.position, transform.position + (transform.forward * verti), 0.33f);
		if(Input.GetKey(KeyCode.Space))
			transform.parent.position = Vector3.Lerp (transform.parent.position,transform.parent.position +(transform.parent.up), 0.33f);
		if(Input.GetKey(KeyCode.C))
			transform.parent.position = Vector3.Lerp (transform.parent.position,transform.parent.position -(transform.parent.up), 0.33f);
	}

	void RotationCamera(){
		if (Input.GetAxis ("Mouse X") != 0)
			transform.parent.Rotate (transform.parent.up * vitesseMouse * Input.GetAxis ("Mouse X"));
		if (Input.GetAxis ("Mouse Y") != 0 && !Input.GetKey (KeyCode.Mouse0))
			transform.Rotate (Vector3.right* vitesseMouse * Input.GetAxis ("Mouse Y"));
	}

	void PopBlackHall(){
		
		if (Input.GetKey (KeyCode.Mouse0) && (Input.GetAxis ("Mouse Y") != 0)) {
			Cible.transform.position = Vector3.Lerp (Cible.transform.position,Cible.transform.position +(Cible.transform.forward* Input.GetAxis ("Mouse Y")), 0.33f);
		}
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			Instantiate (BlackHall, Cible.transform.position, Quaternion.identity);
		}
	}

	void GravityGunPop(){
		
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			GameObject pro = Instantiate (proj, Canon.position, Quaternion.identity) as GameObject;
			pro.GetComponent<Rigidbody> ().AddForce (transform.forward * forceProj);
			ListProj.Add (pro);
		}

		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			for (int i = ListProj.Count - 1; i >= 0; i--) {
				GameObject blInst = Instantiate (BlackHall, ListProj[i].transform.position, Quaternion.identity) as GameObject;
				ListBL.Add (blInst);
				Destroy (ListProj [i]);
				ListProj.RemoveAt (i);
			}
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			for (int i = ListBL.Count - 1; i >= 0; i--) {
				ListBL [i].transform.GetChild(0).gameObject.GetComponent<AttractV2> ().Depop ();
				Destroy (ListBL [i]);
				ListBL.RemoveAt (i);
			}
		}
	}
	//NEWCDAV10
	void RayCastForDrag(){
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, 2000f);

		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider.gameObject.tag == "BlackHall" && Input.GetKeyDown(KeyCode.E)) {
				blInDrag = hits [i].collider.transform.parent.gameObject;
				inDrag = true;
				transfoDrag.position = hits [i].point;
			}
		}
		if (inDrag) {
			blInDrag.transform.position = Vector3.Lerp (blInDrag.transform.position, transfoDrag.position, 0.33f);
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			inDrag = false;
			blInDrag = null;
		}
	}
}
