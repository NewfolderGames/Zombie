using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public int weaponNumber;
	public string weaponName;
	public GameObject weaponModel; 

	public int weaponPallet;
	public float weaponDamage;
	public float weaponKnockback;

	public int weaponBullet;
	public int weaponClip;

	public float weaponRange;

	public float weaponRecoil;

	public float weaponSpreadMin;
	public float weaponSpreadMax;
	public float weaponSpreadPercent;

	public float weaponTimeAttack;

	public bool weaponAvailableAttack;

	public float weaponSpreadAngle;
	public Vector3 weaponSpreadCircle;

	public weaponShellList weaponShell;
	public enum weaponShellList {

		ShellRifle,
		ShellShotgun

	}

	// ========== ========== ========== CONSTRUCTOR ========== ========== ========== \\

	public ItemWeapon ( int number, string name, GameObject model, int pallet, float damage, float knockback, int clip, float range, float spreadMin, float spreadMax, float timeAttack, weaponShellList shell ) {

		weaponNumber = number;
		weaponName = name;
		weaponModel = model;

		weaponPallet = pallet;
		weaponDamage = damage;
		weaponKnockback = knockback;

		weaponBullet = clip;
		weaponClip = clip;

		weaponRange = range;

		weaponSpreadMin = spreadMin;
		weaponSpreadMax = spreadMax;

		weaponTimeAttack = timeAttack;

		weaponAvailableAttack = true;

		WeaponRecoilCalculate ();

		weaponShell = shell;

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Recoil / Spread

	public void WeaponRecoilCalculate() {  // Calculate Recoil

		weaponSpreadAngle = Random.Range (0, 360);
		weaponSpreadPercent = Mathf.Clamp(weaponSpreadMin, weaponSpreadMax, Random.Range( weaponSpreadMin, weaponSpreadMax ));

		weaponSpreadCircle.x = Mathf.Cos (weaponSpreadAngle) * weaponSpreadPercent;
		weaponSpreadCircle.y = Mathf.Sin (weaponSpreadAngle) * weaponSpreadPercent;
		weaponSpreadCircle.z = 0;

	}
		
}
