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

		gameObject.GetComponent<Rigidbody>().freezeRotation = true;
		knockback *= knockbackMultiply * 25f;
		damage *= damageMultiply;
		if (range != -1)
			Destroy (gameObject, range);
		else
			Destroy (gameObject, Time.deltaTime * 2);

	}

	void OnCollisionEnter( Collision other ) {

		if (other.gameObject.tag == "Map")
			Destroy (gameObject);
		
		if (other.gameObject.tag == "Enemy") {
			
			Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody> ();
			Zombie otherZombie = other.gameObject.GetComponent<Zombie> ();

			if (!otherZombie.enemyDead) {
				
				otherZombie.EnemyChangeHealth (damage);
				otherZombie.enemyKnockback = true;
				otherRigidbody.AddForce (transform.rotation * Vector3.forward * knockback, ForceMode.Impulse);

			}
			Destroy (gameObject);

		}
		
	}

}
