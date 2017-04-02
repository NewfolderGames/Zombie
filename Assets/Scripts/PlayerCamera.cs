using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public GameObject playerCamera;
	public Camera playerCameraMain;

	Vector3 playerCameraPosition;
	public float playerCameraOffsetX;
	public float playerCameraOffsetY;
	public float playerCameraOffsetZ;
	public float offsetX;
	public float offsetY;
	public float offsetZ;

	public float playerCameraShake = 0;
	public float playerCameraShakeLimit;
	public float playerCameraShakeMultiply;

	public float playerCameraSpeed;

	public float playerCameraZoom;

	public bool helpmode;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	// Use this for initialization
	void Awake () {

		if (!helpmode) {
			
			// CAMERA POSITION

			playerCameraPosition.x = transform.position.x + playerCameraOffsetX;
			playerCameraPosition.y = transform.position.y + playerCameraOffsetY;
			playerCameraPosition.z = transform.position.z + playerCameraOffsetZ;
			offsetX = playerCameraOffsetX;
			offsetY = playerCameraOffsetY;
			offsetZ = playerCameraOffsetZ;

			playerCamera.transform.position = playerCameraPosition;

			// CAMERA ZOOM

			playerCameraMain.orthographicSize = playerCameraZoom;

		}
		
	}

	void LateUpdate() {

		playerCameraShake = Mathf.Clamp (playerCameraShake, -playerCameraShake, playerCameraShakeLimit);

		playerCameraPosition.x = transform.position.x + playerCameraOffsetX + Random.Range(-playerCameraShake, playerCameraShake);
		playerCameraPosition.y = transform.position.y + playerCameraOffsetY + Random.Range(-playerCameraShake, playerCameraShake);
		playerCameraPosition.z = transform.position.z + playerCameraOffsetZ + Random.Range(-playerCameraShake, playerCameraShake);

		playerCamera.transform.position = Vector3.Lerp( playerCamera.transform.position, playerCameraPosition, playerCameraSpeed * Time.deltaTime );

		playerCameraShake = Mathf.Lerp (playerCameraShake, 0f, 0.1f);

	}

	public void View(bool top) {

		if (!helpmode) {

			if (top) {

				playerCameraOffsetX = 0f;
				playerCameraOffsetY = 20f;
				playerCameraOffsetZ = 0f;
				playerCameraPosition.x = transform.position.x + playerCameraOffsetX + Random.Range (-playerCameraShake, playerCameraShake);
				playerCameraPosition.y = transform.position.y + playerCameraOffsetY + Random.Range (-playerCameraShake, playerCameraShake);
				playerCameraPosition.z = transform.position.z + playerCameraOffsetZ + Random.Range (-playerCameraShake, playerCameraShake);
				playerCamera.transform.position = playerCameraPosition;
				playerCameraMain.transform.rotation = Quaternion.Euler (new Vector3 (90, 45, 0));

			} else {

				playerCameraOffsetX = offsetX;
				playerCameraOffsetY = offsetY;
				playerCameraOffsetZ = offsetZ;
				playerCameraPosition.x = transform.position.x + playerCameraOffsetX + Random.Range (-playerCameraShake, playerCameraShake);
				playerCameraPosition.y = transform.position.y + playerCameraOffsetY + Random.Range (-playerCameraShake, playerCameraShake);
				playerCameraPosition.z = transform.position.z + playerCameraOffsetZ + Random.Range (-playerCameraShake, playerCameraShake);
				playerCamera.transform.position = playerCameraPosition;
				playerCameraMain.transform.rotation = Quaternion.Euler (new Vector3 (30, 45, 0));

			}

		}

	}
		
}
