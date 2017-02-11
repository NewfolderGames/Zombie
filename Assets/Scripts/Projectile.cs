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

		knockback *= knockbackMultiply * 25f;
		damage *= damageMultiply;
		Destroy (gameObject, range);

	}

	void OnCollisionEnter( Collision other ) {

		if (other.gameObject.tag == "Map")
			Destroy (gameObject);
		
		if (other.gameObject.tag == "Enemy") {
			
			Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody> ();
			Zombie otherZombie = other.gameObject.GetComponent<Zombie> ();

			otherZombie.EnemyChangeHealth (damage);
			otherZombie.enemyKnockback = true;
			otherZombie.enemyNavigation.Stop ();
			otherRigidbody.AddForce(transform.rotation * Vector3.forward * knockback, ForceMode.Impulse);

			Destroy (gameObject);

		}
		
	}

}
