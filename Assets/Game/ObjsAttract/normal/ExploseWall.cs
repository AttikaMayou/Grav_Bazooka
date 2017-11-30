using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploseWall : MonoBehaviour {
	public float vitesseMinimum;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Mur") {
			float vitesse = Mathf.Sqrt ((rb.velocity.x * rb.velocity.x) + (rb.velocity.y * rb.velocity.y));
			if (vitesse > vitesseMinimum)
				StartCoroutine ("DestroyWall", col);
		}
	}

	IEnumerator DestroyWall(Collider col){
		yield return new WaitForSeconds (0.01f);
		Destroy (col.gameObject);
	}
}
