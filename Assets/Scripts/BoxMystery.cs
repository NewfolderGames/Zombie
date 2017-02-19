using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMystery : MonoBehaviour {

	public GameObject player;
	public Player playerInfo;
	public PlayerEquip playerWeapon;

	public GameObject boxLight;
	public GameObject boxWeapon;

	public Light boxLightInfo;

	public MeshFilter mesh;
	public MeshRenderer meshRenderer;

	public Mesh[] modelWeapon;
	public Material[] materialWeapon;

	public bool availableBuy = true;
	public bool availableGet = false;

	int changeNumber = 0;
	int changeNumberMax = 25;
	float changeNumberTime = 2.5f;

	float weaponVanish = 10f;
	float weaponVanishCurrent = 0;

	int weaponNumber;

	public boxType box;
	public enum boxType {

		Box_Weapon,
		Box_Damage,
		Box_Clip,
		Box_Laser

	}

	public int boxCost;

	public bool boxCrate;
	public bool boxAmmo;
	public bool boxRandom;

	public static int boxOpenWeapon;
	public static int boxOpenDamage;
	public static int boxOpenClip;
	public static int boxOpenLaser;

	void Start() {

		player = GameObject.Find ("Player");
		playerInfo = player.GetComponent<Player> ();
		playerWeapon = GameObject.Find ("Player_Equip").GetComponent<PlayerEquip> ();

		if (boxCrate) {

			boxAmmo = false;
			box = (boxType)Mathf.Floor (Random.Range(0f, 4f));
			switch ((int)box) {

			case 0:
				boxLightInfo.color = Color.yellow;
				boxCost = Mathf.RoundToInt (Random.Range (500f, 1000f));
				break;
			case 1:
				boxLightInfo.color = new Color (1f, 1f / 2f, 0f);
				boxCost = Mathf.RoundToInt (Random.Range(350f, 500f));
				break;
			case 2:
				boxLightInfo.color = new Color (0f, 3f / 4f, 1f);
				boxCost = Mathf.RoundToInt (Random.Range(350f, 500f));
				break;
			case 3:
				boxLightInfo.color = Color.red;
				boxCost = Mathf.RoundToInt (Random.Range(350f, 500f));
				break;

			}
			boxRandom = true;
			Destroy (gameObject, 25f);

		}
		if (boxAmmo) {

			boxCrate = false;
			boxCost = Mathf.RoundToInt (Random.Range(150f, 250f));
			boxLightInfo.color = Color.green;
			Destroy (gameObject, 25f);

		}

	}

	void Update () {

		if (Input.GetKeyUp (KeyCode.E)) {

			if (Vector3.Distance (player.transform.position, transform.position) <= 5) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit rayHit;

				float rayLenght = 500f;

				int layerMask = LayerMask.GetMask ("Box");

				if (Physics.Raycast (ray, out rayHit, rayLenght, layerMask)) {

					if (rayHit.collider.gameObject.Equals (gameObject)) {
						
						if (availableBuy && !availableGet) {
							
							if (playerInfo.playerPoint >= boxCost) {
								
								playerInfo.playerPoint -= boxCost;
								playerInfo.TextUpdate ();
								StartCoroutine (WeaponChoose ());
								boxWeapon.SetActive (true);
								boxLight.SetActive (true);
								boxWeapon.transform.localPosition = new Vector3 (0f, 1.5f, 0f);

							} else
								Debug.Log ("돈이 부족합니다");

						} else if (availableGet) {

							if (!boxAmmo) {
								
								switch ((int)box) {

								case 0: 
									playerWeapon.itemSlot [playerWeapon.itemSlotNumber] = playerWeapon.WeaponChange (weaponNumber);
									playerWeapon.WeaponSelect (playerWeapon.itemSlot [playerWeapon.itemSlotNumber]);
									playerWeapon.WeaponUpdate (playerWeapon.itemSlotNumber, true, true, false);
									break;

								case 1:
									playerWeapon.weaponDamageAdd [weaponNumber]++;
									for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
										if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
											playerWeapon.WeaponUpdate (i, true, false, false);
									}
									break;

								case 2:
									playerWeapon.weaponClipAdd [weaponNumber]++;
									for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
										if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
											playerWeapon.WeaponUpdate (i, true, true, true);
									}
									break;

								case 3:
									playerWeapon.weaponLaserAdd [weaponNumber] = true;
									for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
										if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
											playerWeapon.itemSlot [i].weaponLaserpoint = true;
									}
									break;

								}

							} else {

								for (int i = 0; i < playerWeapon.itemSlot.Length; i++)
									if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber) {
										playerWeapon.WeaponUpdate (i, true, true, true);
									}

							}
							playerWeapon.TextUpdate (playerWeapon.itemSlot [playerWeapon.itemSlotNumber]);
							boxWeapon.SetActive (false);
							boxLight.SetActive (false);
							availableGet = false;

							if (boxCrate || boxAmmo) {
								Destroy (gameObject);
							}

						}

					}

				}
					
			}

		}

		if (availableGet) {

			weaponVanishCurrent += Time.deltaTime;
			boxWeapon.transform.localPosition = Vector3.Lerp (new Vector3 (0f, 1.5f, 0f), new Vector3 (0f, 0f, 0f), weaponVanishCurrent / weaponVanish);
			boxLightInfo.intensity = Mathf.Lerp (2, 0, weaponVanishCurrent / weaponVanish);
			if (boxWeapon.transform.localPosition == new Vector3 (0f, 0f, 0f)) {

				weaponVanishCurrent = 0;
				boxWeapon.SetActive (false);
				boxLight.SetActive (false);
				availableGet = false;

			}
				
		}

	}

	IEnumerator WeaponChoose() {
		
		availableBuy = false;
		boxLightInfo.intensity = 2f;
		if (boxRandom)
			weaponNumber = (int)Random.Range (0f, playerWeapon.weaponModel.Length);
		else
			weaponNumber = playerWeapon.itemSlot[Mathf.RoundToInt (Random.Range (0f, playerWeapon.itemSlot.Length - 1))].weaponNumber;

		mesh.mesh = modelWeapon [weaponNumber];
		meshRenderer.material = materialWeapon [weaponNumber];
		changeNumber++;

		yield return new WaitForSeconds (changeNumberTime / changeNumberMax);

		if (changeNumber < changeNumberMax)
			StartCoroutine (WeaponChoose ());
		else {
			
			weaponVanishCurrent = 0;
			changeNumber = 0;
			availableBuy = true;
			availableGet = true;

		}

	}



}
