using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : ItemWeapon {

	//  WEAPON SETTING

	public itemWeaponList weaponSelect;
	public enum itemWeaponList {

		Block47

	}

	// PLAYER

	public GameObject player;
	public Player playerInfo;

	// MOUSE

	Vector3 mousePosition;
	Vector3 mouseTarget;

	// Bullet

	public GameObject bulletObject;

	void Awake() {

		playerInfo = player.GetComponent<Player> ();
		WeaponSelect (itemWeaponList.Block47);

	}

	void Update() {

		// MOUSE

		bool mouse = Input.GetMouseButton (0);

		if (mouse)
			StartCoroutine (weaponFire (weaponTimeShoot));

		mousePosition = playerInfo.mousePosition - transform.position;
		transform.rotation = Quaternion.LookRotation (mousePosition);

		WeaponSpreadHeal ();


	}


	// FUNCTION

	void WeaponSelect ( itemWeaponList weapon ) {

		weaponSelect = weapon;
		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) {

			if (weapons [i].GetComponent<ItemWeapon> ().weaponNumber != (int)weapon) weapons [i].SetActive (false);
			else {
				
				weapons [i].SetActive (true);
				overrideInfo (weapons [i].GetComponent<ItemWeapon>());

			}

		}
			
	}
		
	void WeaponSpreadHeal() {

		if (weaponSpread > weaponSpreadMin) {

			weaponSpread -= weaponSpreadHeal;
			weaponSpread = Mathf.Clamp (weaponSpread, weaponSpreadMin, weaponSpreadMax);

		} else return;

	}

	IEnumerator weaponFire( float time ) {

		// FIRE BULLET

		weaponAvailableShoot = false;

		for (int i = 0; i < weaponPallet; i++) {

			GameObject bullet = Instantiate (bulletObject, transform.position, transform.rotation);
			ProjectileBullet bulletInfo = bullet.GetComponent<ProjectileBullet> ();
			Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody> ();

			bulletInfo.damage = weaponDamage;
			bulletInfo.range = weaponRange;

			bulletRigidbody.AddForce (mousePosition * 5f, ForceMode.Impulse);

		}

		weaponSpread += weaponRecoil;
		weaponBullet--;

		// AFTER FIRE

		yield return new WaitForSeconds (time);

		weaponAvailableShoot = true;

	}

	void overrideInfo (ItemWeapon weapon) {

		weaponNumber = weapon.weaponNumber;
		weaponName = weapon.weaponName;

		weaponType = weapon.weaponType;

		weaponPallet = weapon.weaponPallet;
		weaponDamage = weapon.weaponDamage;

		weaponBullet = weapon.weaponBullet;
		weaponClip = weapon.weaponClip;

		weaponRange = weapon.weaponRange;

		weaponRecoil = weapon.weaponRecoil;
		weaponSpread = weapon.weaponSpread;
		weaponSpreadMin = weapon.weaponSpreadMin;
		weaponSpreadMax = weapon.weaponSpreadMax;
		weaponSpreadHeal = weapon.weaponSpreadHeal;

		weaponTimeShoot = weapon.weaponTimeShoot;
		weaponTimeReload = weapon.weaponTimeReload;

	}

}
