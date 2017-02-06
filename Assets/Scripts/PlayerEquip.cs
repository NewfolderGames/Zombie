using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// CAMRTA

	public PlayerCamera playerCameraInfo;

	// WEAPON LIST

	public itemWeaponList weaponSelect;
	public enum itemWeaponList {

		Player_Weapon_Test47

	}

	public ItemWeapon[] itemWeapon = new ItemWeapon[1];
	public Transform[] itemWeaponTransform;

	// PLAYER INFO 

	public GameObject player;
	public Player playerInfo;

	// MOUSE INFO

	Vector3 mousePosition;
	Vector3 mouseTarget;

	// BULLET INFO

	public GameObject bulletObject;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		// WEAPON LIST 
		itemWeapon [0] = new ItemWeapon (0, "Player_Weapon_Test47", GameObject.Find ("Player_Weapon_Test47"), ItemWeapon.weaponTypeList.ranged, 1, 10f, 1f, 30, 10f, 0.1f, 1f, 0.1f, 1f );

		itemWeaponTransform = GetComponentsInChildren<Transform>();

	}

	void Update() {

		// MOUSE

		mousePosition = playerInfo.mousePosition - transform.position;
		mousePosition.y = 0f;
		transform.rotation = Quaternion.LookRotation (mousePosition);

		bool mouse = Input.GetMouseButton (0);
		if (mouse)
			WeaponAttack (itemWeapon [(int)weaponSelect]);

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Weapon Info

	void WeaponSelect ( itemWeaponList weapon ) {  // Select Weapon

		weaponSelect = weapon;
		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) {

			if ( i != (int)weapon ) weapons [i].SetActive (false);
			else weapons [i].SetActive (true);

		}

	}

	// Attack

	public void WeaponAttack(ItemWeapon weapon) {

		if ( weapon.weaponAvailableAttack && (weapon.weaponType == ItemWeapon.weaponTypeList.melee || weapon.weaponBullet-1 >= 0)) {

			StartCoroutine (WeaponSpawnProjectile (weapon));

		}

	}
	IEnumerator WeaponSpawnProjectile(ItemWeapon weapon) {

		// Before Fire

		weapon.weaponAvailableAttack = false;

		for (int i = 0; i < weapon.weaponPallet; i++) {

			weapon.WeaponRecoilCalculate ();

			GameObject projectileClone = Instantiate (bulletObject, transform.position, transform.rotation * Quaternion.Euler (itemWeapon [(int)weaponSelect].weaponSpreadCircle));
			Projectile projectileInfo = projectileClone.GetComponent<Projectile> ();
			Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody> ();

			projectileInfo.range = weapon.weaponRange;
			projectileInfo.damage = weapon.weaponDamage * projectileInfo.damageMultiply;
			projectileInfo.knockback = weapon.weaponKnockback * projectileInfo.knockbackMultiply;

			projectileRigidbody.AddForce ( projectileInfo.transform.forward * projectileInfo.speed, ForceMode.Impulse);

		}

		playerCameraInfo.playerCameraShake += weapon.weaponKnockback * playerCameraInfo.playerCameraShakeMultiply;

		if (weapon.weaponType != ItemWeapon.weaponTypeList.melee)
			weapon.weaponBullet--;

		yield return new WaitForSeconds (weapon.weaponTimeAttack);

		// After Fire

		weapon.weaponAvailableAttack = true;

	}

}
