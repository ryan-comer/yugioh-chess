using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SyncVar]
	public int playerNumber = 0;	// Player 0/1 are players, rest are observers

	public static Player singleton;
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
	}

	void Start(){
		if (isLocalPlayer) {
			singleton = this;
		}
	}

	// RPC to set the player ID
	[ClientRpc]
	public void RpcSetPlayerID(int playerID){
		if (!isLocalPlayer) {
			return;
		}

		playerNumber = playerID;

		Player.singleton = this;
	}

	// Server command to move a piece
	[Command]
	public void CmdMovePiece(int startingSlotIndex, int endingSlotIndex){
		GameController.singleton.MovePiece(startingSlotIndex, endingSlotIndex);
	}

	// Server command to perform a castle play
	[Command]
	public void CmdCastlePlay(int pieceOneSlotIndex, int pieceTwoSlotIndex){
		GameController.singleton.CastlePlay (pieceOneSlotIndex, pieceTwoSlotIndex);
	}

	// Execute pawn promotion on the server
	[Command]
	public void CmdPawnPromotion(int pawnSlotIndex, ChessPiece.PieceType pieceType){
		RpcPawnPromotion (pawnSlotIndex, pieceType);
	}

	// Pawn promotion on all of the clients
	[ClientRpc]
	public void RpcPawnPromotion(int pawnSlotIndex, ChessPiece.PieceType pieceType){
		ChessPiece pawn = GameController.singleton.gameBoard.chessSlots [pawnSlotIndex].currentPiece;	// Pawn to promote
		ChessPiece newPiece = PieceGenerator.singleton.GeneratePiece(PieceGenerator.Character.YUGI, pieceType);

		// Instantiate the new piece
		ChessBoard chessBoard = GameController.singleton.gameBoard;
		chessBoard.replacePiece (pawn, newPiece);

		List<int> playerList = new List<int> ();
		if (chessBoard.checkForCheck (0)) {
			playerList.Add (0);
		}
		if (chessBoard.checkForCheck (1)) {
			playerList.Add (1);
		}


		GameController.singleton.playerInCheck.Invoke (playerList.ToArray ());

		GameController.singleton.controlsEnabled = true;
	}

}
