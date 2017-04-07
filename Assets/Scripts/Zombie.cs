using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Zombie : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public GameObject player;
	public NavMeshAgent enemyNavigation;
	public Rigidbody enemyRigidbody;
	public SpawnerSystem spawnerSystem;

	public float enemyHealth;
	public float enemyHealthMulti;
	public float enemySpeed;
	public float enemyDamage;
	public bool enemyDead;

	public bool enemyKnockback;
	public float enemyKnockbackMulti;

	public float enemyAttackTime;
	public bool enemyAttackAvailable = true;

	public bool enemyAttackCharge;
	public float enemyAttackChargeTime;
	public float enemyAttackChargeEndTime;
	public float enemyAttackChargeDelay;
	public float enemyAttackChargeSpeed;
	public float enemyAttackChargeDamage;
	public float enemyAttackChargeKnockback;
	public float enemyAttackChargeRangeMin;
	public float enemyAttackChargeRangeMax;
	public bool enemyAttackChargeAvailable;

	public bool enemyBoss;

	public bool damageExplosive = false;

	public AudioSource enemySound;
	public AudioClip enemySoundHit;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start () {

		spawnerSystem = GameObject.Find ("Spawner_System").GetComponent<SpawnerSystem> ();

		enemyNavigation = gameObject.GetComponent<NavMeshAgent> ();
		enemyRigidbody = gameObject.GetComponent<Rigidbody> ();
		enemyRigidbody.freezeRotation = true;

		enemyNavigation.speed = enemySpeed;

		enemyHealth *= enemyHealthMulti;

		player = GameObject.Find ("Player");

	}

	void Update () {

		if (!enemyKnockback) {

			if(enemyAttackChargeAvailable && Vector3.Distance (player.transform.position, transform.position) <= enemyAttackChargeRangeMax && Vector3.Distance (player.transform.position, transform.position) >= enemyAttackChargeRangeMin) {

				if (!enemyAttackCharge) {

					enemyAttackCharge = true;
					enemyAttackChargeAvailable = false;
					StartCoroutine (AttackCharge (enemyAttackChargeTime));

				}

			} else {

				if (!enemyAttackCharge) {
					
					enemyNavigation.isStopped = false;
					enemyNavigation.SetDestination (player.transform.position);

				}

			}

		}
		else {

			enemyNavigation.isStopped = true;
			if (enemyRigidbody.velocity == Vector3.zero) {

				enemyNavigation.isStopped = false;
				enemyKnockback = false;

			}

		}

	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.tag == player.tag) {
			
			if (enemyAttackAvailable && !enemyAttackCharge) {

				enemyAttackAvailable = false;
				Player playerInfo = player.GetComponent<Player> ();
				playerInfo.ChangeHealth (enemyDamage);
				StartCoroutine (AttackMeleeDelay (enemyAttackTime));

			}
			if (enemyAttackCharge) {

				player.GetComponent<Player> ().ChangeHealth (enemyAttackChargeDamage);
				player.GetComponent<Rigidbody> ().AddForce (transform.rotation * Vector3.forward * enemyAttackChargeKnockback, ForceMode.Impulse);

			}
				
		}
		
	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void EnemyChangeHealth(float damage) {

		if (!enemyDead) {
			
			enemyHealth -= damage;
			enemySound.PlayOneShot (enemySoundHit);
			Player playerInfo = player.GetComponent<Player> ();
			playerInfo.playerPoint += damage;
			playerInfo.playerPointTotal += damage;

			playerInfo.TextUpdate ();
			if (enemyHealth <= 0) {
			
				enemyDead = true;
				if (!enemyBoss) {

					if (!spawnerSystem.waveWait) {
						
						spawnerSystem.waveZombieNumberCurrent++;
						spawnerSystem.TextUpdate ();

					}

				} else {

					spawnerSystem.waveBossDead = true;
					playerInfo.playerPoint += 500;
					playerInfo.playerPointTotal += 500;

				}
				Destroy (gameObject);

			}

		}

	}

	IEnumerator AttackMeleeDelay(float time) {

		yield return new WaitForSeconds (time);

		enemyAttackAvailable = true;

	}

	IEnumerator AttackCharge(float time) {

		enemyNavigation.isStopped = true;

		yield return new WaitForSeconds (time);

		if (enemyAttackCharge) {

			Quaternion rotation = Quaternion.LookRotation (player.transform.position - transform.position);
			transform.rotation = rotation;
			enemyRigidbody.AddForce (rotation * Vector3.forward * enemyAttackChargeSpeed, ForceMode.Impulse);
			StartCoroutine(AttackChargeEnd (enemyAttackChargeEndTime));

		} else {

			StartCoroutine(AttackChargeDelay (enemyAttackChargeDelay));
			enemyAttackCharge = false;

		}

	}
	IEnumerator AttackChargeEnd(float time) {

		yield return new WaitForSeconds (time);
		enemyAttackCharge = false;
		StartCoroutine(AttackChargeDelay (enemyAttackChargeDelay));

	}
	IEnumerator AttackChargeDelay(float time) {

		yield return new WaitForSeconds (time);
		enemyAttackChargeAvailable = true;

	}

}
