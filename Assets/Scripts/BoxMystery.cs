using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMystery : MonoBehaviour {

	public GameObject player;
	public Player playerInfo;
	public PlayerEquip playerWeapon;

	public int boxCost;

	void Start() {

		player = GameObject.Find ("Player");
		playerInfo = player.GetComponent<Player> ();
		playerWeapon = GameObject.Find ("Player_Equip").GetComponent<PlayerEquip> ();

	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.E)) {

			if (Vector3.Distance (player.transform.position, transform.position) <= 5) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit rayHit;

				float rayLenght = 500f;

				int layerMask = LayerMask.GetMask ("BoxMystery");

				if( Physics.Raycast( ray, out rayHit, rayLenght, layerMask ) ) {

					if (playerInfo.playerPoint >= boxCost) {

						playerWeapon.itemSlot [playerWeapon.itemSlotNumber] = playerWeapon.WeaponChange ((int)Random.Range (0f, 6f));
						playerWeapon.WeaponSelect (playerWeapon.itemSlot [playerWeapon.itemSlotNumber]);
						playerInfo.playerPoint -= boxCost;
						playerInfo.TextUpdate ();

					} else
						Debug.Log ("돈이 부족합니다");

				}
					
			}

		}

	}

}
