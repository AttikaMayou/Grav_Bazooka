using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSc : MonoBehaviour {

	public void SceneFPS(){
		SceneManager.LoadScene (0);
	}
	public void Scene2D(){
		SceneManager.LoadScene (1);
	}
	public void SceneMorphing(){
		SceneManager.LoadScene (3);
	}
	public void SceneMenu(){
		SceneManager.LoadScene (2);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape))
			SceneMenu ();
	}
}
