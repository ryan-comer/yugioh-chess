using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectPawnPiece : MonoBehaviour, IPointerClickHandler {

	public CardSelected cardSelected;
	public ChessPiece chessPiece_p;	// The chessPiece that this selector will spawn

	[System.Serializable]
	public class CardSelected : UnityEvent<SelectPawnPiece>{}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Clicked on the card
	public void OnPointerClick(PointerEventData eventData){
		Debug.Log (cardSelected);
		cardSelected.Invoke (this);
	}

}
