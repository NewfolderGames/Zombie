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

	public bool weaponAvailableShoot;
	public bool weaponAvailableReload;
		
}
