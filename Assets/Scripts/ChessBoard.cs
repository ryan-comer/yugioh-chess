using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ChessBoard : MonoBehaviour {

	public GameController gameController;

	public ChessPiece pawnOne, rookOne, bishopOne, knightOne, queenOne, kingOne;
	public ChessPiece pawnTwo, rookTwo, bishopTwo, knightTwo, queenTwo, kingTwo;

	public ChessPiece lastMoved;

	public ChessSlot[] chessSlots = new ChessSlot[8*8];	// Slots where the pieces can go
	public List<ChessPiece> activePieces = new List<ChessPiece>();
	public List<ChessPiece> destroyedPieces = new List<ChessPiece>();

	public PieceDestroyed pieceDestroyed;	// Event for when a piece is  destroyed
	public MoveComplete moveComplete;	// Event for when a move is complete - switch players and do stuff

	public bool movingCharacter = false;	// Flag for when a character is being moved

	[System.Serializable]
	public class PieceDestroyed : UnityEvent<ChessPiece>{}
	[System.Serializable]
	public class MoveComplete : UnityEvent{}

	private ChessPiece currentTarget;

	// Use this for initialization
	void Start () {
		subscribeToEvents ();
	}

	void Awake(){
		initializeSlots ();
		initializeBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Subscire to relevant events
	private void subscribeToEvents(){
		foreach(ChessPiece chessPiece in activePieces){
			chessPiece.targetHit.AddListener(targetHit);
			chessPiece.death.AddListener (chessPieceDeath);
		}
	}

	// Make sure the slots are set up
	private void initializeSlots(){
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				getSlot (i, j).x = i;
				getSlot (i, j).y = j;
			}
		}
	}

	// Initialize the game board by placing all of the pieces
	private void initializeBoard(){
		// ********* PLAYER ONE ***********
		Quaternion playerOneLookRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		// Pawns
		for (int i = 0; i < 8; i++) {
			int x = i;
			int y = 1;
			ChessPiece newPawn = Instantiate (pawnOne, getSlot(x, y).transform.position, playerOneLookRotation);
			getSlot (x, y).currentPiece = newPawn;
			newPawn.currentX = x;
			newPawn.currentY = y;
			newPawn.owner = 0;

			activePieces.Add (newPawn);
		}

		// Rooks
		ChessPiece newRook = Instantiate(rookOne, getSlot(0, 0).transform.position, playerOneLookRotation);
		getSlot (0, 0).currentPiece = newRook;
		newRook.currentX = 0;
		newRook.currentY = 0;
		newRook.owner = 0;
		activePieces.Add (newRook);

		newRook = Instantiate (rookOne, getSlot (7, 0).transform.position, playerOneLookRotation);
		getSlot (7, 0).currentPiece = newRook;
		newRook.currentX = 7;
		newRook.currentY = 0;
		newRook.owner = 0;
		activePieces.Add (newRook);

		// Knights
		ChessPiece newKnight = Instantiate(knightOne, getSlot(1, 0).transform.position, playerOneLookRotation);
		getSlot (1, 0).currentPiece = newKnight;
		newKnight.currentX = 1;
		newKnight.currentY = 0;
		newKnight.owner = 0;
		activePieces.Add (newKnight);

		newKnight = Instantiate (knightOne, getSlot (6, 0).transform.position, playerOneLookRotation);
		getSlot (6, 0).currentPiece = newKnight;
		newKnight.currentX = 6;
		newKnight.currentY = 0;
		newKnight.owner = 0;
		activePieces.Add (newKnight);

		// Bishops
		ChessPiece newBishop = Instantiate(bishopOne, getSlot(2, 0).transform.position, playerOneLookRotation);
		getSlot (2, 0).currentPiece = newBishop;
		newBishop.currentX = 2;
		newBishop.currentY = 0;
		newBishop.owner = 0;
		activePieces.Add (newBishop);

		newBishop = Instantiate (bishopOne, getSlot (5, 0).transform.position, playerOneLookRotation);
		getSlot (5, 0).currentPiece = newBishop;
		newBishop.currentX = 5;
		newBishop.currentY = 0;
		newBishop.owner = 0;
		activePieces.Add (newBishop);

		// Queen
		ChessPiece newQueen = Instantiate(queenOne, getSlot(4, 0).transform.position, playerOneLookRotation);
		getSlot (4, 0).currentPiece = newQueen;
		newQueen.currentX = 4;
		newQueen.currentY = 0;
		newQueen.owner = 0;
		activePieces.Add (newQueen);

		// King
		ChessPiece newKing = Instantiate(kingOne, getSlot(3, 0).transform.position, playerOneLookRotation);
		getSlot (3, 0).currentPiece = newKing;
		newKing.currentX = 3;
		newKing.currentY = 0;
		newKing.owner = 0;
		activePieces.Add (newKing);

		// ********* PLAYER TWO ***********
		Quaternion playerTwoLookRotation = Quaternion.LookRotation(Vector3.forward * -1f, Vector3.up);
		// Pawns
		for (int i = 0; i < 8; i++) {
			int x = i;
			int y = 6;
			ChessPiece newPawn = Instantiate (pawnOne, getSlot(x, y).transform.position, playerTwoLookRotation);
			getSlot (x, y).currentPiece = newPawn;
			newPawn.currentX = x;
			newPawn.currentY = y;
			newPawn.owner = 1;

			activePieces.Add (newPawn);
		}

		// Rooks
		newRook = Instantiate(rookTwo, getSlot(0, 7).transform.position, playerTwoLookRotation);
		getSlot (0, 7).currentPiece = newRook;
		newRook.currentX = 0;
		newRook.currentY = 7;
		newRook.owner = 1;
		activePieces.Add (newRook);

		newRook = Instantiate (rookOne, getSlot (7, 7).transform.position, playerTwoLookRotation);
		getSlot (7, 7).currentPiece = newRook;
		newRook.currentX = 7;
		newRook.currentY = 7;
		newRook.owner = 1;
		activePieces.Add (newRook);

		// Knights
		newKnight = Instantiate(knightTwo, getSlot(1, 7).transform.position, playerTwoLookRotation);
		getSlot (1, 7).currentPiece = newKnight;
		newKnight.currentX = 1;
		newKnight.currentY = 7;
		newKnight.owner = 1;
		activePieces.Add (newKnight);

		newKnight = Instantiate (knightTwo, getSlot (6, 7).transform.position, playerTwoLookRotation);
		getSlot (6, 7).currentPiece = newKnight;
		newKnight.currentX = 6;
		newKnight.currentY = 7;
		newKnight.owner = 1;
		activePieces.Add (newKnight);

		// Bishops
		newBishop = Instantiate(bishopTwo, getSlot(2, 7).transform.position, playerTwoLookRotation);
		getSlot (2, 7).currentPiece = newBishop;
		newBishop.currentX = 2;
		newBishop.currentY = 7;
		newBishop.owner = 1;
		activePieces.Add (newBishop);

		newBishop = Instantiate (bishopTwo, getSlot (5, 7).transform.position, playerTwoLookRotation);
		getSlot (5, 7).currentPiece = newBishop;
		newBishop.currentX = 5;
		newBishop.currentY = 7;
		newBishop.owner = 1;
		activePieces.Add (newBishop);

		// Queen
		newQueen = Instantiate(queenTwo, getSlot(4, 7).transform.position, playerTwoLookRotation);
		getSlot (4, 7).currentPiece = newQueen;
		newQueen.currentX = 4;
		newQueen.currentY = 7;
		newQueen.owner = 1;
		activePieces.Add (newQueen);

		// King
		newKing = Instantiate(kingTwo, getSlot(3, 7).transform.position, playerTwoLookRotation);
		getSlot (3, 7).currentPiece = newKing;
		newKing.currentX = 3;
		newKing.currentY = 7;
		newKing.owner = 1;
		activePieces.Add (newKing);
	}

	// Move the piece to the destination slot
	public void movePiece(ChessPiece chessPiece, ChessSlot destination){
		// Start the walk animation
		StartCoroutine (moveCoroutine (chessPiece, destination));

		// Update piece state
		chessPiece.canCastle = false;	// Moved, can no longer perform castle
	}

	// Replaces the old piece with the new piece
	public void replacePiece(ChessPiece oldPiece, ChessPiece newPiece){
		// Update new piece values
		newPiece.owner = oldPiece.owner;
		newPiece.transform.position = oldPiece.transform.position;
		if (newPiece.owner == 0) {
			newPiece.transform.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.up);
		} else {
			newPiece.transform.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.up);
		}
		newPiece.currentX = oldPiece.currentX;
		newPiece.currentY = oldPiece.currentY;
		getSlot (oldPiece.currentX, oldPiece.currentY).currentPiece = newPiece;

		activePieces.Remove (oldPiece);
		activePieces.Add (newPiece);

		// Subscribe to callbacks
		newPiece.targetHit.AddListener(targetHit);
		newPiece.death.AddListener (chessPieceDeath);

		Destroy (oldPiece.gameObject);
	}

	// Callback for when a chess piece hits another
	private void targetHit(){
		currentTarget.GetComponent<Animator> ().SetTrigger ("death");
	}

	// Coroutine to slowly move piece
	// Optional, play attack animation
	public IEnumerator moveCoroutine(ChessPiece chessPiece, ChessSlot destination){
		// Set flag to stop control
		movingCharacter = true;

		Animator anim = chessPiece.GetComponent<Animator>();
		anim.SetBool ("walking", true);

		// Look at your destination
		chessPiece.transform.LookAt(destination.transform.position);

		ChessPiece target = destination.currentPiece;

		// Target distance based on if an attack needs to happen
		float targetDistance = 0.05f;
		if (target != null) {
			targetDistance = chessPiece.attackDistance;
		}

		// Move until the distance is closed
		while (Vector3.Distance (chessPiece.transform.position, destination.transform.position) > targetDistance) {
			Vector3 direction = destination.transform.position - chessPiece.transform.position;
			direction.Normalize ();
			Vector3 movement = direction * Time.deltaTime * chessPiece.moveSpeed;

			chessPiece.transform.position += movement;
			yield return null;
		}

		// Reached target destination
		anim.SetBool("walking", false);

		// See if you need to attack
		if (target != null) {
			currentTarget = target;
			chessPiece.currentTarget = target;
			chessPiece.transform.LookAt (target.transform.position);	// Look at the target
			anim.SetTrigger("attack");

			yield return null;	// Give animator time to update

			// Wait for the target to die
			do {
				yield return null;
			} while(target.gameObject.activeInHierarchy);

			// Wait for attack animation to finish
			do {
				yield return null;
			} while(anim.GetCurrentAnimatorStateInfo(0).IsName("attack"));

			// Move to the slot
			anim.SetBool("walking", true);
			targetDistance = 0.05f;
			while (Vector3.Distance (chessPiece.transform.position, destination.transform.position) > targetDistance) {
				Vector3 direction = destination.transform.position - chessPiece.transform.position;
				direction.Normalize ();
				Vector3 movement = direction * Time.deltaTime * chessPiece.moveSpeed;

				chessPiece.transform.position += movement;
				yield return null;
			}

			anim.SetBool ("walking", false);
			chessPiece.currentTarget = null;	// Clear the current target
		}

		// Face the correct direction
		if (chessPiece.owner == 0) {
			chessPiece.transform.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.up);
		} else {
			chessPiece.transform.rotation = Quaternion.LookRotation (Vector3.forward * -1.0f, Vector3.up);
		}

		// Update variable for new piece location
		getSlot (chessPiece.currentX, chessPiece.currentY).currentPiece = null;
		chessPiece.currentX = destination.x;
		chessPiece.currentY = destination.y;
		destination.currentPiece = chessPiece;

		// Free up control
		movingCharacter = false;

		lastMoved = chessPiece;

		// Fire move complete event
		moveComplete.Invoke();

		yield return null;
	}

	// Return the chess slot at the coordinates
	public ChessSlot getSlot(int x, int y){
		// Out of range
		if (x < 0 || x > 7 || y < 0 || y > 7) {
			return null;
		}

		return chessSlots [x + 8 * y];
	}

	// Get the prefab for the piece for the player
	public ChessPiece getPlayerPiece(int player, ChessPiece.PieceType pieceType){
		switch(player){
		case 0:
			switch (pieceType) {
			case ChessPiece.PieceType.Pawn:
				return pawnOne;
			case ChessPiece.PieceType.Rook:
				return rookOne;
			case ChessPiece.PieceType.Bishop:
				return bishopOne;
			case ChessPiece.PieceType.Knight:
				return knightOne;
			case ChessPiece.PieceType.Queen:
				return queenOne;
			case ChessPiece.PieceType.King:
				return kingOne;
			}
			break;
		case 1:
			switch (pieceType) {
			case ChessPiece.PieceType.Pawn:
				return pawnTwo;
			case ChessPiece.PieceType.Rook:
				return rookTwo;
			case ChessPiece.PieceType.Bishop:
				return bishopTwo;
			case ChessPiece.PieceType.Knight:
				return knightTwo;
			case ChessPiece.PieceType.Queen:
				return queenTwo;
			case ChessPiece.PieceType.King:
				return kingTwo;
			}
			break;
		}

		return null;
	}

	// Remove the chess piece from the game
	private void chessPieceDeath(ChessPiece chessPiece){
		activePieces.Remove (chessPiece);
		destroyedPieces.Add (chessPiece);
		chessPiece.transform.position = new Vector3 (1000, 1000, 1000);
		chessPiece.gameObject.SetActive (false);

		pieceDestroyed.Invoke (chessPiece);
	}

	// Figure out if the player is in check
	public bool checkForCheck(int player){
		// Loop through all of the pieces
		foreach (ChessPiece chessPiece in activePieces) {
			// This is your piece, keep checking
			if (chessPiece.owner == player) {
				continue;
			}

			// See if your king can be attacked
			foreach (ChessSlot chessSlot in getPossibleDestinations(chessPiece)) {
				if (chessSlot.currentPiece == null) {
					continue;
				}
				if (chessSlot.currentPiece.pieceType == ChessPiece.PieceType.King && chessSlot.currentPiece.owner == player) {
					return true;
				}
			}
		}

		return false;
	}

	// Check if a castle move can be done this turn
	public bool canCastle(ChessPiece pieceOne, ChessPiece pieceTwo){
		// Empty piece, return false
		if (pieceOne == null || pieceTwo == null) {
			return false;
		}

		// Conditions
		bool bothPiecesSameOwner = pieceOne.owner == pieceTwo.owner;
		bool neitherPieceMoved = pieceOne.canCastle && pieceTwo.canCastle;
		bool kingAndRook = (pieceOne.pieceType == ChessPiece.PieceType.Rook && pieceTwo.pieceType == ChessPiece.PieceType.King) ||
		                   (pieceOne.pieceType == ChessPiece.PieceType.King && pieceTwo.pieceType == ChessPiece.PieceType.Rook);
		bool inCheck = checkForCheck (pieceOne.owner);
		// No sace between pieces
		// Check to the left
		int pieceY = pieceOne.currentY;
		if (pieceOne.currentX > pieceTwo.currentX) {
			int pieceX = pieceOne.currentX - 1;
			while (pieceX > pieceTwo.currentX) {
				if (getSlot (pieceX, pieceY).currentPiece != null) {
					return false;
				}
				pieceX -= 1;
			}
		}
		// Check to the right
		else {
			int pieceX = pieceOne.currentX + 1;
			while (pieceX < pieceTwo.currentX) {
				if (getSlot (pieceX, pieceY).currentPiece != null) {
					return false;
				}
				pieceX += 1;
			}
		}

		return bothPiecesSameOwner && neitherPieceMoved && kingAndRook && !inCheck;
	}

	// Perform the castle play for the pieces
	// ASSUMPTION: ONE IS A KING THE OTHER IS A ROOK
	public void castlePlay(ChessPiece pieceOne, ChessPiece pieceTwo){
		// Find the king and rook
		ChessPiece king = null;
		ChessPiece rook = null;
		if (pieceOne.pieceType == ChessPiece.PieceType.King) {
			king = pieceOne;
			rook = pieceTwo;
		} else {
			king = pieceTwo;
			rook = pieceOne;
		}

		int kingStartingX = king.currentX;

		// King moving left
		if (rook.currentX < king.currentX) {
			movePiece (king, getSlot (kingStartingX - 2, king.currentY));
			movePiece (rook, getSlot (kingStartingX - 1, king.currentY));
		} else {
			// King moving right
			movePiece (king, getSlot (kingStartingX + 2, king.currentY));
			movePiece (rook, getSlot (kingStartingX + 1, king.currentY));
		}

		moveComplete.Invoke ();
	}

	// Return a set of all of the possible slots that this piece can go
	public HashSet<ChessSlot> getPossibleDestinations(ChessPiece chessPiece){
		HashSet<ChessSlot> possibleDestinations = new HashSet<ChessSlot> ();

		ChessSlot destination;
		switch (chessPiece.pieceType) {
		case ChessPiece.PieceType.Pawn:
			// Different depending on player
			// Player 1
			if (chessPiece.owner == 0) {
				destination = getSlot (chessPiece.currentX, chessPiece.currentY + 1);
				if (destination != null) {
					bool slotOpen = destination.currentPiece == null;
					if (slotOpen) {
						possibleDestinations.Add (destination);
					}
				}

				// Starting move
				if (chessPiece.currentY == 1) {
					destination = getSlot (chessPiece.currentX, chessPiece.currentY + 2);
					if (destination != null) {
						bool firstSlotOpen = getSlot (chessPiece.currentX, chessPiece.currentY + 1).currentPiece == null;
						bool secondSlotOpen = destination.currentPiece == null;
						if (firstSlotOpen && secondSlotOpen) {
							possibleDestinations.Add (destination);
						}
					}
				}

				// Check for enemies on corners
				destination = getSlot(chessPiece.currentX - 1, chessPiece.currentY + 1);
				if (destination != null) {
					bool enemyPresent = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
					if (enemyPresent) {
						possibleDestinations.Add (destination);
					}
				}

				destination = getSlot (chessPiece.currentX + 1, chessPiece.currentY + 1);
				if (destination != null) {
					bool enemyPresent = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
					if (enemyPresent) {
						possibleDestinations.Add (destination);
					}
				}
			} 
			// Player 2
			else {
				destination = getSlot (chessPiece.currentX, chessPiece.currentY - 1);
				if (destination != null) {
					bool slotOpen = destination.currentPiece == null;
					if (slotOpen) {
						possibleDestinations.Add (destination);
					}
				}

				// Starting move
				if (chessPiece.currentY == 6) {
					destination = getSlot (chessPiece.currentX, chessPiece.currentY - 2);
					if (destination != null) {
						bool firstSlotOpen = getSlot (chessPiece.currentX, chessPiece.currentY - 1).currentPiece == null;
						bool secondSlotOpen = destination.currentPiece == null;
						if (firstSlotOpen && secondSlotOpen) {
							possibleDestinations.Add (destination);
						}
					}
				}

				// Check for enemies on corners
				destination = getSlot(chessPiece.currentX - 1, chessPiece.currentY - 1);
				if (destination != null) {
					bool enemyPresent = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
					if (enemyPresent) {
						possibleDestinations.Add (destination);
					}
				}

				destination = getSlot (chessPiece.currentX + 1, chessPiece.currentY - 1);
				if (destination != null) {
					bool enemyPresent = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
					if (enemyPresent) {
						possibleDestinations.Add (destination);
					}
				}
			}
			break;
		case ChessPiece.PieceType.Rook:
			// Left
			for(int i = chessPiece.currentX - 1; i >= 0; i --) {
				destination = getSlot (i, chessPiece.currentY);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}
			// Right
			for(int i = chessPiece.currentX + 1; i < 8; i ++) {
				destination = getSlot (i, chessPiece.currentY);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Up
			for(int i = chessPiece.currentY + 1; i < 8; i ++) {
				destination = getSlot (chessPiece.currentX, i);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Down
			for(int i = chessPiece.currentY - 1; i >= 0; i --) {
				destination = getSlot (chessPiece.currentX, i);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}
			break;
		case ChessPiece.PieceType.Knight:
			// U2L1
			destination = getSlot(chessPiece.currentX - 1, chessPiece.currentY + 2);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// U2R1
			destination = getSlot(chessPiece.currentX + 1, chessPiece.currentY + 2);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// U1L2
			destination = getSlot(chessPiece.currentX - 2, chessPiece.currentY + 1);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// U1R2
			destination = getSlot(chessPiece.currentX + 2, chessPiece.currentY + 1);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// D2L1
			destination = getSlot(chessPiece.currentX - 1, chessPiece.currentY - 2);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// D2R1
			destination = getSlot(chessPiece.currentX + 1, chessPiece.currentY - 2);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// D1L2
			destination = getSlot(chessPiece.currentX - 2, chessPiece.currentY - 1);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// D1R2
			destination = getSlot(chessPiece.currentX + 2, chessPiece.currentY - 1);
			if(destination != null){
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			break;
		case ChessPiece.PieceType.Bishop:
			// Top-Right
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX + i, chessPiece.currentY + i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Top-Left
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX - i, chessPiece.currentY + i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Bot-Right
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX + i, chessPiece.currentY - i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Bot-Left
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX - i, chessPiece.currentY - i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			break;
		case ChessPiece.PieceType.Queen:
			// ROOK MOVEMENTS
			// Left
			for(int i = chessPiece.currentX - 1; i >= 0; i --) {
				destination = getSlot (i, chessPiece.currentY);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}
			// Right
			for(int i = chessPiece.currentX + 1; i < 8; i ++) {
				destination = getSlot (i, chessPiece.currentY);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Up
			for(int i = chessPiece.currentY + 1; i < 8; i ++) {
				destination = getSlot (chessPiece.currentX, i);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Down
			for(int i = chessPiece.currentY - 1; i >= 0; i --) {
				destination = getSlot (chessPiece.currentX, i);
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;

				if (blockedByOwnPiece) {
					break;
				}

				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// BISHOP MOVEMENTS
			// Top-Right
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX + i, chessPiece.currentY + i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Top-Left
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX - i, chessPiece.currentY + i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Bot-Right
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX + i, chessPiece.currentY - i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			// Bot-Left
			for (int i = 1; i < 8; i++) {
				destination = getSlot (chessPiece.currentX - i, chessPiece.currentY - i);
				if (destination == null) {
					break;
				}

				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				bool blockedByEnemyPiece = destination.currentPiece != null && destination.currentPiece.owner != chessPiece.owner;
				if (blockedByOwnPiece) {
					break;
				}
				if (blockedByEnemyPiece) {
					possibleDestinations.Add (destination);
					break;
				}

				possibleDestinations.Add (destination);
			}

			break;
		case ChessPiece.PieceType.King:
			// Top
			destination = getSlot (chessPiece.currentX, chessPiece.currentY + 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// Bot
			destination = getSlot (chessPiece.currentX, chessPiece.currentY - 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// Left
			destination = getSlot (chessPiece.currentX - 1, chessPiece.currentY);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// Right
			destination = getSlot (chessPiece.currentX + 1, chessPiece.currentY);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// TL
			destination = getSlot (chessPiece.currentX - 1, chessPiece.currentY + 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// TR
			destination = getSlot (chessPiece.currentX + 1, chessPiece.currentY + 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// BL
			destination = getSlot (chessPiece.currentX - 1, chessPiece.currentY - 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}

			// BR
			destination = getSlot (chessPiece.currentX + 1, chessPiece.currentY - 1);
			if (destination != null) {
				bool blockedByOwnPiece = destination.currentPiece != null && destination.currentPiece.owner == chessPiece.owner;
				if (!blockedByOwnPiece) {
					possibleDestinations.Add (destination);
				}
			}
			break;
		}

		return possibleDestinations;
	}

}
