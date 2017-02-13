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

		Player_Weapon_Test47,
		Player_Weapon_Test12,
		Player_Weapon_Test18,
		player_Weapon_Test45,
		Player_Weapon_TestSMG,
		Player_Weapon_WTF

	}

	public int itemSlotNumber = 0;
	public ItemWeapon[] itemSlot = new ItemWeapon[2];

	// PLAYER INFO 

	public GameObject player;
	public Player playerInfo;

	// MOUSE INFO

	Vector3 mousePosition;
	Vector3 mouseTarget;

	// BULLET INFO

	public GameObject bulletObject;

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

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		itemSlot [0] = WeaponRandom((int)Random.Range (0f, 6f));
		itemSlot [1] = WeaponRandom((int)Random.Range (0f, 6f));

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

	void WeaponSelect (ItemWeapon weapon) {  // Select Weapon

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

			GameObject projectileClone = Instantiate (bulletObject, weaponBarrel.transform.position, transform.rotation * Quaternion.Euler (itemSlot[itemSlotNumber].weaponSpreadCircle));
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

	}

	public ItemWeapon WeaponRandom(int number) {

		switch (number) {

		//							number	name					modelobject										pallet	damage	knkbck	recoil	clip	range	min		max		speed		shelltype								semi	laser	modelposition					barrelposition

		case (int)itemWeaponList.Player_Weapon_Test47:
			return new ItemWeapon (0,	"Player_Weapon_Test47",	 	GameObject.Find ("Player_Weapon_Test47"), 		1,		5f,		1.5f,	1.2f,	150,	10f,	0.1f,	1f, 	3f / 60f,	ItemWeapon.weaponShellList.ShellRifle,	false,	true,	new Vector3 (0f, 0f, 0f), 		new Vector3 (0f, 0f, 0.5f));
		case (int)itemWeaponList.Player_Weapon_Test12:
			return new ItemWeapon (1,	"Player_Weapon_Shotgun_1",	GameObject.Find ("Player_Weapon_Shotgun_1"), 	12,		2f,		0.3f,	0.3f,	25,		8f,		0.5f,	7.5f,	45f / 60f,	ItemWeapon.weaponShellList.ShellShotgun,true,	true,	new Vector3(0f,0.0f,0f),		new Vector3(0f,0.025f,1f));
		case (int)itemWeaponList.Player_Weapon_Test18:
			return new ItemWeapon (2,	"Player_Weapon_Pistol_1",	GameObject.Find ("Player_Weapon_Pistol_1"), 	1, 		4f,		1f,		1.5f, 	60,		10f,	0.25f,	1f, 	6f / 60f,	ItemWeapon.weaponShellList.ShellPistol,	true,	true,	new Vector3 (0f, -0.25f, 0f), 	new Vector3 (0f, 0.02f, 0.23f));
		case (int)itemWeaponList.player_Weapon_Test45:
			return new ItemWeapon (3,	"Player_Weapon_SMG_1",		GameObject.Find ("Player_Weapon_SMG_1"), 		1, 		5f,		1.5f,	0.5f,	250,	10f,	0.25f,	2f, 	7f / 60f,	ItemWeapon.weaponShellList.ShellPistol,	false,	true,	new Vector3 (0f, -0.35f, 0f),	new Vector3 (0f, 0.01f, 0.5f));
		case (int)itemWeaponList.Player_Weapon_TestSMG:
			return new ItemWeapon (4,	"Player_Weapon_SMG_2",		GameObject.Find ("Player_Weapon_SMG_2"),		1, 		4f,		1f,		0.75f, 	500,	10f,	0.1f,	1.5f,	4f / 60f,	ItemWeapon.weaponShellList.ShellPistol,	false,	true,	new Vector3 (0f, -0.375f, 0f),	new Vector3 (0f, 0.015f, 0.7875f));
		case (int)itemWeaponList.Player_Weapon_WTF:
			return new ItemWeapon (5,	"Player_Weapon_TestWTF",	GameObject.Find ("Player_Weapon_TestWTF"),		10, 	1f, 	0.1f,	0.1f, 	9999,	10f,	0f, 	10f,	1f / 60f,	ItemWeapon.weaponShellList.ShellRifle,	false,	true,	new Vector3 (0f, 0f, 0f), 		new Vector3 (0f, 0f, 1f));

		}
		return null;

	}

}
