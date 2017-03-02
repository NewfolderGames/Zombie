using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class SpawnerSystem : MonoBehaviour {

	// ========== ========== ========== VARIABLE SETTING ========== ========== ========== \\

	public int wave = 0;

	public int waveZombie;
	public int waveZombieNumber = 10;
	public int waveZombieNumberLeft;
	public int waveZombieNumberCurrent;
	public float waveZombieHealth = 10f;
	public float waveZombieSpeed = 2f;

	public float waveSpawnDelay = 2.5f;
	public int waveSpawnNumber = 1;
	public bool waveSpawnAvailable = true;

	public bool waveWait = true;
	public float waveWaitTime = 20f;

	public float waveDay;

	public bool waveBoss;
	public bool waveBossDead;
	public bool waveBossActive;

	public Spanwer[] spawners;

	public Text textWave;
	public Text textWaveZombie;
	public Text textWaveSkip;

	public GameObject lightObject;
	public Light lightInfo;
	public Quaternion lightNext;

	public float lightIntensityNext;
	public float lightAmbientIntensityNext;
	public float lightReflectionIntensityNext;

	public GameObject player;
	public GameObject boxRandom;
	public GameObject boxAmmo;
	public GameObject boxFlare;
	public AudioClip boxSound;

	public AudioSource waveSound;
	public AudioClip waveSoundFinish;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start() {

		WaveNext ();

	}

	void Update() {

		if (!waveWait) {

			if (waveZombieNumberCurrent >= waveZombie && waveZombieNumberLeft <= 0) {

				if (waveBossDead || !waveBoss) {

					boxFlare.transform.position = player.transform.position;
					boxFlare.SetActive (true);
					DropCrate (1 + wave / 10);
					WaveNext ();

				} else if(!waveBossActive && waveBoss) {
					
					waveBossActive = true;
					TextUpdate ();
					spawners [Mathf.FloorToInt (Random.Range (0f, spawners.Length))].SpawnEnemy (1,true);

				}

			}

		} else {

			if (Input.GetKeyDown (KeyCode.Space))
				WaveWaitFinish ();

		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void WaveNext() {

		WaveCalculate (wave++);
		StartCoroutine (WaveWait());
		waveSound.PlayOneShot (waveSoundFinish);

		Player playerInfo = player.GetComponent<Player> ();
		playerInfo.playerHealth += 5f;
		playerInfo.playerHealth = Mathf.Clamp (playerInfo.playerHealth, 0f, 100f);
		playerInfo.TextUpdate ();

	}
	IEnumerator WaveWait() {

		waveWait = true;
		TextUpdate ();

		lightObject.transform.rotation = lightNext;
		RenderSettings.ambientIntensity = lightAmbientIntensityNext;
		RenderSettings.reflectionIntensity = lightReflectionIntensityNext;
		lightInfo.intensity = lightIntensityNext;

		yield return new WaitForSeconds (waveWaitTime);

		if (waveWait)
			WaveWaitFinish ();

	}

	public void WaveCalculate(int wave) {

		if ((wave + 1) % 10 == 0)
			waveBoss = true;
		else
			waveBoss = false;

		waveBossActive = false;
		waveBossDead = false;

		waveZombie = Mathf.FloorToInt (10f + wave);
		waveZombieHealth = Mathf.Min( 50f, 10f + (wave * 1.5f));
		waveZombieSpeed = Mathf.Min(10f, 2f + (wave * 0.1f));

		waveSpawnDelay = Mathf.Max (1f, 2f - (wave * 0.025f));
		waveSpawnNumber = Mathf.Min(spawners.Length - 1, Mathf.FloorToInt (1 + (wave * 0.1f)));

		waveZombieNumber = waveZombie;
		waveZombieNumberLeft = waveZombieNumber;
		waveZombieNumberCurrent = 0;

		lightNext = Quaternion.Euler (new Vector3 (30f - ((75f / waveDay ) * (wave % waveDay)), -45f, 0f));
		lightAmbientIntensityNext = ((waveDay - 1f) - (wave % waveDay)) / (waveDay - 1f);
		lightReflectionIntensityNext = ((waveDay - 1f) - (wave % waveDay)) / (waveDay - 1f);
		lightIntensityNext = ((waveDay - 1f) - (wave % waveDay)) / (waveDay - 1f);

		TextUpdate ();

	}

	IEnumerator WaveSpawn() {

		waveSpawnAvailable = false;

		yield return new WaitForSeconds (waveSpawnDelay);

		float chance = spawners.Length;
		int number = waveSpawnNumber;
		while (number > 0) {
			
			for (int i = 0; i < spawners.Length; i++) {

				if (Mathf.RoundToInt (Random.Range (1f, chance)) == 1) {

					chance--;
					if (number > 0 && waveZombieNumberLeft > 0) {

						number--;
						waveZombieNumberLeft--;
						spawners [i].SpawnEnemy (0,false);
						if (waveZombieNumberLeft == 0 || number == 0)
							break;

					} else
						break;

				} else
					chance--;

			}
			if (waveZombieNumberLeft == 0 || number == 0)
				break;
			
		}

		if (waveZombieNumberLeft > 0 && !waveWait) {
			
			waveSpawnAvailable = true;
			StartCoroutine (WaveSpawn ());

		}

	}

	public void WaveWaitFinish() {

		waveWait = false;
		StartCoroutine (WaveSpawn ());
		lightObject.transform.rotation = lightNext;
		RenderSettings.ambientIntensity = lightAmbientIntensityNext;
		RenderSettings.reflectionIntensity = lightReflectionIntensityNext;
		lightInfo.intensity = lightIntensityNext;
		boxFlare.SetActive (false);
		TextUpdate ();

	}

	public void TextUpdate() {

		textWave.text = "WAVE : " + wave.ToString ();
		textWaveZombie.color = Color.white;
		if (!waveWait) {

			textWaveSkip.gameObject.SetActive (false);
			if (waveBossActive) {
				
				textWaveZombie.text = "BOSS";
				textWaveZombie.color = Color.red;

			} else
				textWaveZombie.text = waveZombieNumberCurrent.ToString () + " / " + waveZombieNumber.ToString ();

		} else {
			
			textWaveZombie.text = "INCOMING";
			textWaveSkip.gameObject.SetActive (true);

		}
	
	}

	public void DropCrate(int number) {

		waveSound.PlayOneShot (boxSound);
		for (int i = 0; i < number; i++) {
			Instantiate (boxRandom, player.transform.position + new Vector3 (Random.Range (-5f, 5f), 50f, Random.Range (-5f, 5f)), Quaternion.Euler(Vector3.zero));
			Instantiate (boxAmmo, player.transform.position + new Vector3 (Random.Range (-5f, 5f), 50f, Random.Range (-5f, 5f)), Quaternion.Euler (Vector3.zero));
		}

	}

}
