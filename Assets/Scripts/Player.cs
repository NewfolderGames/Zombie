﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// PLAYER INFO

	public float playerHealth = 100f;

	public float playerPoint = 0f;
	public float playerPointTotal = 0f;

	// MOVEMENT

	Vector3 playerMovment;

	public float playerSpeed;

	// INPUT

	float inputHorizontal;
	float inputVertical;

	public Vector3 mousePosition;
	public Quaternion mouseRotation;

	// COMPONENT

	Rigidbody componentRigidbody;

	// CAMERA

	public GameObject playerCameraMain;

	// TEXT

	public Text textHealth;
	public Text textPoint;
	public Text textPointTotal;

	// ========== ========== ========== CONSTRUCTOR ========== ========== ========== \\

	void Awake () {

		componentRigidbody = GetComponent<Rigidbody> ();

		componentRigidbody.freezeRotation = true;

	}

	void Start() {

		TextUpdate ();

	}

	void Update () {



	}

	void FixedUpdate() {

		inputHorizontal = Input.GetAxisRaw ("Horizontal");
		inputVertical = Input.GetAxisRaw ("Vertical");

		PlayerMove (inputHorizontal, inputVertical);

		MouseRotation ();


	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	void PlayerMove ( float h, float v ) {

		playerMovment.Set (h + v, 0, v - h);
		playerMovment = playerMovment.normalized * playerSpeed * Time.deltaTime;

		componentRigidbody.MovePosition (transform.position + playerMovment);

	}

	// MOUSE FUNCTION

	void MouseRotation () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHit;

		float rayLenght = 500f;

		int layerMask = LayerMask.GetMask ("Map", "Enemy");

		if( Physics.Raycast( ray, out rayHit, rayLenght, layerMask ) ) {

			Vector3 mouse = rayHit.point - transform.position;
			mouse.y = 0f;

			Quaternion rotation = Quaternion.LookRotation( mouse );

			mousePosition = rayHit.point;
			mouseRotation = rotation;
			componentRigidbody.MoveRotation (rotation);

		}
	
	}

	public void TextUpdate() {

		textHealth.text = "HEALTH : " + playerHealth.ToString ();
		textPointTotal.text = playerPointTotal.ToString ();
		textPoint.text = playerPoint.ToString ();

	}

}
