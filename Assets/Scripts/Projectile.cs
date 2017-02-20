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

	public bool explosive;
	public GameObject explosion;

	void Start() {

		gameObject.GetComponent<Rigidbody>().freezeRotation = true;
		knockback *= knockbackMultiply * 25f;
		damage *= damageMultiply;
		Destroy (gameObject, 5f);

	}

	void OnCollisionEnter( Collision other ) {

		if (other.gameObject.tag == "Map") {

			if (!explosive)
				Destroy (gameObject);
			else {

				GameObject exp = Instantiate (explosion, transform.position, transform.rotation);
				ProjectileExplosion expInfo = exp.GetComponent<ProjectileExplosion> ();

				expInfo.damage = damage;
				expInfo.knockback = knockback;
				expInfo.range = range;

				Destroy (gameObject);

			}

		}

		if (other.gameObject.tag == "Enemy") {

			if (!explosive) {
				
				Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody> ();
				Zombie otherZombie = other.gameObject.GetComponent<Zombie> ();

				if (!otherZombie.enemyDead) {
				
					otherZombie.damageExplosive = false;
					otherZombie.EnemyChangeHealth (damage);
					otherZombie.enemyKnockback = true;
					otherRigidbody.AddForce (transform.rotation * Vector3.forward * knockback * otherZombie.enemyKnockbackMulti, ForceMode.Impulse);

				}
				Destroy (gameObject);

			} else {

				GameObject exp = Instantiate (explosion, transform.position, transform.rotation);
				ProjectileExplosion expInfo = exp.GetComponent<ProjectileExplosion> ();

				expInfo.damage = damage;
				expInfo.knockback = knockback;
				expInfo.range = range;

				Destroy (gameObject);

			}

		}
		
	}

}
