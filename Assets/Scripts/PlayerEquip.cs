using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// CAMRTA

	public PlayerCamera playerCameraInfo;

	// WEAPON LIST

	public enum itemWeaponList {

		Player_Weapon_Rifle_1,
		Player_Weapon_Shotgun_1,
		Player_Weapon_Pistol_1,
		player_Weapon_SMG_1,
		Player_Weapon_SMG_2,
		Player_Weapon_Rifle_2,
		Player_Weapon_SMG_3,
		player_Weapon_GrenadeLauncher_1

	}

	public int itemSlotNumber = 0;
	public ItemWeapon[] itemSlot = new ItemWeapon[2];

	public float[] weaponDamageAdd;
	public float[] weaponClipAdd;
	public bool[] weaponLaserAdd;

	// PLAYER INFO 

	public GameObject player;
	public Player playerInfo;

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

	// WEAPON MODEL

	public GameObject[] weaponModel;

	// UI

	public Text textBullet;
	public Text textWeapon;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		weaponDamageAdd = new float[weaponModel.Length];
		weaponClipAdd = new float[weaponModel.Length];
		weaponLaserAdd = new bool[weaponModel.Length];

		for (int i = 0; i < weaponModel.Length; i++) {

			weaponDamageAdd [i] = 1f;
			weaponClipAdd [i] = 1f;

		}
			
		itemSlot [0] = WeaponChange((int)Random.Range (0f, weaponModel.Length));
		itemSlot [1] = WeaponChange((int)Random.Range (0f, weaponModel.Length));

		WeaponSelect (itemSlot [itemSlotNumber]);

	}

	void Start() {

		WeaponLaserpoint ();

	}

	void Update() {

		// MOUSE

		mousePosition = playerInfo.mousePosition - transform.position;
		mousePosition.y = 0f;
		transform.rotation = Quaternion.LookRotation (mousePosition);

		if ((itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButtonDown (0))||((!itemSlot [itemSlotNumber].weaponSemiauto && Input.GetMouseButton (0)))) {

			WeaponAttack (itemSlot [itemSlotNumber]);

		}

		WeaponScroll ();


		if (Input.GetKeyDown (KeyCode.F)) {

			weaponFlashlightOn = !weaponFlashlightOn;
			weaponFlashlight.SetActive (weaponFlashlightOn);

		}
			
		if (weaponLaserpoint == null)
			WeaponLaserpoint ();
		else{

			if (itemSlot[itemSlotNumber].weaponLaserpoint) {
					
				weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
				weaponLaserpoint.SetPosition (1, WeaponRotation ());

			} else {
					
				weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
				weaponLaserpoint.SetPosition (1, weaponLaserpoint.transform.position);

			}

		}

		WeaponHeal (itemSlot[itemSlotNumber]);

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Weapon Info

	public void WeaponSelect (ItemWeapon weapon) {  // Select Weapon

		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) weapons [i].SetActive (false);
		weapon.weaponModel.SetActive (true);
		weapon.weaponModel.transform.localPosition = weapon.weaponPosition;
		weaponBarrel.transform.localPosition = weapon.weaponPoint;

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
			WeaponSpawnShell ((int)weapon.weaponShell);
			transform.localPosition = new Vector3(0.75f,0f,0f);

		}

	}
	IEnumerator WeaponSpawnProjectile(ItemWeapon weapon) {

		// Before Fire

		weapon.weaponAvailableAttack = false;

		for (int i = 0; i < weapon.weaponPallet; i++) {

			weapon.WeaponRecoilCalculate ();

			GameObject projectileClone = Instantiate (bulletObject[(int)weapon.weaponProjectile], weaponBarrel.transform.position, transform.rotation * Quaternion.Euler (itemSlot[itemSlotNumber].weaponSpreadCircle));
			Projectile projectileInfo = projectileClone.GetComponent<Projectile> ();
			Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody> ();

			projectileInfo.range = weapon.weaponRange;
			projectileInfo.damage = weapon.weaponDamage * projectileInfo.damageMultiply;
			projectileInfo.knockback = weapon.weaponKnockback * projectileInfo.knockbackMultiply;

			projectileRigidbody.AddForce ( projectileInfo.transform.forward * projectileInfo.speed, ForceMode.Impulse);

			playerCameraInfo.playerCameraShake += weapon.weaponRecoil * playerCameraInfo.playerCameraShakeMultiply;

		}
			
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
		//weaponLaserpoint.useWorldSpace = true;

	}

	void WeaponHeal (ItemWeapon weapon) {  // Select Weapon

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.75f,0f,0.5f), Time.deltaTime * 5f);

	}

	void TextUpdate(ItemWeapon weapon) {

		textBullet.text = weapon.weaponBullet.ToString();
		textWeapon.text = weapon.weaponName;

		if (weapon.weaponBullet <= weapon.weaponClip / 5)
			textBullet.color = Color.red;
		else if(weapon.weaponBullet <= weapon.weaponClip / 3)
			textBullet.color = Color.yellow;
		else
			textBullet.color = Color.white;

	}

	public ItemWeapon WeaponChange(int number) {

		switch (number) {

		//						  number	name			modelobject			pallet	damage							knkbck	recoil	clip											range	min		max		speed		projectiletype										 shelltype									semi	laser					modelposition					barrelposition

		case (int)itemWeaponList.Player_Weapon_Rifle_1:
			return new ItemWeapon (0,	"Rifle_1",			weaponModel[0], 	1,		7.5f * weaponDamageAdd[0]	,	1f,		0.5f,	Mathf.RoundToInt(150f * weaponClipAdd[0])	,	10f,	0.1f,	1f, 	3f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellRifle,		false,	weaponLaserAdd[0]	,	new Vector3 (0f, -0.4f, 0f), 	new Vector3 (0f, 0f, 0.9f)); 
		case (int)itemWeaponList.Player_Weapon_Shotgun_1:
			return new ItemWeapon (1,	"Shotgun_1",		weaponModel[1], 	12,		2f * weaponDamageAdd[1]		,	0.3f,	0.3f,	Mathf.RoundToInt(25f * weaponClipAdd[1])	,	8f,		0.5f,	7.5f,	45f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellShotgun,	true,	weaponLaserAdd[1]	,	new Vector3(0f, 0f, 0f),		new Vector3(0f,0.025f,1f));
		case (int)itemWeaponList.Player_Weapon_Pistol_1:
			return new ItemWeapon (2,	"Pistol_1",			weaponModel[2], 	1, 		4.5f * weaponDamageAdd[2]	,	1f,		1.5f, 	Mathf.RoundToInt(100f * weaponClipAdd[2])	,	10f,	0.25f,	1f, 	6f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellPistol,	true,	weaponLaserAdd[2]	,	new Vector3 (0f, -0.25f, 0f), 	new Vector3 (0f, 0.02f, 0.23f)); 
		case (int)itemWeaponList.player_Weapon_SMG_1:
			return new ItemWeapon (3,	"SMG_1",			weaponModel[3], 	1, 		5.5f * weaponDamageAdd[3]	,	1.5f,	0.5f,	Mathf.RoundToInt(250f * weaponClipAdd[3])	,	10f,	0.25f,	2f, 	7f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellPistol,	false,	weaponLaserAdd[3]	,	new Vector3 (0f, -0.45f, 0f),	new Vector3 (0f, 0.0875f, 0.75f));
		case (int)itemWeaponList.Player_Weapon_SMG_2:
			return new ItemWeapon (4,	"SMG_2",			weaponModel[4],		1, 		5f * weaponDamageAdd[4]		,	1f,		0.75f, 	Mathf.RoundToInt(300f * weaponClipAdd[4])	,	10f,	0.1f,	1.5f,	4f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellPistol,	false,	weaponLaserAdd[4]	,	new Vector3 (0f, -0.375f, 0f),	new Vector3 (0f, 0.03f, 0.92f));
		case (int)itemWeaponList.Player_Weapon_Rifle_2:
			return new ItemWeapon (5,	"Rifle_2",			weaponModel[5], 	1,		12.5f * weaponDamageAdd[5]	,	2f,		2f,		Mathf.RoundToInt(60f * weaponClipAdd[5])	,	12f,	0.05f,	0.5f, 	6f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellRifle,		true,	weaponLaserAdd[5]	,	new Vector3 (0f, -0.475f, 0f), 	new Vector3 (0f, -0.0325f, 0.875f));
		case (int)itemWeaponList.Player_Weapon_SMG_3:
			return new ItemWeapon (6,	"SMG_3",			weaponModel[6],		1, 		4f * weaponDamageAdd[6]		,	1f,		0.75f, 	Mathf.RoundToInt(350f * weaponClipAdd[6])	,	10f,	0.25f,	2.5f,	3f / 60f,	ItemWeapon.weaponProjectileList.ProjectileBullet,	 ItemWeapon.weaponShellList.ShellPistol,	false,	weaponLaserAdd[6]	,	new Vector3 (0f, -0.45f, 0f),	new Vector3 (0f, -0.015f, 0.6f));
		case (int)itemWeaponList.player_Weapon_GrenadeLauncher_1:
			return new ItemWeapon (7,	"GrenadeLauncher_1",weaponModel[7],		1, 		25f * weaponDamageAdd[6]	,	1f,		10f, 	Mathf.RoundToInt(5f * weaponClipAdd[7])		,	2.5f,	0.1f,	0.5f,	120f / 60f,	ItemWeapon.weaponProjectileList.ProjectileGrenade,	 ItemWeapon.weaponShellList.ShellPistol,	false,	weaponLaserAdd[7]	,	new Vector3 (0f, -0.25f, 0f),	new Vector3 (0f, 0f, 0.405f));

		}
		return null;

	}

}
