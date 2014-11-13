using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	const float ZERO = 0f;
	const float POSITIVE = 1f;
	const float NEGETIVE = -1f;

	public float magnitude, mouseScroll;
	public float cameraMoveSpeed = 200f;
	public float cameraZoomSpeed = 10f;
	public float velocity = ZERO;

	public float terrainWidth;
	public float terrainHeight;

	float maxSpeed, minSpeed;

	public Vector3 direction = Vector3.zero;

	Terrain terrain;

	void Start() {
		maxSpeed = cameraMoveSpeed;
		minSpeed = ZERO;

		terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
		Vector3 terrainSize = terrain.terrainData.size;

		terrainWidth = terrainSize.x;
		terrainHeight = terrainSize.z;
	}

	// Update is called once per frame
	void Update () {
		KeyboardMovements ();	
	}

	void KeyboardMovements() 
	{
		cameraMoveSpeed = -maxSpeed;

		if (Input.GetKey(KeyCode.W)) {
			direction += new Vector3(ZERO, ZERO, POSITIVE);
			cameraMoveSpeed = maxSpeed;
		}

		if (Input.GetKey(KeyCode.S)) {
			direction += new Vector3(ZERO, ZERO, NEGETIVE);
			cameraMoveSpeed = maxSpeed;
		}

		if (Input.GetKey(KeyCode.D)) {
			direction += new Vector3(POSITIVE, ZERO, ZERO);
			cameraMoveSpeed = maxSpeed;
		}

		if (Input.GetKey(KeyCode.A)) {
			direction += new Vector3(NEGETIVE, ZERO, ZERO);
			cameraMoveSpeed = maxSpeed;
		}

		mouseScroll = Input.GetAxis("Mouse ScrollWheel");

		if (mouseScroll > ZERO) {
			direction += new Vector3(ZERO, NEGETIVE, ZERO);
			velocity = cameraZoomSpeed;
		}

		if (mouseScroll < ZERO) {
			direction += new Vector3(ZERO, POSITIVE, ZERO);
			velocity = cameraZoomSpeed;
		}

		magnitude = direction.sqrMagnitude;

		if (magnitude > 2) { 
			direction.Normalize(); 
		}

		magnitude = direction.sqrMagnitude;

		CalculateSpeed();
		CheckBoundaries();
		MoveCamera();
	}

	void CalculateSpeed() {
		velocity += cameraMoveSpeed * Time.deltaTime;
	}

	void CheckBoundaries() {
		Vector3 cameraPosition = transform.position;

		if (velocity < minSpeed) {
			velocity = minSpeed;
			direction = Vector3.zero;
		}
		if (velocity > maxSpeed) velocity = maxSpeed;

		if (cameraPosition.y < 35) {
			cameraPosition.y = 35;
			transform.position = cameraPosition;
		}
		if (cameraPosition.y > 200) {
			cameraPosition.y = 200;
			transform.position = cameraPosition;
		}

		if (cameraPosition.z <= 0) {
			cameraPosition.z = 0;
			transform.position = cameraPosition;
		}
		if (cameraPosition.z >= terrainHeight) {
			cameraPosition.z = terrainHeight;
			transform.position = cameraPosition;
		}

		if (cameraPosition.x <= 0) {
			cameraPosition.x = 0;
			transform.position = cameraPosition;
		}
		if (cameraPosition.x >= terrainWidth) {
			cameraPosition.x = terrainWidth;
			transform.position = cameraPosition;
		}
	}

	void MoveCamera() 
	{
		transform.Translate(direction * velocity * Time.deltaTime, Space.World);
	}
}
