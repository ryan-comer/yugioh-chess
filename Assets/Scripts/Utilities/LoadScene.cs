using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Load the scene given by name
	public void LoadSceneFromName(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

}
