using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public static int enemyNumber;

	public GameObject player;
	public NavMeshAgent enemyNavigation;
	public Rigidbody enemyRigidbody;
	public SpawnerSystem spawner;

	public float enemyHealth;
	public float enemySpeed;

	public bool enemyKnockback;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start () {

		enemyNumber++;
		spawner.waveZombieNumberCurrent = enemyNumber;

		enemyNavigation = gameObject.GetComponent<NavMeshAgent> ();
		enemyRigidbody = gameObject.GetComponent<Rigidbody> ();
		enemyRigidbody.freezeRotation = true;

		enemyNavigation.speed = enemySpeed;

		player = GameObject.Find ("Player");

	}

	void Update () {

		if (!enemyKnockback)
			enemyNavigation.SetDestination (player.transform.position);
		else {

			enemyNavigation.Stop ();
			if (enemyRigidbody.velocity == Vector3.zero) {

				enemyNavigation.Resume ();
				enemyKnockback = false;

			}

		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void EnemyChangeHealth(float damage) {

		enemyHealth -= damage;
		if (enemyHealth <= 0) {
			
			Destroy (gameObject);
			enemyNumber--;
			spawner.waveZombieNumberCurrent = enemyNumber;

		}

	}

}
