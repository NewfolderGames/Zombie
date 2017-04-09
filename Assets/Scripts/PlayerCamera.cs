using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public GameObject playerCamera;
	public Camera playerCameraMain;

	Vector3 playerCameraPosition;
	public Vector3 playerCamQuaOffset;
	public Vector3 playerCamTopOffset; // new Vector3(0f, 20f, 0f);

	public float playerCameraShake = 0;
	public float playerCameraShakeLimit;
	public float playerCameraShakeMultiply;

	public float playerCameraSpeed;

	public float playerCameraZoom;

	public bool helpmode;

	Quaternion topQ = Quaternion.Euler (new Vector3 (90, 45, 0));
	Quaternion quaQ = Quaternion.Euler (new Vector3 (30, 45, 0));
	bool viewMode = false;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void LateUpdate() {
		Vector3 pos = viewMode ? playerCamTopOffset : playerCamQuaOffset;
		playerCameraShake = Mathf.Clamp (playerCameraShake, -playerCameraShake, playerCameraShakeLimit);
		playerCameraPosition = transform.position + pos * Random.Range(-playerCameraShake, playerCameraShake);
		playerCamera.transform.position = Vector3.Lerp( playerCamera.transform.position, playerCameraPosition, playerCameraSpeed * Time.deltaTime);

		playerCameraShake = Mathf.Lerp (playerCameraShake, 0f, 0.1f);

		playerCameraMain.transform.rotation = viewMode? Quaternion.Slerp (playerCameraMain.transform.rotation, topQ, 5f * Time.deltaTime) : Quaternion.Slerp (playerCameraMain.transform.rotation, quaQ, 5f * Time.deltaTime);

	}

	public void View(bool top) {
		if (!helpmode) {
			viewMode = top;
			// playerCamera.transform.position = playerCameraPosition;
		}
	}
}
