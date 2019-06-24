using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class SummonedSkull : MonoBehaviour {

	public GameObject lightningBall_p;	// Prefab for the lightning ball created in front of Summoned Skull
	public LightningBoltScript lightningBolt_p;	// Prefab for shooting lightning bolts
	public Vector3 ballLocalLocation = new Vector3(0, 1, 1);
	public int numberOfLightingBolts = 5;

	private GameObject lightningBall;	// Used as reference for lightning bolt start location

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Spawn the lightning ball in front of the summoned skull
	public void CreateLightningBall(){
		lightningBall = Instantiate (lightningBall_p, transform.TransformPoint (ballLocalLocation), Quaternion.identity);
		Destroy (lightningBall, 3.0f);
	}

	// Attack effects for Summoned Skull
	public void ShootLightning(){
		for (int i = 0; i < numberOfLightingBolts; i++) {
			LightningBoltScript lightningBolt = Instantiate (lightningBolt_p);
			lightningBolt.StartObject = lightningBall;
			lightningBolt.EndObject = null;
			lightningBolt.EndPosition = GetComponent<ChessPiece> ().currentTarget.GetComponentInChildren<Renderer> ().bounds.center;
			Destroy (lightningBolt.gameObject, 2.0f);
		}
	}

}
