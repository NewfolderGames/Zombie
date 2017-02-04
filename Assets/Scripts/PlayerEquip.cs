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

	}

	void Update() {

		// MOUSE

		bool mouse = Input.GetMouseButton (0);

		if (mouse)
			WeaponFire ();

		mousePosition = playerInfo.mousePosition;
		transform.rotation = Quaternion.LookRotation (mousePosition);


	}


	// FUNCTION

	void WeaponSelect ( itemWeaponList weapon ) {

		weaponSelect = weapon;
		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) {

			if( weapons [i].GetComponent<ItemWeapon>().weaponNumber != (int)weapon ) weapons [i].SetActive (false);
			else weapons [i].SetActive (true);

		}
			
	}

	void WeaponFire() {

		GameObject bullet = Instantiate (bulletObject,transform.position,transform.rotation);
		ProjectileBullet bulletInfo = bullet.GetComponent<ProjectileBullet> ();
		Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody> ();

		bulletInfo.damage = weaponDamage;
		bulletInfo.range = 10f;
		//bulletInfo.range = weaponRange;

		bulletRigidbody.AddForce (mousePosition * 2.5f, ForceMode.Impulse);

	}

}
