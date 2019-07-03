using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour {

	public static PieceGenerator singleton;

	public PieceOptions yugiPieces;
    public PieceOptions kaibaPieces;

	public enum Character{
		YUGI,
        KAIBA
	}

	private Dictionary<Character, PieceOptions> characterPieces;

	[System.Serializable]
	public struct PieceOptions{
		public ChessPiece pawn;
		public ChessPiece rook;
		public ChessPiece bishop;
		public ChessPiece knight;
		public ChessPiece queen;
		public ChessPiece king;
	}

	void Awake(){
		characterPieces = new Dictionary<Character, PieceOptions> ();
		characterPieces.Add (Character.YUGI, yugiPieces);
        characterPieces.Add(Character.KAIBA, kaibaPieces);

		singleton = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Generate a chess piece for the giver character and pieceType
	public ChessPiece GeneratePiece(Character character, ChessPiece.PieceType pieceType){
		// Switch based on the piece type
		ChessPiece newPiece = null;
		switch (pieceType) {
		case ChessPiece.PieceType.Pawn:
			newPiece = Instantiate (characterPieces [character].pawn);
			break;
		case ChessPiece.PieceType.Rook:
			newPiece = Instantiate (characterPieces [character].rook);
			break;
		case ChessPiece.PieceType.Knight:
			newPiece = Instantiate (characterPieces [character].knight);
			break;
		case ChessPiece.PieceType.Bishop:
			newPiece = Instantiate (characterPieces [character].bishop);
			break;
		case ChessPiece.PieceType.Queen:
			newPiece = Instantiate (characterPieces [character].queen);
			break;
		case ChessPiece.PieceType.King:
			newPiece = Instantiate (characterPieces [character].king);
			break;
		}

		return newPiece;
	}

}
