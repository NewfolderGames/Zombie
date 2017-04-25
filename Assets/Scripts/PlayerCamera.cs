using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public GameObject playerCamera;
	public Camera playerCameraMain;

	public Vector3 playerCameraPosition;
	public Vector3 playerCameraOffset;
	public Vector3 shakeOffset;
	public Vector3 offset;

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

			playerCameraPosition = transform.position + playerCameraOffset;
			offset = playerCameraOffset;
			playerCamera.transform.position = playerCameraPosition;
			playerCameraMain.orthographicSize = playerCameraZoom;

		}
		
	}

	void LateUpdate() {

		playerCameraShake = Mathf.Clamp (playerCameraShake, -playerCameraShake, playerCameraShakeLimit);
		shakeOffset.Set (Random.Range (-playerCameraShake, playerCameraShake), Random.Range (-playerCameraShake, playerCameraShake), Random.Range (-playerCameraShake, playerCameraShake));
		playerCameraPosition = transform.position + playerCameraOffset + shakeOffset;
		playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, playerCameraPosition, playerCameraSpeed * Time.deltaTime);
		playerCameraShake = Mathf.Lerp (playerCameraShake, 0f, 0.1f);

	}

	public void View(bool top) {

		if (!helpmode) {

			if (top) {

				playerCameraOffset.Set (0f, 20f, 0f);
				playerCameraPosition = transform.position + playerCameraOffset + shakeOffset;
				playerCamera.transform.position = playerCameraPosition;
				playerCameraMain.transform.rotation = Quaternion.Euler (new Vector3 (90, 45, 0));

			} else {

				playerCameraOffset = offset;
				playerCameraPosition = transform.position + playerCameraOffset + shakeOffset;
				playerCamera.transform.position = playerCameraPosition;
				playerCameraMain.transform.rotation = Quaternion.Euler (new Vector3 (30, 45, 0));

			}

		}

	}
		
}
