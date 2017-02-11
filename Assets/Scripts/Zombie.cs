using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {

	public GameObject player;
	public NavMeshAgent enemyNavigation;
	public Rigidbody enemyRigidbody;

	public float enemyHealth;

	public bool enemyKnockback;

	void Start () {


		enemyNavigation = gameObject.GetComponent<NavMeshAgent> ();
		enemyRigidbody = gameObject.GetComponent<Rigidbody> ();
		enemyRigidbody.freezeRotation = true;

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

	public void EnemyChangeHealth(float damage) {

		enemyHealth -= damage;
		if (enemyHealth <= 0)
			Destroy (gameObject);

	}

}
