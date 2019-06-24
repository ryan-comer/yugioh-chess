using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform stage;	// This is what the camera focuses on
	public float moveSpeed = 1f;	// Speed of moving the camera
	public float scrollSpeed = 1f;	// Speed of zooming camera

	// Bounds for rotation
	public float minPitch = -40f;
	public float maxPitch = 40f;

	// Bounds for scrolling
	public float minScroll = 5f;
	public float maxScroll = 20f;

	private float distanceFromStage = 10f;	// Used to position the camera
	private Camera cam;	// Camera for this controller

	// Struct to hold input for the camera
	private struct InputStruct{
		public float moveX, moveY, scroll;

		public InputStruct(float moveX, float moveY, float scroll){
			this.moveX = moveX;
			this.moveY = moveY;
			this.scroll = scroll;
		}
	}

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		initializeCameraPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		InputStruct input = getInput ();
		updateCamera (input);
	}

	// Get the input for this frame
	private InputStruct getInput(){
		// Check for camera movement
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		moveX *= moveSpeed * Time.deltaTime * -1f;
		moveY *= moveSpeed * Time.deltaTime;

		// Check for scroll
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		scroll *= scrollSpeed * Time.deltaTime * -1f;

		return new InputStruct (moveX, moveY, scroll);
	}

	// Update the camera based on the input
	private void updateCamera(InputStruct input){
		float camEulerX = cam.transform.rotation.eulerAngles.x;
		float newMin = minPitch;
		float newMax = maxPitch;
		if(camEulerX > 180){
			newMin += 360;	// Because rotations in Unity are weird
			newMax += 360;
		}

		// Rotate the camera
		// Check the bounds
		bool insideBounds = (camEulerX > newMin && camEulerX < newMax);
		bool canGoUp = camEulerX < newMin && input.moveY > 0;
		bool canGoDown = camEulerX > newMax && input.moveY < 0;
		if (insideBounds || canGoUp || canGoDown) {
			cam.transform.RotateAround (stage.position, cam.transform.right, input.moveY);
		}
		cam.transform.RotateAround (stage.position, cam.transform.up, input.moveX);

		// Camera scrolling
		distanceFromStage += input.scroll;
		distanceFromStage = Mathf.Clamp (distanceFromStage, minScroll, maxScroll);

		Vector3 movementVector = cam.transform.position - stage.position;
		movementVector.Normalize ();
		movementVector *= distanceFromStage;

		cam.transform.position = stage.transform.position + movementVector;

		// Look at the stage
		cam.transform.LookAt(stage.position);
	}

	// Set the starting position/orientation of the camera
	private void initializeCameraPosition(){
		// Set the camera position
		Vector3 targetPosition = stage.position + (new Vector3 (1f, 1f, 1f) * distanceFromStage);
		cam.transform.position = targetPosition;

		// Look at the stage
		cam.transform.LookAt(stage.position);
	}

	// accepts e.g. -80, 80
	private float ClampAngle(float angle, float from, float to)
	{
		if (angle < 0f) angle = 360 + angle;
		if (angle > 180f) return Mathf.Max(angle, 360+from);
		return Mathf.Min(angle, to);
	}

}
