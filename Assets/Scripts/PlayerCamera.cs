﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// CAMERA

	public GameObject playerCamera;

	Vector3 playerCameraPosition;
	public float PlayerCameraOffsetX;
	public float PlayerCameraOffsetY;
	public float PlayerCameraOffsetZ;

	public float playerCameraSpeed;

	//====================================================\\

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate() {

		playerCameraPosition.x = transform.position.x + PlayerCameraOffsetX;
		playerCameraPosition.y = transform.position.y + PlayerCameraOffsetY;
		playerCameraPosition.z = transform.position.z + PlayerCameraOffsetZ;

		playerCamera.transform.position = Vector3.Lerp( playerCamera.transform.position, playerCameraPosition, playerCameraSpeed * Time.deltaTime );

	}

	//====================================================\\



}