﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// PLAYER INFO

	public float playerHealth;

	// MOVEMENT

	Vector3 playerMovment;

	public float playerSpeed;

	// INPUT

	float inputHorizontal;
	float inputVertical;

	// COMPONENT

	Rigidbody componentRigidbody;

	// CAMERA

	public GameObject playerCameraMain;

	//====================================================\\

	void Awake () {

		componentRigidbody = GetComponent<Rigidbody> ();


	}
	

	void Update () {



	}

	void FixedUpdate() {

		inputHorizontal = Input.GetAxisRaw ("Horizontal");
		inputVertical = Input.GetAxisRaw ("Vertical");

		PlayerMove (inputHorizontal, inputVertical);

		PlayerPoint ();

	}

	//====================================================\\

	void PlayerMove ( float h, float v ) {

		playerMovment.Set (h + v, 0, v - h);
		playerMovment = playerMovment.normalized * playerSpeed * Time.deltaTime;

		componentRigidbody.MovePosition (transform.position + playerMovment);

	}

	void PlayerPoint ( ) {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHit;

		float rayLenght = 500f;

		if( Physics.Raycast( ray, out rayHit, rayLenght ) ) {

			Vector3 mouse = rayHit.point - transform.position;
			mouse.y = 0f;

			Quaternion rotation = Quaternion.LookRotation( mouse );
			componentRigidbody.MoveRotation( rotation );

		}
	
	}

}
