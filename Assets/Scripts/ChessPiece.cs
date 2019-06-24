using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessPiece : MonoBehaviour {

	public PieceType pieceType;	// Type of piece
	public int owner;	// Which player owns this piece (0 or 1)
	public int currentX, currentY;	// The current coordinates of this piece
	public Sprite cardFace;	// Used for selecting the monster

	public ChessPiece currentTarget;	// Used to see which piece this piece is attacking

	public float moveSpeed = 1.0f;
	public float attackDistance = 1.0f;

	public bool canCastle = true;	// This is used on rooks and kings to see if a castle play can happen (might refactor later)

	public PieceClicked pieceClicked;
	public TargetHit targetHit;
	public Death death;

	// Types the piece can be
	public enum PieceType{
		Pawn,
		Rook,
		Bishop,
		Knight,
		Queen,
		King
	}

	[System.Serializable]
	public class PieceClicked : UnityEvent<ChessPiece>{}

	[System.Serializable]
	public class TargetHit : UnityEvent{}

	[System.Serializable]
	public class Death : UnityEvent<ChessPiece>{}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Callback from the animation
	public void targetHitCallback(){
		Debug.Log ("Target Hit");
		targetHit.Invoke ();
	}

	// Callback from animation
	public void deathCallback(){
		death.Invoke (this);
	}
		
	void OnMouseOver(){
		checkMouseClick ();
	}

	// See if the user clicks on the object
	private void checkMouseClick(){
		if (Input.GetMouseButtonDown (0)) {
			pieceClicked.Invoke (this);
		}
	}

}
