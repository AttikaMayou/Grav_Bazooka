using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneModifier : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown (KeyCode.R))
			ResetScene ();
	}

	void ResetScene(){
		SceneManager.LoadScene (1);
	}
}
