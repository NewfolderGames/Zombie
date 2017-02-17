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
	int changeNumberMax = 50;
	float changeNumberTime = 5f;

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

	public bool boxRandom;
	public bool boxAmmo;

	void Start() {

		player = GameObject.Find ("Player");
		playerInfo = player.GetComponent<Player> ();
		playerWeapon = GameObject.Find ("Player_Equip").GetComponent<PlayerEquip> ();

		if (boxRandom) {

			boxAmmo = false;
			box = (boxType)Mathf.RoundToInt (Random.Range (0f, 3f));
			boxCost = Mathf.RoundToInt (Random.Range(100f, 1000f));
			switch ((int)box) {

			case 0:
				boxLightInfo.color = Color.yellow;
				break;
			case 1:
				boxLightInfo.color = new Color (1f, 1f / 2f, 0f);
				break;
			case 2:
				boxLightInfo.color = new Color (0f, 3f / 4f, 1f);
				break;
			case 3:
				boxLightInfo.color = Color.red;
				break;

			}
			Destroy (gameObject, 25f);

		}
		if (boxAmmo) {

			boxRandom = false;
			boxCost = Mathf.RoundToInt (Random.Range(75f, 150f));
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
									break;

								case 1:
									playerWeapon.weaponDamageAdd [weaponNumber] *= 1.25f;
									for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
										if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
											playerWeapon.itemSlot [i].weaponDamage *= 1.25f;
									}
									break;

								case 2:

									switch (weaponNumber) {

									case 7:
									case 8:
										playerWeapon.weaponClipAdd [weaponNumber]++;
										for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
											if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
												playerWeapon.itemSlot [i].weaponBullet++;
										}
										break;

									default:
										playerWeapon.weaponClipAdd [weaponNumber] *= 1.25f;
										for (int i = 0; i < playerWeapon.itemSlot.Length; i++) {
											if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
												playerWeapon.itemSlot [i].weaponBullet = Mathf.RoundToInt (playerWeapon.itemSlot [i].weaponBullet * 1.25f);
										}
										break;

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
									if (weaponNumber == playerWeapon.itemSlot [i].weaponNumber)
										playerWeapon.itemSlot [i].weaponBullet += Mathf.RoundToInt(playerWeapon.itemSlot [i].weaponClip * 0.25f);

							}
							playerWeapon.TextUpdate (playerWeapon.itemSlot [playerWeapon.itemSlotNumber]);
							boxWeapon.SetActive (false);
							boxLight.SetActive (false);
							availableGet = false;

							if (boxRandom || boxAmmo) {
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
		if (!boxAmmo)
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
