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

	public weaponProjectileList weaponProjectile;
	public enum weaponProjectileList {

		ProjectileBullet,
		ProjectileGrenade

	}

	public weaponShellList weaponShell;
	public enum weaponShellList {

		ShellRifle,
		ShellShotgun,
		ShellPistol

	}

	public bool weaponSemiauto;
	public bool weaponLaserpoint;

	public Vector3 weaponPosition;
	public Vector3 weaponPoint;

	// ========== ========== ========== CONSTRUCTOR ========== ========== ========== \\

	public ItemWeapon ( int number, string name, GameObject model, int pallet, float damage, float knockback, float recoil, int clip, float range, float spreadMin, float spreadMax, float timeAttack, weaponProjectileList projectile, weaponShellList shell, bool semiauto, bool laserpoint, Vector3 position, Vector3 point ) {

		weaponNumber = number;
		weaponName = name;
		weaponModel = model;

		weaponPallet = pallet;
		weaponDamage = damage;
		weaponKnockback = knockback;

		weaponBullet = clip;
		weaponClip = clip;

		weaponRange = range;

		weaponRecoil = recoil;

		weaponSpreadMin = spreadMin;
		weaponSpreadMax = spreadMax;

		weaponTimeAttack = timeAttack;

		weaponAvailableAttack = true;

		WeaponRecoilCalculate ();

		weaponProjectile = projectile;
		weaponShell = shell;

		weaponSemiauto = semiauto;

		weaponLaserpoint = laserpoint;

		weaponPosition = position;
		weaponPoint = point;

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Recoil / Spread

	public void WeaponRecoilCalculate() {  // Calculate Recoil

		weaponSpreadAngle = Random.Range (0, 360);
		weaponSpreadPercent = Mathf.Lerp(weaponSpreadMin, weaponSpreadMax, Random.Range( 0f, 1f ));

		weaponSpreadCircle.x = Mathf.Cos (weaponSpreadAngle) * weaponSpreadPercent;
		weaponSpreadCircle.y = Mathf.Sin (weaponSpreadAngle) * weaponSpreadPercent;
		weaponSpreadCircle.z = 0;

	}
		
}
