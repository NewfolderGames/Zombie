using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	// CAMRTA

	public PlayerCamera playerCameraInfo;

	// WEAPON LIST

	public itemWeaponList weaponSelect;
	public enum itemWeaponList {

		Player_Weapon_Test47,
		Player_Weapon_Test12,
		Player_Weapon_Test18,
		Player_Weapon_WTF

	}

	public ItemWeapon[] itemWeapon = new ItemWeapon[5];

	// PLAYER INFO 

	public GameObject player;
	public Player playerInfo;

	// MOUSE INFO

	Vector3 mousePosition;
	Vector3 mouseTarget;

	// BULLET INFO

	public GameObject bulletObject;

	public GameObject[] bulletShell;

	// LIGHT

	public GameObject weaponFlash;
	public GameObject weaponFlashlight;

	public bool weaponFlashlightOn;

	public LineRenderer weaponLaserpoint;
	public Material weaponLaserpointMaterial;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Awake() {

		playerInfo = player.GetComponent<Player> ();

		// WEAPON LIST 
		itemWeapon [0] = new ItemWeapon (0, "Player_Weapon_Test47", GameObject.Find ("Player_Weapon_Test47"), 	1,	10f,	1f,		150, 10f, 1f / 60f, 1f, 0.1f,ItemWeapon.weaponShellList.ShellRifle,false,true,new Vector3(0f,0f,0.5f),new Vector3(0f,0f,0.5f));
		itemWeapon [1] = new ItemWeapon (1, "Player_Weapon_Test12", GameObject.Find ("Player_Weapon_Test12"), 	12,	2f,		0.2f,	25, 8f, 30f / 60f, 3f, 0.5f,ItemWeapon.weaponShellList.ShellShotgun,true,true,new Vector3(0f,0.025f,1f),new Vector3(0f,0.025f,1f));
		itemWeapon [2] = new ItemWeapon (2, "Player_Weapon_Test18", GameObject.Find ("Player_Weapon_Test18"), 	1,	7.5f,	0.5f,	60, 10f, 5f / 60f, 0.5f, 0.2f,ItemWeapon.weaponShellList.ShellPistol,true,true,new Vector3(0f,0.27f,0.23f),new Vector3(0f,0.27f,0.23f));
		itemWeapon [3] = new ItemWeapon (3, "Player_Weapon_Test45", GameObject.Find ("Player_Weapon_Test45"), 	1,	10f,	0.5f,	250, 10f, 5f / 60f, 2f, 0.2f,ItemWeapon.weaponShellList.ShellPistol,false,true,new Vector3(0f,0f,0.5f),new Vector3(0f,0f,0.5f));
		itemWeapon [4] = new ItemWeapon (4, "Player_Weapon_TestWTF", GameObject.Find ("Player_Weapon_TestWTF"),	10,	1f,		0.1f,	9999, 10f, 1f / 60f, 10f, 0.01f,ItemWeapon.weaponShellList.ShellRifle,false,true,new Vector3(0f,0f,1f),new Vector3(0f,0f,1f));

		WeaponSelect ();

	}

	void Start() {

		WeaponLaserpoint ();

	}

	void Update() {

		// MOUSE

		mousePosition = playerInfo.mousePosition - transform.position;
		mousePosition.y = 0f;
		transform.rotation = Quaternion.LookRotation (mousePosition);

		if ((itemWeapon [(int)weaponSelect].weaponSemiauto && Input.GetMouseButtonDown (0))||(!itemWeapon [(int)weaponSelect].weaponSemiauto && Input.GetMouseButton (0))) {

			WeaponAttack (itemWeapon [(int)weaponSelect]);

		}

		WeaponScroll ();
		WeaponSelect ();

		if (Input.GetKeyDown (KeyCode.F)) {

			weaponFlashlightOn = !weaponFlashlightOn;
			weaponFlashlight.SetActive (weaponFlashlightOn);

		}
			
		if (weaponLaserpoint == null)
			WeaponLaserpoint ();
		else{

			if (itemWeapon [(int)weaponSelect].weaponLaserpoint) {
					
				weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
				weaponLaserpoint.SetPosition (1, WeaponRotation ());

			} else {
					
				weaponLaserpoint.SetPosition (0, weaponLaserpoint.transform.position);
				weaponLaserpoint.SetPosition (1, weaponLaserpoint.transform.position);

			}

		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	// Weapon Info

	void WeaponSelect () {  // Select Weapon

		GameObject[] weapons = GameObject.FindGameObjectsWithTag ("PlayerWeapon");
		for (int i = 0; i < weapons.Length; i++) weapons [i].SetActive (false);
		itemWeapon [(int)weaponSelect].weaponModel.SetActive (true);

		weaponFlashlight.transform.localPosition = itemWeapon [(int)weaponSelect].weaponPointFlashlight;
		weaponLaserpoint.transform.localPosition = itemWeapon [(int)weaponSelect].weaponPointLaserpoint;

	}

	void WeaponScroll () {

		if (Input.GetAxis ("Mouse ScrollWheel") > 0){
			
			weaponSelect--;
			weaponSelect = (itemWeaponList)Mathf.Clamp ((int)weaponSelect, 0, itemWeapon.Length - 1);

		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0){

			weaponSelect++;
			weaponSelect = (itemWeaponList)Mathf.Clamp ((int)weaponSelect, 0, itemWeapon.Length - 1);

		}


	}

	// Attack

	public void WeaponAttack(ItemWeapon weapon) {

		if ( weapon.weaponAvailableAttack && weapon.weaponBullet-1 >= 0) {

			StartCoroutine (WeaponSpawnProjectile (weapon));
			StartCoroutine (WeaponSpawnLight (Time.deltaTime));
			WeaponSpawnShell ();

		}

	}
	IEnumerator WeaponSpawnProjectile(ItemWeapon weapon) {

		// Before Fire

		weapon.weaponAvailableAttack = false;

		for (int i = 0; i < weapon.weaponPallet; i++) {

			weapon.WeaponRecoilCalculate ();

			GameObject projectileClone = Instantiate (bulletObject, transform.position, transform.rotation * Quaternion.Euler (itemWeapon [(int)weaponSelect].weaponSpreadCircle));
			Projectile projectileInfo = projectileClone.GetComponent<Projectile> ();
			Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody> ();

			projectileInfo.range = weapon.weaponRange;
			projectileInfo.damage = weapon.weaponDamage * projectileInfo.damageMultiply;
			projectileInfo.knockback = weapon.weaponKnockback * projectileInfo.knockbackMultiply;

			projectileRigidbody.AddForce ( projectileInfo.transform.forward * projectileInfo.speed, ForceMode.Impulse);

			playerCameraInfo.playerCameraShake += weapon.weaponKnockback * playerCameraInfo.playerCameraShakeMultiply;

		}
			
		weapon.weaponBullet--;


		yield return new WaitForSeconds (weapon.weaponTimeAttack);

		// After Fire

		weapon.weaponAvailableAttack = true;

	}
	IEnumerator WeaponSpawnLight(float time) {

		weaponFlash.SetActive (true);
		yield return new WaitForSeconds (time);
		weaponFlash.SetActive (false);

	}
	void WeaponSpawnShell() {

		GameObject shell = Instantiate (bulletShell [(int)itemWeapon[(int)weaponSelect].weaponShell], transform.position, transform.rotation);
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

}
