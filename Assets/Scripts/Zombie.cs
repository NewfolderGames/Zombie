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
			
			enemyNavigation.SetDestination (player.transform.position);
			if (Vector3.Distance(player.transform.position, transform.position) <= enemyAttackRange && enemyAttack) {

				Player playerInfo = player.GetComponent<Player> ();
				playerInfo.ChangeHealth (enemyDamage);
				enemyAttackAvailable = true;
				enemyAttack = false;

			} else if (Vector3.Distance(player.transform.position, transform.position) <= enemyAttackRange && !enemyAttack) {

				if (enemyAttackAvailable) {
					enemyAttackAvailable = false;
					StartCoroutine (AttackWait (enemyAttackTime));
				}

			} else enemyAttack = false;

		}
		else {

			enemyNavigation.Stop ();
			if (enemyRigidbody.velocity == Vector3.zero) {

				enemyNavigation.Resume ();
				enemyKnockback = false;

			}

		}

	}
	/*
	void OnCollisionEnter( Collision other ) {

		if (other.gameObject.tag == "Player") {

			if (!enemyAttack) {

				StartCoroutine (AttackWait (enemyAttackTime));

			} else {

				Player playerInfo = player.GetComponent<Player> ();
				playerInfo.ChangeHealth (enemyDamage);
				enemyAttack = false;

			}

		}

	}*/

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void EnemyChangeHealth(float damage) {

		if (!enemyDead) {
			
			enemyHealth -= damage;
			Player playerInfo = player.GetComponent<Player> ();
			playerInfo.playerPointTotal += damage;
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

		yield return new WaitForSeconds (time);
		enemyAttack = true;

	}

}
