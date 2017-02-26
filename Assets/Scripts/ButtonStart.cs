using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStart : MonoBehaviour {

	public string map;

	public void StartGame() {
		SceneManager.LoadScene (map);
	}

}
