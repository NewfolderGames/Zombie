using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public GameObject player;
	public NavMeshAgent enemyNavigation;
	public Rigidbody enemyRigidbody;
	public SpawnerSystem spawnerSystem;

	public float enemyHealth;
	public float enemySpeed;
	public float enemyDamage;
	public bool enemyDead;

	public bool enemyKnockback;

	public bool enemyAttack;
	public float enemyAttackTime;
	public float enemyAttackRange;
	public bool enemyAttackAvailable = true;

	public bool damageExplosive = false;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start () {

		spawnerSystem = GameObject.Find ("Spawner_System").GetComponent<SpawnerSystem> ();

		enemyNavigation = gameObject.GetComponent<NavMeshAgent> ();
		enemyRigidbody = gameObject.GetComponent<Rigidbody> ();
		enemyRigidbody.freezeRotation = true;

		enemyNavigation.speed = enemySpeed;

		player = GameObject.Find ("Player");

	}

	void Update () {

		if (!enemyKnockback) {

			if (Vector3.Distance (player.transform.position, transform.position) <= enemyAttackRange) {

				if (enemyAttackAvailable && !enemyAttack) {

					enemyAttack = true;
					enemyAttackAvailable = false;
					StartCoroutine (AttackWait (enemyAttackTime));

				}

			} else {

				enemyNavigation.Resume ();
				enemyNavigation.SetDestination (player.transform.position);

			}

		}
		else {

			enemyAttack = false;
			enemyNavigation.Stop ();
			if (enemyRigidbody.velocity == Vector3.zero) {

				enemyNavigation.Resume ();
				enemyKnockback = false;

			}

		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void EnemyChangeHealth(float damage) {

		if (!enemyDead) {
			
			enemyHealth -= damage;
			Player playerInfo = player.GetComponent<Player> ();
			playerInfo.playerPointTotal += damage;
			if (damageExplosive)
				playerInfo.playerPoint += damage / 5f;
			else
				playerInfo.playerPoint += damage;
			playerInfo.TextUpdate ();
			if (enemyHealth <= 0) {
			
				enemyDead = true;
				spawnerSystem.waveZombieNumberCurrent++;
				spawnerSystem.TextUpdate ();
				Destroy (gameObject);

			}

		}

	}

	IEnumerator AttackWait(float time) {

		enemyNavigation.Stop ();

		yield return new WaitForSeconds (time);

		if (Vector3.Distance (player.transform.position, transform.position) <= enemyAttackRange && !enemyKnockback && enemyAttack) {

			Player playerInfo = player.GetComponent<Player> ();
			playerInfo.ChangeHealth (enemyDamage);

		}

		enemyAttackAvailable = true;
		enemyNavigation.Resume ();
		enemyAttack = false;

	}

}
