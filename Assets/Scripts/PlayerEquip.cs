using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// Component

	public PlayerCamera playerCameraInfo;
	public AudioSource playerSound;

	// WEAPON LIST

	public enum itemWeaponList {

		Player_Weapon_Rifle_1,
		Player_Weapon_Shotgun_1,
		Player_Weapon_Pistol_1,
		player_Weapon_SMG_1,
		Player_Weapon_SMG_2,
		Player_Weapon_Rifle_2,
		Player_Weapon_SMG_3,
		player_Weapon_GrenadeLauncher_1,
		player_Weapon_RocketLauncher_1

	}

	public int itemSlotNumber = 0;
	public ItemWeapon[] itemSlot = new ItemWeapon[2];

	public float[] weaponDamageAdd;
	public float[] weaponClipAdd;
	public bool[] weaponLaserAdd;

	public string[] weaponName;
	public GameObject[] weaponModel;
	public int[] weaponPallet;
	public float[] weaponDamage;
	public float[] weaponKnockback;
	public float[] weaponRecoil;
	public float[] weaponClip;
	public float[] weaponRange;
	public float[] weaponSpreadMin;
	public float[] weaponSpreadMax;
	public float[] weaponSpeed;
	public weaponProjectileList[] weaponProjectile;
	public weaponShellList[] weaponShell;
	public bool[] weaponSemi;
	public bool[] weaponZoom;
	public Vector3[] weaponPosition;
	public Vector3[] weaponBarrelPosition;
	public AudioClip[] weaponSound;

	public enum weaponProjectileList {

		ProjectileBullet,
		ProjectileGrenade,
		ProjectileRocket

	}
	public enum weaponShellList {

		ShellRifle,
		ShellShotgun,
		ShellPistol

	}

	// VIEW

	public bool viewZoom;
	public bool viewTop;

	// PLAYER INFO 

	public GameObject player;
	public Player playerInfo;

	public bool playerDead;

	// MOUSE INFO

	Vector3 mousePosition;
	Vector3 mouseTarget;

	// BULLET INFO

	public GameObject[] bulletObject;

	public GameObject[] bulletShell;

	// WEAPON BARREL

	public GameObject weaponBarrel;

	public GameObject weaponFlash;
	public GameObject weaponFlashlight;

	public bool weaponFlashlightOn;

	public LineRenderer weaponLaserpoint;
	public Material weaponLaserpointMaterial;

	// UI

	public Text textBullet;
	public Text[] textWeapon;
	public Text textClip;
	public Text textDamage;

	// other Sound

	public AudioClip weaponSoundSwitch;
	public AudioClip weaponSoundFlashlight;

	// ect

	public bool helpmode;
	public bool helpmodeDisableGun;
	public bool helpmodeDisableFlash;

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		weaponDamageAdd = new float[weaponModel.Length];
		weaponClipAdd = new float[weaponModel.Length];
		weaponLaserAdd = new bool[weaponModel.Length];

		for (int i = 0; i < weaponModel.Length; i++) {

			weaponDamageAdd [i] = 0f;
			weaponClipAdd [i] = 0f;
			//weaponLaserAdd[i] = true;

		}
			
		itemSlot [0] = WeaponChange(2);
		itemSlot [1] = WeaponChange(2);

		WeaponSelect (itemSlot [itemSlotNumber]);
		WeaponUpdate (0, true, true, false);
		WeaponUpdate (1, true, true, false);

	}

	void Start() {

		WeaponLaserpoint ();

	}

	void Update() {

		if (!playerDead) {

			mousePosition = playerInfo.mousePosition - transform.position;
			mousePosition.y = 0f;
			transform.rotation = Quaternion.LookRotation (mousePosition);

			if (!helpmodeDisableGun) {

				if ((itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButtonDown (0)) || ((!itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButton (0)))) {

					WeaponAttack (itemSlot [itemSlotNumber]);

				}

				WeaponScroll ();

			}

			if (Input.GetKeyDown (KeyCode.F) && !helpmodeDisableFlash) {

				weaponFlashlightOn = !weaponFlashlightOn;
				weaponFlashlight.SetActive (weaponFlashlightOn);
				playerSound.PlayOneShot (weaponSoundFlashlight);

			}
			
			if (weaponLaserpoint == null)
				WeaponLaserpoint ();
			else {

				if (itemSlot [itemSlotNumber].weaponLaserpoint) {
					
					weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
					weaponLaserpoint.SetPosition (1, WeaponRotation ());

				} else {
					
					weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
					weaponLaserpoint.SetPosition (1, weaponLaserpoint.transform.position);

				}

			}

			if (itemSlot [itemSlotNumber].weaponZoom && !helpmode) {

				if (Input.GetMouseButtonDown (1)) {

					viewZoom = !viewZoom;
					if (viewZoom)
						playerCameraInfo.playerCameraMain.orthographicSize = 12.5f;
					else
						playerCameraInfo.playerCameraMain.orthographicSize = playerCameraInfo.playerCameraZoom;

				}

			}

			if (Input.GetMouseButtonDown (2) && !helpmode) {

				viewTop = !viewTop;
				if (viewTop)
					playerCameraInfo.View (true);
				else
					playerCameraInfo.View (false);

			}

			WeaponHeal (itemSlot [itemSlotNumber]);

		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Weapon Info

	public void WeaponSelect (ItemWeapon weapon) {  // Select Weapon

		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) weapons [i].SetActive (false);
		weapon.weaponModel.SetActive (true);
		weapon.weaponModel.transform.localPosition = weapon.weaponPosition;
		weaponBarrel.transform.localPosition = weapon.weaponPoint;
		if (viewZoom && !helpmode) {
			viewZoom = false;
			playerCameraInfo.playerCameraMain.orthographicSize = playerCameraInfo.playerCameraZoom;
		}

		TextUpdate (itemSlot [itemSlotNumber]);

	}

	void WeaponScroll () {

		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (itemSlot[itemSlotNumber].weaponAvailableAttack && wheel != 0 ) {

			if (wheel < 0) itemSlotNumber--;
			else if (wheel > 0) itemSlotNumber++;

			itemSlotNumber = Mathf.Clamp (itemSlotNumber, 0, itemSlot.Length - 1);
			WeaponSelect (itemSlot[itemSlotNumber]);
			playerSound.PlayOneShot (weaponSoundSwitch);

		}

	}

	// Attack

	public void WeaponAttack(ItemWeapon weapon) {

		if (weapon.weaponAvailableAttack && weapon.weaponBullet > 0) {

			StartCoroutine (WeaponSpawnProjectile (weapon));
			StartCoroutine (WeaponSpawnLight ());
			WeaponSpawnShell (weapon.weaponShell);
			transform.localPosition = new Vector3(0.75f,0f,0f);

		}

	}
	IEnumerator WeaponSpawnProjectile(ItemWeapon weapon) {

		// Before Fire

		weapon.weaponAvailableAttack = false;

		for (int i = 0; i < weapon.weaponPallet; i++) {

			weapon.WeaponRecoilCalculate ();

			GameObject projectileClone = Instantiate (bulletObject[weapon.weaponProjectile], weaponBarrel.transform.position, transform.rotation * Quaternion.Euler (itemSlot[itemSlotNumber].weaponSpreadCircle));
			Projectile projectileInfo = projectileClone.GetComponent<Projectile> ();
			Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody> ();

			projectileInfo.range = weapon.weaponRange;
			projectileInfo.damage = weapon.weaponDamage * projectileInfo.damageMultiply;
			projectileInfo.knockback = weapon.weaponKnockback * projectileInfo.knockbackMultiply;

			projectileRigidbody.AddForce ( projectileInfo.transform.forward * projectileInfo.speed, ForceMode.Impulse);

			playerCameraInfo.playerCameraShake += weapon.weaponRecoil * playerCameraInfo.playerCameraShakeMultiply;

		}
			
		playerSound.PlayOneShot (weapon.weaponSound,1f);
		weapon.weaponBullet--;

		TextUpdate (itemSlot [itemSlotNumber]);

		yield return new WaitForSeconds (weapon.weaponTimeAttack);

		// After Fire

		weapon.weaponAvailableAttack = true;

	}
	IEnumerator WeaponSpawnLight() {

		weaponFlash.SetActive (true);
		yield return null;
		weaponFlash.SetActive (false);

	}
	void WeaponSpawnShell(int shellType) {

		GameObject shell = Instantiate (bulletShell [shellType], transform.position, transform.rotation);
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody> ();

		shellRigidbody.AddForce (transform.right * Random.Range (0.5f, 0.75f), ForceMode.Impulse);

	}

	Vector3 WeaponRotation () {

		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit rayHit;

		float rayLenght = 500f;

		int layerMask = LayerMask.GetMask ("Map", "Enemy");

		if (Physics.Raycast (ray, out rayHit, rayLenght, layerMask))
			return rayHit.point;
		else
			return transform.position + transform.forward * 100f;

	}

	void WeaponLaserpoint() {

		weaponLaserpoint = GameObject.Find("Player_Weapon_Laserpoint").GetComponent<LineRenderer> ();
		weaponLaserpoint.positionCount = 2;
		weaponLaserpoint.startWidth = 0.0375f;
		weaponLaserpoint.endWidth = 0.0375f;
		weaponLaserpoint.startColor = Color.white;
		weaponLaserpoint.endColor = Color.white;
		weaponLaserpoint.material = weaponLaserpointMaterial;

	}

	void WeaponHeal (ItemWeapon weapon) {  // Select Weapon

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.75f,0f,0.5f), Time.deltaTime * 5f);

	}

	public void TextUpdate(ItemWeapon weapon) {

		if (!helpmode) {

			textBullet.text = weapon.weaponBullet.ToString () + " / " + weapon.weaponClip;
			textWeapon [0].text = itemSlot [0].weaponName;
			textWeapon [1].text = itemSlot [1].weaponName;
			textWeapon [itemSlotNumber].text = "> " + itemSlot [itemSlotNumber].weaponName;

			if (weapon.weaponBullet <= weapon.weaponClip / 5f)
				textBullet.color = Color.red;
			else if (weapon.weaponBullet <= weapon.weaponClip / 3f)
				textBullet.color = Color.yellow;
			else
				textBullet.color = Color.white;

			textClip.text = "탄창 크기 증가 레벨 : " + weaponClipAdd [weapon.weaponNumber].ToString();
			textDamage.text = "데미지 증가 레벨 : " + weaponDamageAdd [weapon.weaponNumber].ToString();

		}

	}

	public void WeaponUpdate(int number, bool damage, bool clip, bool clipAdd) {
		
		if(damage) itemSlot [number].weaponDamage = weaponDamage[itemSlot [number].weaponNumber] + ( weaponDamage[itemSlot [number].weaponNumber] * weaponDamageAdd [itemSlot [number].weaponNumber] / 5f);
		if (clip) {

			switch(itemSlot [number].weaponNumber) {

				case 7:
				case 8:
					if (!clipAdd) 
						itemSlot [number].weaponBullet = Mathf.RoundToInt (weaponClip [itemSlot [number].weaponNumber] + (weaponClipAdd [itemSlot [number].weaponNumber] * 2f));
					else itemSlot [number].weaponBullet += 2;
					break;

				default:
					if (!clipAdd)
						itemSlot [number].weaponBullet = Mathf.RoundToInt (weaponClip [itemSlot [number].weaponNumber] + (weaponClip [itemSlot [number].weaponNumber] * weaponClipAdd [itemSlot [number].weaponNumber] / 4f));
					else itemSlot [number].weaponBullet += Mathf.RoundToInt (weaponClip [itemSlot [number].weaponNumber] / 4f);
					break;
				
			}

		}

	}

	public ItemWeapon WeaponChange(int number) {

		return new ItemWeapon (number, weaponName [number], weaponModel [number], weaponPallet [number], weaponDamage [number], weaponKnockback [number], weaponRecoil [number], weaponClip [number], weaponRange [number],	weaponSpreadMin [number], weaponSpreadMax [number], weaponSpeed [number], (int)weaponProjectile [number], (int)weaponShell [number], weaponSemi [number], weaponZoom[number], weaponLaserAdd [number], weaponPosition [number], weaponBarrelPosition [number], weaponSound[number]); 

	}

}
