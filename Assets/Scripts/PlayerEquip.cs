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

		Player_Weapon_Test47,
		Player_Weapon_Test12

	}

	public ItemWeapon[] itemWeapon = new ItemWeapon[2];

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
		itemWeapon [0] = new ItemWeapon (0, "Player_Weapon_Test47", GameObject.Find ("Player_Weapon_Test47"), 1, 10f, 1f, 30, 10f, 0.1f, 1f, 0.1f, 5f );
		itemWeapon [1] = new ItemWeapon (1, "Player_Weapon_Test12", GameObject.Find ("Player_Weapon_Test12"), 12, 2f, 0.2f, 5, 8f, 0.3f, 3f, 0.5f, 1f );

		WeaponSelect ();

	}

	void Update() {

		// MOUSE

		mousePosition = playerInfo.mousePosition - transform.position;
		mousePosition.y = 0f;
		transform.rotation = Quaternion.LookRotation (mousePosition);

		if (Input.GetMouseButton (0))
			WeaponAttack (itemWeapon [(int)weaponSelect]);

		if (Input.GetKeyDown (KeyCode.R))
			WeaponReload (itemWeapon [(int)weaponSelect]);

		WeaponScroll ();
		WeaponSelect ();



	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Weapon Info

	void WeaponSelect () {  // Select Weapon

		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) weapons [i].SetActive (false);
		itemWeapon [(int)weaponSelect].weaponModel.SetActive (true);

	}

	void WeaponScroll () {

		if (Input.GetAxis ("Mouse ScrollWheel") > 0){
			
			weaponSelect++;
			weaponSelect = (itemWeaponList)Mathf.Clamp ((int)weaponSelect, 0, 1);

		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0){

			weaponSelect--;
			weaponSelect = (itemWeaponList)Mathf.Clamp ((int)weaponSelect, 0, 1);

		}


	}

	// Attack

	public void WeaponAttack(ItemWeapon weapon) {

		if ( weapon.weaponAvailableAttack && !weapon.weaponIsReload && weapon.weaponBullet-1 >= 0) {

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

			playerCameraInfo.playerCameraShake += weapon.weaponKnockback * playerCameraInfo.playerCameraShakeMultiply;

		}

		weapon.weaponAvailableReload = true;
		weapon.weaponBullet--;


		yield return new WaitForSeconds (weapon.weaponTimeAttack);

		// After Fire

		weapon.weaponAvailableAttack = true;

	}

	// Reload

	public void WeaponReload(ItemWeapon weapon) {
	
		if (weapon.weaponAvailableAttack && weapon.weaponAvailableReload && !weapon.weaponIsReload) {
			StartCoroutine (WeaponBulletReload (weapon));
		}
	
	}
	IEnumerator WeaponBulletReload(ItemWeapon weapon) {

		weapon.weaponIsReload = true;
		weapon.weaponAvailableAttack = false;
		weapon.weaponAvailableReload = false;

		yield return new WaitForSeconds (weapon.weaponTimeReload);

		if (weapon.weaponBullet > 0)
			weapon.weaponBullet = weapon.weaponClip + 1;
		else
			weapon.weaponBullet = weapon.weaponClip;
		
		weapon.weaponIsReload = false;
		weapon.weaponAvailableAttack = true;

	}

}
