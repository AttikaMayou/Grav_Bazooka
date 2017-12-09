using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popObjs : MonoBehaviour {
	public float timerPop;
	public GameObject objAPop;
	public float forceAuPop;
	int nbrPop;
	public int maxPop;

	void Start () {
		StartCoroutine ("Popip");
	}

	IEnumerator Popip(){
		yield return new WaitForSeconds (timerPop);
		Vector3 posPop = new Vector3 (Random.Range (-20, 20), 20, 0);
		GameObject goPop = Instantiate (objAPop,posPop,Quaternion.identity) as GameObject;
		goPop.GetComponent<Rigidbody> ().AddForce (0, -1*forceAuPop*100, 0);
		goPop.GetComponent<tabAttractV2> ().vitesseMax = forceAuPop;
		if(nbrPop < maxPop-1)
			StartCoroutine ("Popip");
		else
			StopCoroutine("Popip");
		nbrPop++;
	}
}
