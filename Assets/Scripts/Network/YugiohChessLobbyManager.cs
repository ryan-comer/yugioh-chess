using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YugiohChessLobbyManager : NetworkLobbyManager {

	// New player added into lobby - set their player number
	public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId)
	{
		// Find the next available number
		var playerNumber = 0;
		foreach (NetworkLobbyPlayer player in lobbySlots) {
			if (player == null) {
				break;
			}

			playerNumber += 1;
		}

		GameObject lobbyPlayer = Instantiate (this.lobbyPlayerPrefab.gameObject);
		lobbyPlayer.GetComponent<LobbyPlayer>().playerNumber = playerNumber;

		return lobbyPlayer;
	}

	public override bool OnLobbyServerSceneLoadedForPlayer (GameObject lobbyPlayer, GameObject gamePlayer)
	{
		var lobby = lobbyPlayer.GetComponent<LobbyPlayer> ();
		var player = gamePlayer.GetComponent<Player> ();

		Debug.Log ("Setting playernumber: " + lobby.playerNumber);
		player.playerNumber = lobby.playerNumber;

		return base.OnLobbyServerSceneLoadedForPlayer (lobbyPlayer, gamePlayer);
	}

}
