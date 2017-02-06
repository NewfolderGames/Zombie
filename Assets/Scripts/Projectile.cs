using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public float damage;
	public float damageMultiply;

	public float knockback;
	public float knockbackMultiply;

	public float range;

	public float speed;

	void Start() {

		Destroy (gameObject, range);

	}

	void OnCollisionEnter( Collision other ) {

		if (other.gameObject.tag == "Map")
			Destroy (gameObject);
	}

}
