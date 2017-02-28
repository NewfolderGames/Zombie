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

	// other Sound

	public AudioClip weaponSwitch;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		weaponDamageAdd = new float[weaponModel.Length];
		weaponClipAdd = new float[weaponModel.Length];
		weaponLaserAdd = new bool[weaponModel.Length];

		for (int i = 0; i < weaponModel.Length; i++) {

			weaponDamageAdd [i] = 0f;
			weaponClipAdd [i] = 0f;

		}
			
		itemSlot [0] = WeaponChange((int)Random.Range (0f, weaponModel.Length));
		itemSlot [1] = WeaponChange((int)Random.Range (0f, weaponModel.Length));

		WeaponSelect (itemSlot [itemSlotNumber]);
		WeaponUpdate (0, true, true, false);
		WeaponUpdate (1, true, true, false);

	}

	void Start() {

		WeaponLaserpoint ();

	}

	void Update() {

		if (!playerDead) {

			// MOUSE

			mousePosition = playerInfo.mousePosition - transform.position;
			mousePosition.y = 0f;
			transform.rotation = Quaternion.LookRotation (mousePosition);

			if ((itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButtonDown (0)) || ((!itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButton (0)))) {

				WeaponAttack (itemSlot [itemSlotNumber]);

			}

			WeaponScroll ();


			if (Input.GetKeyDown (KeyCode.F)) {

				weaponFlashlightOn = !weaponFlashlightOn;
				weaponFlashlight.SetActive (weaponFlashlightOn);

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

			if (itemSlot [itemSlotNumber].weaponZoom) {

				if (Input.GetMouseButtonDown (1)) {

					viewZoom = !viewZoom;
					if (viewZoom)
						playerCameraInfo.playerCameraMain.orthographicSize = 12.5f;
					else
						playerCameraInfo.playerCameraMain.orthographicSize = playerCameraInfo.playerCameraZoom;

				}

			}

			if (Input.GetMouseButtonDown (2)) {

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
		playerSound.PlayOneShot (weaponSwitch);
		//playerSound.clip = weapon.weaponSound;
		if (viewZoom) {
			viewZoom = false;
			playerCameraInfo.playerCameraMain.orthographicSize = playerCameraInfo.playerCameraZoom;
		}

		TextUpdate (itemSlot [itemSlotNumber]);

	}

	void WeaponScroll () {

		if (itemSlot[itemSlotNumber].weaponAvailableAttack) {
			
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {

				itemSlotNumber--;
				itemSlotNumber = Mathf.Clamp (itemSlotNumber, 0, itemSlot.Length - 1);
				WeaponSelect (itemSlot[itemSlotNumber]);

			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {

				itemSlotNumber++;
				itemSlotNumber = Mathf.Clamp (itemSlotNumber, 0, itemSlot.Length - 1);
				WeaponSelect (itemSlot[itemSlotNumber]);

			}

		}

	}

	// Attack

	public void WeaponAttack(ItemWeapon weapon) {

		if ( weapon.weaponAvailableAttack && weapon.weaponBullet-1 >= 0) {

			StartCoroutine (WeaponSpawnProjectile (weapon));
			StartCoroutine (WeaponSpawnLight (Time.deltaTime));
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
	IEnumerator WeaponSpawnLight(float time) {

		weaponFlash.SetActive (true);
		yield return new WaitForSeconds (time);
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
		weaponLaserpoint.numPositions = 2;
		weaponLaserpoint.startWidth = 0.02f;
		weaponLaserpoint.endWidth = 0.02f;
		weaponLaserpoint.startColor = Color.white;
		weaponLaserpoint.endColor = Color.white;
		weaponLaserpoint.material = weaponLaserpointMaterial;

	}

	void WeaponHeal (ItemWeapon weapon) {  // Select Weapon

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.75f,0f,0.5f), Time.deltaTime * 5f);

	}

	public void TextUpdate(ItemWeapon weapon) {

		textBullet.text = weapon.weaponBullet.ToString();
		textWeapon[0].text = itemSlot[0].weaponName;
		textWeapon[1].text = itemSlot[1].weaponName;
		textWeapon[itemSlotNumber].text = "> " + itemSlot[itemSlotNumber].weaponName;

		if (weapon.weaponBullet <= weapon.weaponClip / 5f)
			textBullet.color = Color.red;
		else if(weapon.weaponBullet <= weapon.weaponClip / 3f)
			textBullet.color = Color.yellow;
		else
			textBullet.color = Color.white;

	}

	public void WeaponUpdate(int number, bool damage, bool clip, bool clipAdd) {
		
		if(damage) itemSlot [number].weaponDamage = weaponDamage[itemSlot [number].weaponNumber] + ( weaponDamage[itemSlot [number].weaponNumber] * weaponDamageAdd [itemSlot [number].weaponNumber] / 5f);
		if (clip) {

			switch(itemSlot [number].weaponNumber) {

				case 7:
				case 8:
					if(!clipAdd) itemSlot [number].weaponBullet = Mathf.RoundToInt(weaponClip [itemSlot [number].weaponNumber] + (weaponClipAdd [itemSlot [number].weaponNumber] * 2f));
					else itemSlot [number].weaponBullet += 2;
					break;

				default:
					if(!clipAdd) itemSlot [number].weaponBullet = Mathf.RoundToInt (weaponClip [itemSlot [number].weaponNumber] + (weaponClip [itemSlot [number].weaponNumber] * weaponClipAdd [itemSlot [number].weaponNumber] / 4f));
					else itemSlot [number].weaponBullet += Mathf.RoundToInt (weaponClip [itemSlot [number].weaponNumber] / 4f);
					break;
				
			}

		}

	}

	public ItemWeapon WeaponChange(int number) {

		return new ItemWeapon (number, weaponName [number], weaponModel [number], weaponPallet [number], weaponDamage [number], weaponKnockback [number], weaponRecoil [number], weaponClip [number], weaponRange [number],	weaponSpreadMin [number], weaponSpreadMax [number], weaponSpeed [number], (int)weaponProjectile [number], (int)weaponShell [number], weaponSemi [number], weaponZoom[number], weaponLaserAdd [number], weaponPosition [number], weaponBarrelPosition [number], weaponSound[number]); 

	}

}
