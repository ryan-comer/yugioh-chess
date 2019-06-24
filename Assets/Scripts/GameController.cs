using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

	// Singleton object
	public static GameController singleton;

	public ChessBoard gameBoard;
	public UIController uiController;

	[SyncVar]
	public int currentPlayer = 0;	// The current player who's turn it is

	// State events
	public TurnChanged turnChanged;
	public PlayerInCheck playerInCheck;
	public Checkmate checkmate;
	public PawnPromotion pawnPromotion;

	public bool controlsEnabled = true;	// Used to control when the user can move pieces

	private ChessPiece selectedPiece = null;	// The last piece that was selected
	private bool gameOver = false;	// Flag for when the game is over

	[System.Serializable]
	public class TurnChanged : UnityEvent<int>{}	// Event for when the player turn changes
	[System.Serializable]
	public class PlayerInCheck : UnityEvent<int[]>{}	// Event for when the player is in check
	[System.Serializable]
	public class Checkmate : UnityEvent<int>{}	// Event for when the player is in checkmate
	[System.Serializable]
	public class PawnPromotion : UnityEvent<ChessPiece>{}	// Event for when a pawn promotion occurs

	// Use this for initialization
	void Start () {
		subscribeToEvents ();
	}

	void Awake(){
		singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			deselectPiece (selectedPiece);
			selectedPiece = null;
		}
	}

	// Close the game
	public void QuitGame(){
		Application.Quit ();
	}
		
	private void subscribeToEvents(){
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				ChessSlot chessSlot = gameBoard.getSlot (i, j);
				chessSlot.slotClicked.AddListener (slotClicked);
			}
		}

		gameBoard.pieceDestroyed.AddListener (pieceDestroyed);
		gameBoard.moveComplete.AddListener (moveComplete);
	}

	// Piece was clicked
	private void slotClicked(ChessSlot chessSlot){
		Debug.Log ("Slot clicked");
		Player playerObject = Player.singleton;
		Debug.Log (playerObject);
		// Player object not set yet, skip
		if (playerObject == null) {
			return;
		}

		// Game is over, don't allow input
		if (gameOver) {
			return;
		}

		// Controls disabled
		if (controlsEnabled == false) {
			return;
		}

		// Check if it's your turn
		if (playerObject.playerNumber != currentPlayer) {
			return;
		}


		// Check if there is a chess piece
		ChessPiece chessPiece = chessSlot.currentPiece;

		// Check for castle play
		if (gameBoard.canCastle (selectedPiece, chessPiece)) {
			deselectPiece(selectedPiece);

			int pieceOneSlotIndex = System.Array.IndexOf (gameBoard.chessSlots, gameBoard.getSlot (selectedPiece.currentX, selectedPiece.currentY));
			int pieceTwoSlotIndex = System.Array.IndexOf (gameBoard.chessSlots, gameBoard.getSlot (chessPiece.currentX, chessPiece.currentY));

			Player.singleton.CmdCastlePlay (pieceOneSlotIndex, pieceTwoSlotIndex);
			return;
		}

		// Try to select the piece
		if (chessPiece != null && chessPiece.owner == currentPlayer) {
			deselectPiece (selectedPiece);
			selectPiece (chessPiece);
			selectedPiece = chessPiece;
			return;
		}

		// Piece is selected - try for move
		if (selectedPiece != null) {
			// Check if an animation is going on
			if (gameBoard.movingCharacter) {
				return;
			}

			HashSet<ChessSlot> possibleDestinations = gameBoard.getPossibleDestinations (selectedPiece);

			// Check if the selected slot can be moved to
			if (possibleDestinations.Contains (chessSlot)) {
				// Send server command to move the piece
				int startingSlotIndex = System.Array.IndexOf(gameBoard.chessSlots, gameBoard.getSlot(selectedPiece.currentX, selectedPiece.currentY));
				int endingSlotIndex = System.Array.IndexOf (gameBoard.chessSlots, chessSlot);

				Player.singleton.CmdMovePiece(startingSlotIndex, endingSlotIndex);

				/* Check for check
				if (gameBoard.checkForCheck (currentPlayer)) {
				}
				if (gameBoard.checkForCheck ((currentPlayer + 1) % 2)) {
				}*/

				return;
			}
		}

	}

	// Move a piece on the board
	public void MovePiece(int startingSlotIndex, int endingSlotIndex){
		RpcMovePiece (startingSlotIndex, endingSlotIndex);
	}

	// Client RPC to move a piece
	[ClientRpc]
	void RpcMovePiece(int startingSlotIndex, int endingSlotIndex){
		ChessPiece chessPiece = gameBoard.chessSlots [startingSlotIndex].currentPiece;
		ChessSlot chessSlot = gameBoard.chessSlots [endingSlotIndex];
		deselectPiece (chessPiece);
		gameBoard.movePiece (chessPiece, chessSlot);
	}

	// Perform a castle play
	public void CastlePlay(int pieceOneSlotIndex, int pieceTwoSlotIndex){
		RpcCastlePlay (pieceOneSlotIndex, pieceTwoSlotIndex);
	}

	// Client RPC to perform a castle play
	[ClientRpc]
	void RpcCastlePlay(int pieceOneSlotIndex, int pieceTwoSlotIndex){
		ChessPiece pieceOne = gameBoard.chessSlots [pieceOneSlotIndex].currentPiece;
		ChessPiece pieceTwo = gameBoard.chessSlots [pieceTwoSlotIndex].currentPiece;

		// Perform the castle play
		gameBoard.castlePlay(pieceOne, pieceTwo);
	}

	// Get the players who are in check
	private int[] getPlayersInCheck(){
		List<int> players = new List<int> ();

		if (gameBoard.checkForCheck (0)) {
			players.Add (0);
		}
		if (gameBoard.checkForCheck (1)) {
			players.Add (1);
		}

		return players.ToArray ();
	}

	// Switch to the other player
	private void switchPlayers(){
		if (isServer) {
			currentPlayer = (currentPlayer + 1) % 2;
		}
		turnChanged.Invoke (currentPlayer);
	}

	// Check if the selected slot is a possible move
	private bool checkPossibleMove(ChessSlot chessSlot, ChessPiece chessPiece){
		return true;
	}

	// Check if it is your turn
	private bool checkIsTurn(){
		return true;
	}


	// Callback for when pieces are destroyed
	private void pieceDestroyed(ChessPiece chessPiece){
		// Check for game over
		if (chessPiece.pieceType == ChessPiece.PieceType.King) {
			checkmate.Invoke (chessPiece.owner);
			gameOver = true;
		}
	}

	// Callback for when the turn is complete
	private void moveComplete(){
		// Check if anyone is in check
		playerInCheck.Invoke (getPlayersInCheck());

		// Check for pawn promotion
		ChessPiece pawn = gameBoard.lastMoved;
		switch (Player.singleton.playerNumber) {
		case 0:
			if (pawn.currentY == 7 && pawn.pieceType == ChessPiece.PieceType.Pawn) {
				pawnPromotion.Invoke (pawn);
			}
			break;
		case 1:
			if (pawn.currentY == 0 && pawn.pieceType == ChessPiece.PieceType.Pawn) {
				pawnPromotion.Invoke (pawn);
			}
			break;
		}


		selectedPiece = null;
		switchPlayers ();
	}

	// ********* METHODS FOR SELECTING/DESELECTING PIECES

	private void selectPiece(ChessPiece chessPiece){
		highlightPiece (chessPiece);
		highlightPossibleDestinations (chessPiece);
	}

	private void deselectPiece(ChessPiece chessPiece){
		// Nothing to deselect
		if (chessPiece == null) {
			return;
		}

		unHighlightPiece (chessPiece);
		unHighlightSlots ();
	}

	private void highlightPiece(ChessPiece chessPiece){
		if (chessPiece == null) {
			return;
		}
		gameBoard.getSlot (chessPiece.currentX, chessPiece.currentY).highlight();
	}

	private void unHighlightPiece(ChessPiece chessPiece){
		if (chessPiece == null) {
			return;
		}
		gameBoard.getSlot (chessPiece.currentX, chessPiece.currentY).unHighlight ();
	}

	// Highlight all of the possible slots that this piece can move
	private void highlightPossibleDestinations(ChessPiece chessPiece){
		HashSet<ChessSlot> possibleDestinations = gameBoard.getPossibleDestinations (chessPiece);
		foreach (ChessSlot destination in possibleDestinations) {
			destination.highlight ();
		}

		// Check for castling
		int[] rookXOptions = {0, 7, 0, 7};
		int[] rookYOptions = {0, 0, 7, 7};
		for(int i = 0; i < 4; i++){
			if(gameBoard.canCastle(chessPiece, gameBoard.getSlot(rookXOptions[i], rookYOptions[i]).currentPiece)){
				gameBoard.getSlot (rookXOptions [i], rookYOptions [i]).highlight ();
			}	
		}
	}

	// Unhighlight all of the possible destinations for this chess piece
	private void unHighlightSlots(){
		foreach (ChessSlot slot in gameBoard.chessSlots) {
			slot.unHighlight ();
		}
	}

}
