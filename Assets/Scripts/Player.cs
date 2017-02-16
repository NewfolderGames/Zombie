using System.Collections;
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

	public bool autoAim;

	// COMPONENT

	Rigidbody componentRigidbody;

	// CAMERA

	public GameObject playerCameraMain;

	// TEXT

	public Text textHealth;
	public Text textPoint;
	public Text textPointTotal;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake () {

		componentRigidbody = GetComponent<Rigidbody> ();

		componentRigidbody.freezeRotation = true;

	}

	void Start() {

		TextUpdate ();

	}

	void Update () {

		inputHorizontal = Input.GetAxisRaw ("Horizontal");
		inputVertical = Input.GetAxisRaw ("Vertical");

		MouseRotation ();

	}

	void FixedUpdate() {

		PlayerMove (inputHorizontal, inputVertical);

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

		if (autoAim) {
			
			int enemyMask = LayerMask.GetMask ("Enemy");
			int mapMask = LayerMask.GetMask ("Map");

			if (Physics.Raycast (ray, out rayHit, rayLenght, enemyMask)) {
				
				GameObject enemy = rayHit.collider.gameObject;
				Vector3 mouse = enemy.transform.position - transform.position;
				mouse.y = 0f;

				Quaternion rotation = Quaternion.LookRotation (mouse);

				mousePosition = enemy.transform.position;
				mouseRotation = rotation;
				componentRigidbody.MoveRotation (rotation);

			} else if (Physics.Raycast (ray, out rayHit, rayLenght, mapMask)) {

				Vector3 mouse = rayHit.point - transform.position;
				mouse.y = 0f;

				Quaternion rotation = Quaternion.LookRotation (mouse);

				mousePosition = rayHit.point;
				mouseRotation = rotation;
				componentRigidbody.MoveRotation (rotation);

			}

		} else {

			int mask = LayerMask.GetMask ("Enemy","Map");

			if (Physics.Raycast (ray, out rayHit, rayLenght, mask)) {

				Vector3 mouse = rayHit.point - transform.position;
				mouse.y = 0f;

				Quaternion rotation = Quaternion.LookRotation (mouse);

				mousePosition = rayHit.point;
				mouseRotation = rotation;
				componentRigidbody.MoveRotation (rotation);

			}

		}

	
	}

	public void TextUpdate() {

		textHealth.text = "HEALTH : " + playerHealth.ToString ();
		textPointTotal.text = playerPointTotal.ToString ();
		textPoint.text = playerPoint.ToString ();

	}

}
