using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour {

	public bool help;
	public static bool helpRead;
	public string scene;
	Image image;

	void Awake() {

		image = GetComponent<Image> ();
		if (!help && !helpRead) image.color = Color.gray;

	}

	public void StartGame() {

		if (helpRead || help) {
			
			SceneManager.LoadScene (scene);
			helpRead = true;

		}

	}

}
