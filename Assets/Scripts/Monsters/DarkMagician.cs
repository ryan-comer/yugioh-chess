using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMagician : MonoBehaviour {

	public GameObject magicBall_p;
	public GameObject energyWave_p;
	public Transform staffTip;	// Used to start magic spells

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Create dark enery ball on staff
	public void CreateMagicBall(){
		GameObject magicBall = Instantiate (magicBall_p, staffTip);
		magicBall.transform.localPosition = Vector3.zero;

		Destroy (magicBall, 7);
	}

	// Shoot energy wave at target
	public void ShootEnergyWave(){
		GameObject energyWave = Instantiate (energyWave_p, staffTip);
		energyWave.transform.localPosition = Vector3.zero;
		energyWave.transform.LookAt (GetComponent<ChessPiece> ().currentTarget.GetComponentInChildren<Renderer>().bounds.center);

		Destroy (energyWave, 3f);
	}

}
