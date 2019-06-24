using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

	[SyncVar]
	public int playerNumber = 0;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
