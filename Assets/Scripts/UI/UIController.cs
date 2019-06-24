using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public static UIController singleton;

	public Canvas mainCanvas;
	public GameController gameController;
	public ChessBoard chessBoard;

	public Text playersTurnText;
	public Text playerCheckText;
	public Text checkmateText;
	public Text playerNumberText;

	public Button gameOverButton;
	public RectTransform pawnPieceSelection;

	private ChessPiece pawnToChange;

	// Use this for initialization
	void Start () {
		gameController.turnChanged.AddListener (turnChanged);
		gameController.playerInCheck.AddListener (playersInCheck);
		gameController.checkmate.AddListener(checkmate);
		gameController.pawnPromotion.AddListener (pawnPromotion);

		playerCheckText.transform.parent.gameObject.SetActive (false);
		checkmateText.transform.parent.gameObject.SetActive (false);
		gameOverButton.gameObject.SetActive (false);
		pawnPieceSelection.gameObject.SetActive (false);

		pawnPieceSelection.GetChild (1).GetChild (0).GetChild (0).GetComponent<SelectPawnPiece> ().cardSelected.AddListener (promotionOptionSelected);
		pawnPieceSelection.GetChild (1).GetChild (0).GetChild (1).GetComponent<SelectPawnPiece> ().cardSelected.AddListener (promotionOptionSelected);
		pawnPieceSelection.GetChild (1).GetChild (1).GetChild (0).GetComponent<SelectPawnPiece> ().cardSelected.AddListener (promotionOptionSelected);
		pawnPieceSelection.GetChild (1).GetChild (1).GetChild (1).GetComponent<SelectPawnPiece> ().cardSelected.AddListener (promotionOptionSelected);

		singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
		// Update player's turn text
		playersTurnText.text = "Player " + (GameController.singleton.currentPlayer + 1) + "'s turn";
		playerNumberText.text = "Player: " + (Player.singleton.playerNumber + 1).ToString ();
	}

	// Update who's turn it is
	private void turnChanged(int newPlayer){
		//playersTurnText.text = "Player " + (newPlayer + 1) + "'s turn";
	}

	// Player is in check
	private void playersInCheck(int[] players){
		// No players in check, clear
		if (players.Length == 0) {
			playerCheckText.transform.parent.gameObject.SetActive (false);
			return;
		}

		string text = "";
		for (int i = 0; i < players.Length; i++) {
			text += "Player " + (players[i]+1) + " in Check!\n";
		}
		playerCheckText.transform.parent.gameObject.SetActive (true);
		playerCheckText.text = text;
	}

	// Game over
	private void checkmate(int losingPlayer){
		checkmateText.transform.parent.gameObject.SetActive (true);
		int winningPlayer = (losingPlayer + 1) % 2;
		//checkmateText.text = "Checkmate!\nPlayer " + (winningPlayer + 1) + " wins!";

		gameOverButton.gameObject.SetActive (true);
	}

	// Pawn was promoted, check for user input
	private void pawnPromotion(ChessPiece pawn){
		gameController.controlsEnabled = false;
		pawnPieceSelection.gameObject.SetActive (true);

		// Figure out how to populate the cards
		ChessPiece rook = chessBoard.getPlayerPiece(pawn.owner, ChessPiece.PieceType.Rook);
		ChessPiece knight = chessBoard.getPlayerPiece(pawn.owner, ChessPiece.PieceType.Knight);
		ChessPiece bishop = chessBoard.getPlayerPiece(pawn.owner, ChessPiece.PieceType.Bishop);
		ChessPiece queen = chessBoard.getPlayerPiece(pawn.owner, ChessPiece.PieceType.Queen);

		SelectPawnPiece slot1 = pawnPieceSelection.GetChild (1).GetChild (0).GetChild (0).GetComponent<SelectPawnPiece> ();
		SelectPawnPiece slot2 = pawnPieceSelection.GetChild (1).GetChild (0).GetChild (1).GetComponent<SelectPawnPiece> ();
		SelectPawnPiece slot3 = pawnPieceSelection.GetChild (1).GetChild (1).GetChild (0).GetComponent<SelectPawnPiece> ();
		SelectPawnPiece slot4 = pawnPieceSelection.GetChild (1).GetChild (1).GetChild (1).GetComponent<SelectPawnPiece> ();

		slot1.chessPiece_p = queen;
		slot2.chessPiece_p = bishop;
		slot3.chessPiece_p = knight;
		slot4.chessPiece_p = rook;

		slot1.GetComponent<Image> ().sprite = queen.cardFace;
		slot2.GetComponent<Image> ().sprite = bishop.cardFace;
		slot3.GetComponent<Image> ().sprite = knight.cardFace;
		slot4.GetComponent<Image> ().sprite = rook.cardFace;

		pawnToChange = pawn;
	}

	// User selected a new card
	public void promotionOptionSelected(SelectPawnPiece selectPawnPiece){
		pawnPieceSelection.gameObject.SetActive (false);
		Player.singleton.CmdPawnPromotion (pawnToChange.currentY * 8 + pawnToChange.currentX, selectPawnPiece.chessPiece_p.pieceType);
	}

}
