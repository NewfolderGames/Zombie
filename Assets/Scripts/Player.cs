using System.Collections;
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

	}

	//====================================================\\

	void PlayerMove ( float h, float v ) {

		playerMovment.Set (h + v, 0, v - h);
		playerMovment = playerMovment.normalized * playerSpeed * Time.deltaTime;

		componentRigidbody.MovePosition (transform.position + playerMovment);

	}

}
