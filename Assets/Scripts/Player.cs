using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Player : MonoBehaviour {
	
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

	private	Rigidbody componentRigidbody;
	private	PlayerCamera componentCamera;

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

		if (Input.GetKeyDown (KeyCode.Escape)) SceneManager.LoadScene ("Menu");

	}

	void FixedUpdate() {

		if(!playerDead) PlayerMove (inputHorizontal, inputVertical);


	}

	void PlayerMove ( float h, float v ) {
		
		if (!helpmodeDisableMove) {
			
			playerMovment.Set (h + v, 0, v - h);
			playerMovment = playerMovment.normalized * playerSpeed * Time.deltaTime;

			componentRigidbody.MovePosition (transform.position + playerMovment);

		}

	}

	// MOUSE FUNCTION

	void MouseRotation () {

		RaycastHit rayHit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out rayHit, Mathf.Infinity, LayerMask.GetMask ("Enemy", "Map", "Box"))) {

			Vector3 mouse;
			GameObject collision = rayHit.collider.gameObject;

			if (autoAim && collision.CompareTag ("Enemy")) mousePosition = collision.transform.position;
			else mousePosition = rayHit.point;
			mouse = mousePosition - transform.position;
			mouse.y = 0f;

			componentRigidbody.MoveRotation (Quaternion.LookRotation (mouse));

			if (!collision.CompareTag ("BoxMystery")) textBox.text = ""; 
			else if (!helpmode) {

				BoxMystery box = collision.GetComponent<BoxMystery> ();
				if (!box.boxAmmo) {
						
					switch ((int)box.box) {

						case 0:
							textBox.text = "무기 구입 상자\n$" + box.boxCost.ToString () + " 가 필요합니다\n결정된 무기로 들고 있는 무기를 교체합니다"; 
							textBox.color = Color.yellow;
							break;
						case 1:
							textBox.text = "무기 데미지 증가 상자\n$" + box.boxCost.ToString () + " 가 필요합니다\n결정된 무기의 데미지를 증가시킵니다"; 
							textBox.color = new Color (1f, 1f / 2f, 0f);
							break;
						case 2:
							textBox.text = "무기 탄창 크기 증가 상자\n$" + box.boxCost.ToString () + " 가 필요합니다\n결정된 무기의 탄창을 늘려줍니다"; 
							textBox.color = new Color (0f, 3f / 4f, 1f);
							;
							break;
						case 3:
							textBox.text = "무기 레이저사이트 추가 상자\n$" + box.boxCost.ToString () + "가 필요합니다\n결정된 무기에 레이저사이트를 장착시킵니다"; 
							textBox.color = Color.red;
							break;

					}

				} else {

					textBox.text = "탄약 상자\n$" + box.boxCost.ToString () + " 가 필요합니다\n결정된 무기의 탄약을 추가합니다"; 
					textBox.color = Color.green;

				}

			}

		}

	}

	public void TextUpdate() {

		if (!helpmode) {
			
			textHealth.text = "HEALTH : " + playerHealth.ToString ();
			textPointTotal.text = "합 : $" + Mathf.Floor (playerPointTotal).ToString ();
			textPoint.text = "돈 : $" + Mathf.Floor (playerPoint).ToString ();

		}

	}

	public void ChangeHealth(float damage) {

		if (!playerDead) {

			playerHealth -= damage;
			playerHealth = Mathf.Clamp (playerHealth, 0f, 100f);
			screenRed.color = new Color( 1f, 0f, 0f, 0.25f - ( (playerHealth * 0.01f) * 0.25f ) );
			componentCamera.playerCameraShake += 2.5f * componentCamera.playerCameraShakeMultiply;
			playerAudio.PlayOneShot (playerSoundHit);
			TextUpdate ();

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
