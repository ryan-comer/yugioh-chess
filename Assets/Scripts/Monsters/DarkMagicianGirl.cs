using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMagicianGirl : MonoBehaviour {

	public GameObject magicBall_p;
	public GameObject explosion_p;
	public Transform staffTip;	// Used to start magic spells

	public float ballMoveSpeed = 1.0f;

	private GameObject magicBall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Create magical ball for dark magician girl
	public void CreateMagicBall(){
		magicBall = Instantiate (magicBall_p, staffTip);
		magicBall.transform.localPosition = Vector3.zero;
	}

	// Send the magic ball towards the target
	public void FireMagicBall(){
		StartCoroutine (moveBallCoroutine ());
	}

	private IEnumerator moveBallCoroutine(){
		float threshold = 0.05f;

		// Get the target and the center
		ChessPiece target = GetComponent<ChessPiece>().currentTarget;
		Vector3 targetCenter = target.GetComponentInChildren<Renderer>().bounds.center;

		// Clear parent
		magicBall.transform.parent = null;

		// Move towards the target
		while (Vector3.Distance (targetCenter, magicBall.transform.position) > threshold) {
			Vector3 moveDirection = (targetCenter - magicBall.transform.position);
			moveDirection.Normalize ();

			magicBall.transform.position += (moveDirection * Time.deltaTime * ballMoveSpeed);
			yield return null;
		}

		// Hit the target
		GameObject explosion = Instantiate (explosion_p, magicBall.transform.position, Quaternion.identity);
		Destroy(magicBall);
		Destroy (explosion, 5.0f);

		target.GetComponent<Animator> ().SetTrigger ("death");
	}

}
