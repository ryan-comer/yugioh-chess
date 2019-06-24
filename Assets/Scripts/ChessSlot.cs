using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessSlot : MonoBehaviour {

	public int x, y;	// The coordinates for this slot

	public ChessPiece currentPiece;	// The piece that is currently on the slot

	public SlotClicked slotClicked;

	[System.Serializable]
	public class SlotClicked : UnityEvent<ChessSlot>{

	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void highlight(){
		GetComponent<MeshRenderer> ().enabled = true;
	}

	public void unHighlight(){
		GetComponent<MeshRenderer> ().enabled = false;
	}

	// Used to check when a user clicks a slot
	void OnMouseOver(){
		checkMouseClick ();
	}

	// See if the user clicks on the object
	private void checkMouseClick(){
		if (Input.GetMouseButtonDown (0)) {
			slotClicked.Invoke (this);
		}
	}

}
