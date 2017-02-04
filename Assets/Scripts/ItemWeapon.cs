using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : MonoBehaviour {

	// WEAPON INFO

	public int weaponNumber;
	public string weaponName;

	public weaponTypeList weaponType;
	public enum weaponTypeList {

		melee,
		ranged,
		throwable

	}

	public int weaponPallet;
	public float weaponDamage;

	public int weaponBullet;
	public int weaponClip;

	public float weaponRange;

	public float weaponRecoil;
	public float weaponSpread;
	public float weaponSpreadMin;
	public float weaponSpreadMax;
	public float weaponSpreadHeal;

	public float weaponTimeShoot;
	public float weaponTimeReload;

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

	void WeaponSpreadHeal() {

		if (weaponSpread > weaponSpreadMin) {

			weaponSpread -= weaponSpreadHeal;
			weaponSpread = Mathf.Clamp (weaponSpread, weaponSpreadMin, weaponSpreadMax);

		} else return;

	}

}
