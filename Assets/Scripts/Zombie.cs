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
	public SpawnerSystem spawnerSystem;

	public float enemyHealth;
	public float enemySpeed;
	public bool enemyDead;

	public bool enemyKnockback;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start () {

		enemyNumber++;
		spawnerSystem = GameObject.Find ("Spawner_System").GetComponent<SpawnerSystem> ();
		spawnerSystem.waveZombieNumberCurrent = enemyNumber;

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
		Player playerInfo = player.GetComponent<Player> ();
		playerInfo.playerPoint += 10f;
		if (enemyHealth <= 0 && !enemyDead) {
			
			enemyNumber--;
			enemyDead = true;
			spawnerSystem.waveZombieNumberCurrent = enemyNumber;
			playerInfo.playerPoint += 90f;
			Destroy (gameObject);

		}
		playerInfo.TextUpdate ();

	}

}
