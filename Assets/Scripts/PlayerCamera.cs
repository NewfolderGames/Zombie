using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// CAMERA

	public GameObject playerCamera;
	public Camera playerCameraMain;

	Vector3 playerCameraPosition;
	public float playerCameraOffsetX;
	public float playerCameraOffsetY;
	public float playerCameraOffsetZ;

	public float playerCameraShake = 0;
	public float playerCameraShakeLimit;
	public float playerCameraShakeMultiply;

	public float playerCameraSpeed;

	public float playerCameraZoom;

	//====================================================\\

	// Use this for initialization
	void Start () {

		// CAMERA POSITION

		playerCameraPosition.x = transform.position.x + playerCameraOffsetX;
		playerCameraPosition.y = transform.position.y + playerCameraOffsetY;
		playerCameraPosition.z = transform.position.z + playerCameraOffsetZ;

		playerCamera.transform.position = playerCameraPosition;

		// CAMERA ZOOM

		playerCameraMain.orthographicSize = playerCameraZoom;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate() {

		playerCameraShake = Mathf.Clamp (playerCameraShake, -playerCameraShake, playerCameraShakeLimit);

		playerCameraPosition.x = transform.position.x + playerCameraOffsetX + Random.Range(-playerCameraShake, playerCameraShake);
		playerCameraPosition.y = transform.position.y + playerCameraOffsetY + Random.Range(-playerCameraShake, playerCameraShake);
		playerCameraPosition.z = transform.position.z + playerCameraOffsetZ + Random.Range(-playerCameraShake, playerCameraShake);

		playerCamera.transform.position = Vector3.Lerp( playerCamera.transform.position, playerCameraPosition, playerCameraSpeed * Time.deltaTime );

		playerCameraShake = Mathf.Lerp (playerCameraShake, 0f, 0.1f);

	}

	//====================================================\\



}
