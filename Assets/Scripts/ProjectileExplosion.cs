using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour {

	public float damage;
	public float knockback;
	public float range;
	public float off = 0f;

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
			enemyInfo.enemyRigidbody.AddForce (Quaternion.LookRotation (enemyInfo.transform.position - transform.position) * Vector3.forward * knockback, ForceMode.Impulse);

		}

	}

	void Update() {

		off += Time.deltaTime;
		exp.intensity = Mathf.Lerp(8, 0, off);
		if (off >= 1f)
			Destroy (gameObject);

	}

}
