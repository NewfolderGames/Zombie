using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Developer : MonoBehaviour {

	public SpawnerSystem spawner;
	public PlayerEquip playerEquip;
	public Player player;
	public GameObject airstrike;

	public Text devText;
	public Text devDesc;
	public bool devmode;

	void Update() {

		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			
			devmode = !devmode;
			devText.gameObject.SetActive(devmode);
			devDesc.gameObject.SetActive(devmode);

		}

		if (devmode) {

			// Next Wave
			if(Input.GetKeyDown(KeyCode.Alpha1)) spawner.WaveNext();

			// Add Point
			if(Input.GetKeyDown(KeyCode.Alpha2)) { 

				player.playerPoint += 10000;
				player.playerPointTotal += 10000;
				player.TextUpdate ();

			}

			// Add Ammo to Current Weapon
			if (Input.GetKeyDown (KeyCode.Alpha3)) {

				playerEquip.itemSlot [playerEquip.itemSlotNumber].weaponBullet += 250;
				playerEquip.TextUpdate (playerEquip.itemSlot [playerEquip.itemSlotNumber]);

			}

			// Switch Weapon
			if (Input.GetKeyDown (KeyCode.Alpha4)) {

				playerEquip.itemSlot [playerEquip.itemSlotNumber] = playerEquip.WeaponChange (Random.Range(0,playerEquip.weaponModel.Length));
				playerEquip.WeaponSelect (playerEquip.itemSlot [playerEquip.itemSlotNumber]);
				playerEquip.WeaponUpdate (playerEquip.itemSlotNumber, true, true, false);
				playerEquip.TextUpdate (playerEquip.itemSlot [playerEquip.itemSlotNumber]);

			}

			// Drop Crate
			if (Input.GetKeyDown (KeyCode.Alpha5)) {

				spawner.DropCrate (1);

			}

			if(Input.GetKeyDown(KeyCode.Alpha6)) { 

				player.ChangeHealth (-100);

			}

			if(Input.GetKeyDown(KeyCode.Alpha7)) { 

				Projectile rocket = Instantiate (airstrike, player.mousePosition + Vector3.up * 50f, Quaternion.Euler(90f, 0f, 0f)).GetComponent<Projectile> ();
				rocket.damage = 25f;
				rocket.knockback = 250f;
				rocket.range = 7.5f;
				rocket.GetComponent<Rigidbody> ().AddForce (Vector3.down * 100f, ForceMode.Impulse);

			}

			if (Input.GetKeyDown (KeyCode.Alpha8)) {

				var weapon = playerEquip.itemSlot [playerEquip.itemSlotNumber];
				weapon.weaponName = "Operation Metro in a Nutshell";
				weapon.weaponBullet = 999;
				weapon.weaponDamage = 10f;
				weapon.weaponPallet = 3;
				weapon.weaponKnockback = 250f;
				weapon.weaponLaserpoint = true;
				weapon.weaponModel = playerEquip.weaponModel [7];
				weapon.weaponPoint = new Vector3 (0f, 0f, 0.405f);
				weapon.weaponPosition = new Vector3 (0f, -0.25f, 0f);
				weapon.weaponZoom = true;
				weapon.weaponSemiauto = false;
				weapon.weaponShell = 1;
				weapon.weaponProjectile = 1;
				weapon.weaponRange = 5f;
				weapon.weaponSpreadMin = 0f;
				weapon.weaponSpreadMax = 12.5f;
				weapon.weaponTimeAttack = 0.25f;
				weapon.weaponRecoil = 1f;

			}

			if (Input.GetKeyDown (KeyCode.Alpha9)) {

				var weapon = playerEquip.itemSlot [playerEquip.itemSlotNumber];
				weapon.weaponName = "Fully Automatic Machine Shotgun";
				weapon.weaponBullet = 999;
				weapon.weaponDamage = 1f;
				weapon.weaponPallet = 12;
				weapon.weaponKnockback = 1f;
				weapon.weaponLaserpoint = true;
				weapon.weaponModel = playerEquip.weaponModel [9];
				weapon.weaponPoint = new Vector3 (-0.025f, -0.055f, 0.95f);
				weapon.weaponPosition = new Vector3 (0f, -0.45f, 0f);
				weapon.weaponZoom = true;
				weapon.weaponSemiauto = false;
				weapon.weaponShell = 1;
				weapon.weaponProjectile = 0;
				weapon.weaponRange = 0f;
				weapon.weaponSpreadMin = 0f;
				weapon.weaponSpreadMax = 7.5f;
				weapon.weaponTimeAttack = 3f / 60f;
				weapon.weaponRecoil = 0.1f;

			}

			if (Input.GetKeyDown (KeyCode.Alpha0)) {

				var weapon = playerEquip.itemSlot [playerEquip.itemSlotNumber];
				weapon.weaponName = "APOCALYPSE LAUNCHER";
				weapon.weaponBullet = 999;
				weapon.weaponDamage = 999f;
				weapon.weaponPallet = 1;
				weapon.weaponKnockback = 1000f;
				weapon.weaponLaserpoint = true;
				weapon.weaponModel = playerEquip.weaponModel [8];
				weapon.weaponPoint = new Vector3 (-0.0125f, 0.125f, 1.05f);
				weapon.weaponPosition = new Vector3 (0f, -0.4f, -0.3f);
				weapon.weaponZoom = true;
				weapon.weaponSemiauto = true;
				weapon.weaponShell = 0;
				weapon.weaponProjectile = 2;
				weapon.weaponRange = 12.5f;
				weapon.weaponSpreadMin = 0f;
				weapon.weaponSpreadMax = 0f;
				weapon.weaponTimeAttack = 0.125f;
				weapon.weaponRecoil = 7.5f;

			}

		}

	}

}
