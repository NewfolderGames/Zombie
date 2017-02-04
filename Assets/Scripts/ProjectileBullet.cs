using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour {

	public float damage;
	public float range;

	public Quaternion target;

	void start() {

		Debug.Log ("123123");
		Destroy (gameObject,range);

	}

}
