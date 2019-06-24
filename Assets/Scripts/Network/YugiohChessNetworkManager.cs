using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

// Network manager to manage the state of a yugioh chess game
public class YugiohChessNetworkManager : NetworkManager {

	private int lastPlayerNumber = 0;	// Increment for every player that joins

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer (conn, playerControllerId);


		Player playerObject = conn.playerControllers [playerControllerId].gameObject.GetComponent<Player> ();
		playerObject.RpcSetPlayerID (lastPlayerNumber);	// Call rpc command on client to set player number
		lastPlayerNumber += 1;
	}

}
