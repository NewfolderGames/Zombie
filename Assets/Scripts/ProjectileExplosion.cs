using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProjectileExplosion : MonoBehaviour {

	public float damage;
	public float knockback;
	public float range;
	public float off = 0f;

	public AudioSource audioSource;
	public AudioClip sound;

	public Light exp;

	void Start() {

		transform.localScale = new Vector3 (range, range, range);

		int mask = LayerMask.GetMask ("Enemy");
		Collider[] enemy = Physics.OverlapSphere (transform.position, range, mask);
		for (int i = 0; i < enemy.Length; i++) {

			Zombie enemyInfo = enemy [i].GetComponent<Zombie> ();
			enemyInfo.damageExplosive = true;
			enemyInfo.EnemyChangeHealth (damage);
			enemyInfo.enemyKnockback = true;
			enemyInfo.enemyRigidbody.AddExplosionForce (knockback * enemyInfo.enemyKnockbackMulti, transform.position, range);

		}
		audioSource.PlayOneShot (sound);

	}

	void Update() {

		off += Time.deltaTime * 0.75f;
		exp.intensity = Mathf.Lerp(8, 0, off);
		if (off >= 1f)
			Destroy (gameObject);

	}

}
