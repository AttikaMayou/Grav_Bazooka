using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExplose : MonoBehaviour {
	public float forceExplo;
	public float porteeExplo;

	void Start () {
		
	}

	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Explosif") {
			Vector3 posPopExplo = (transform.position + col.transform.position) / 2;
			Collider[] Colliders = Physics.OverlapSphere (posPopExplo, porteeExplo, 1 << 8);
			foreach(Collider coll in Colliders){
				coll.GetComponent<Rigidbody> ().AddExplosionForce (forceExplo, posPopExplo, porteeExplo);
			}
		}
	}
}
