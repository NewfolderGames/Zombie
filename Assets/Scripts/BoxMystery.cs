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

	public int boxCost;

	void Start() {

		player = GameObject.Find ("Player");
		playerInfo = player.GetComponent<Player> ();
		playerWeapon = GameObject.Find ("Player_Equip").GetComponent<PlayerEquip> ();

	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.E)) {

			if (Vector3.Distance (player.transform.position, transform.position) <= 5) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit rayHit;

				float rayLenght = 500f;

				int layerMask = LayerMask.GetMask ("BoxMystery");

				if (Physics.Raycast (ray, out rayHit, rayLenght, layerMask)) {

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

						playerWeapon.itemSlot [playerWeapon.itemSlotNumber] = playerWeapon.WeaponChange (weaponNumber);
						playerWeapon.WeaponSelect (playerWeapon.itemSlot [playerWeapon.itemSlotNumber]);
						boxWeapon.SetActive (false);
						boxLight.SetActive (false);
						availableGet = false;

					}

				}
					
			}

		}

		if (availableGet) {

			weaponVanishCurrent += Time.deltaTime;
			boxWeapon.transform.localPosition = Vector3.Lerp (new Vector3 (0f, 1.5f, 0f), new Vector3 (0f, 0f, 0f), weaponVanishCurrent / weaponVanish);
			boxLightInfo.intensity = Mathf.Lerp (2, 0, weaponVanishCurrent / weaponVanish);
			if(boxWeapon.transform.localPosition == new Vector3(0f, 0f, 0f)){

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
		weaponNumber = (int)Random.Range (0f, playerWeapon.weaponModel.Length);
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
