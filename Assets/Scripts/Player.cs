using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Player : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// PLAYER INFO

	public float playerHealth = 100f;
	public bool playerDead;

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
	PlayerCamera componentCamera;

	// CAMERA

	public GameObject playerCameraMain;

	// UI

	public Text textHealth;
	public Text textPoint;
	public Text textPointTotal;
	public Text textBox;
	public Text textGameover;
	public Image screenRed;

	//

	public int[] boxGet;

	//

	public PlayerEquip playerInfo;

	// sound

	public AudioSource playerAudio;
	public AudioClip playerSoundHit;

	// ect

	public bool helpmode;
	public bool helpmodeDisableMove;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake () {

		componentRigidbody = GetComponent<Rigidbody> ();
		componentCamera = GetComponent<PlayerCamera> ();

		componentRigidbody.freezeRotation = true;

		boxGet = new int[4];
	}

	void Start() {

		TextUpdate ();

	}

	void Update () {

		inputHorizontal = Input.GetAxisRaw ("Horizontal");
		inputVertical = Input.GetAxisRaw ("Vertical");

		if (!playerDead)
			MouseRotation ();
		else {

			screenRed.color = Color.Lerp (screenRed.color, Color.black, Time.deltaTime);
			textGameover.color = Color.Lerp (textGameover.color, Color.white, Time.deltaTime);

		}

	}

	void FixedUpdate() {

		if(!playerDead) PlayerMove (inputHorizontal, inputVertical);

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	void PlayerMove ( float h, float v ) {
		
		if (!helpmodeDisableMove) {
			
			playerMovment.Set (h + v, 0, v - h);
			playerMovment = playerMovment.normalized * playerSpeed * Time.deltaTime;

			componentRigidbody.MovePosition (transform.position + playerMovment);

		}

	}

	// MOUSE FUNCTION

	void MouseRotation () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHit;

		float rayLenght = 500f;

		int mask = LayerMask.GetMask ("Enemy","Map","Box");

		if (Physics.Raycast (ray, out rayHit, rayLenght, mask)) {

			GameObject collision = rayHit.collider.gameObject;
			if (autoAim && collision.tag == "Enemy" ) {

				Vector3 mouse = collision.transform.position - transform.position;
				mouse.y = 0f;

				Quaternion rotation = Quaternion.LookRotation (mouse);

				mousePosition = collision.transform.position;
				mouseRotation = rotation;
				componentRigidbody.MoveRotation (rotation);

			} else {
					
				Vector3 mouse = rayHit.point - transform.position;
				mouse.y = 0f;

				Quaternion rotation = Quaternion.LookRotation (mouse);

				mousePosition = rayHit.point;
				mouseRotation = rotation;
				componentRigidbody.MoveRotation (rotation);

				if (collision.tag == "BoxMystery" && !helpmode) {

					BoxMystery box = collision.GetComponent<BoxMystery> ();
					if (!box.boxAmmo) {
						
						switch ((int)box.box) {

						case 0:
							textBox.text = "WEAPON BOX\n" + box.boxCost.ToString () + " POINT"; 
							textBox.color = Color.yellow;
							break;
						case 1:
							textBox.text = "DAMAGE INCREASE BOX\n" + box.boxCost.ToString () + " POINT"; 
							textBox.color = new Color (1f, 1f / 2f, 0f);
							break;
						case 2:
							textBox.text = "CLIP EXTEND BOX\n" + box.boxCost.ToString () + " POINT"; 
							textBox.color = new Color (0f, 3f / 4f, 1f);;
							break;
						case 3:
							textBox.text = "LASER SIGHT BOX\n" + box.boxCost.ToString () + " POINT"; 
							textBox.color = Color.red;
							break;

						}

					} else {

						textBox.text = "AMMO BOX\n" + box.boxCost.ToString () + " POINT"; 
						textBox.color = Color.green;

					}

				} else
					
					if(!helpmode) textBox.text = "";

			}

		}
			
	}

	public void TextUpdate() {

		if (!helpmode) {
			
			textHealth.text = "HEALTH : " + playerHealth.ToString ();
			textPointTotal.text = Mathf.Floor (playerPointTotal).ToString ();
			textPoint.text = Mathf.Floor (playerPoint).ToString ();

		}

	}

	public void ChangeHealth(float damage) {

		if (!playerDead) {

			playerHealth -= damage;
			playerHealth = Mathf.Clamp (playerHealth, 0f, 100f);
			screenRed.color = new Color( 1f, 0f, 0f, ( 1f / 4f ) - ( (playerHealth / 100f) / 4f ) );
			componentCamera.playerCameraShake += 2.5f * componentCamera.playerCameraShakeMultiply;
			TextUpdate ();
			playerAudio.PlayOneShot (playerSoundHit);
			if (playerHealth <= 0) {

				playerDead = true;
				playerInfo.playerDead = true;
				StartCoroutine (Gameover());

			}

		}

	}

	IEnumerator Gameover() {

		yield return new WaitForSeconds (5f);
		SceneManager.LoadScene ("Menu");

	}

}
