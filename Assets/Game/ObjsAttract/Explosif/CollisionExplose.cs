using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExplose : MonoBehaviour {
	public float forceExplo;
	public float porteeExplo;

	void OnCollisionEnter(){
		Vector3 posPopExplo = transform.position;
		Collider[] Colliders = Physics.OverlapSphere (posPopExplo, porteeExplo, 1 << 8);
		foreach(Collider coll in Colliders){
			coll.GetComponent<Rigidbody> ().AddExplosionForce (forceExplo, posPopExplo, porteeExplo);
		}
	}
}
