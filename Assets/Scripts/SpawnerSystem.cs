using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
	public float waveWaitTime = 15f;

	public float waveDay;

	public bool waveBoss;
	public bool waveBossDead;
	public bool waveBossActive;

	public Spanwer[] spawners;

	public Text textWave;
	public Text textWaveZombie;

	public GameObject lightObject;
	public Light lightInfo;
	public Quaternion lightNext;

	public float lightIntensityNext;
	public float lightAmbientIntensityNext;
	public float lightReflectionIntensityNext;

	public GameObject player;
	public GameObject boxRandom;
	public GameObject boxAmmo;

	public int boxAmount;

	// ========== ========== ========== UNITY FUNCTION ========== ========== ========== \\

	void Start() {

		WaveNext ();

	}

	void Update() {

		if (!waveWait) {

			if (waveZombieNumberCurrent == waveZombie && waveZombieNumberLeft == 0) {

				if (waveBossDead || !waveBoss) {
					
					for (int i = 0; i < boxAmount; i++) {
						Instantiate (boxRandom, player.transform.position + new Vector3 (Random.Range (-5f, 5f), 20f, Random.Range (-5f, 5f)), Quaternion.Euler(Vector3.zero));
						Instantiate (boxAmmo, player.transform.position + new Vector3 (Random.Range (-5f, 5f), 20f, Random.Range (-5f, 5f)), Quaternion.Euler(Vector3.zero));
					}
					WaveNext ();

				} else if(!waveBossActive && waveBoss) {
					
					waveBossActive = true;
					TextUpdate ();
					spawners [Mathf.FloorToInt (Random.Range (0f, spawners.Length))].SpawnEnemy (1,true);

				}

			}

		} else {
			/*
			lightObject.transform.rotation = Quaternion.Lerp(lightObject.transform.rotation, lightNext, Time.deltaTime);
			RenderSettings.ambientIntensity = Mathf.Lerp (RenderSettings.ambientIntensity, lightAmbientIntensityNext, Time.deltaTime);
			RenderSettings.reflectionIntensity = Mathf.Lerp (RenderSettings.reflectionIntensity, lightReflectionIntensityNext, Time.deltaTime);
			lightInfo.intensity = Mathf.Lerp (lightInfo.intensity, lightIntensityNext, Time.deltaTime);
			*/


		}

	}

	// ========== ========== ========== FUNCTION ========== ========== ========== \\

	public void WaveNext() {

		WaveCalculate (wave++);
		StartCoroutine (WaveWait());

	}
	IEnumerator WaveWait() {

		waveWait = true;
		TextUpdate ();

		lightObject.transform.rotation = lightNext;
		RenderSettings.ambientIntensity = lightAmbientIntensityNext;
		RenderSettings.reflectionIntensity = lightReflectionIntensityNext;
		lightInfo.intensity = lightIntensityNext;

		yield return new WaitForSeconds (waveWaitTime);

		waveWait = false;
		StartCoroutine (WaveSpawn ());
		lightObject.transform.rotation = lightNext;
		RenderSettings.ambientIntensity = lightAmbientIntensityNext;
		RenderSettings.reflectionIntensity = lightReflectionIntensityNext;
		lightInfo.intensity = lightIntensityNext;
		TextUpdate ();

	}

	public void WaveCalculate(int wave) {

		if ((wave + 1) % 10 == 0)
			waveBoss = true;
		else
			waveBoss = false;

		waveBossActive = false;
		waveBossDead = false;

		waveZombie = Mathf.FloorToInt (10f + wave);
		waveZombieHealth = 10f + (wave * 1.5f);
		waveZombieSpeed = Mathf.Min(10f, 2f + (wave * 0.05f));

		waveSpawnDelay = Mathf.Max (1f, 2f - (wave * 0.025f));
		waveSpawnNumber = Mathf.Min(spawners.Length, Mathf.FloorToInt (1 + (wave * 0.1f)));

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

		if (waveZombieNumberLeft > 0) {
			
			waveSpawnAvailable = true;
			StartCoroutine (WaveSpawn ());

		}

	}

	public void TextUpdate() {

		textWave.text = "WAVE : " + wave.ToString ();
		textWaveZombie.color = Color.white;
		if (!waveWait) {

			if (waveBossActive) {
				
				textWaveZombie.text = "BOSS";
				textWaveZombie.color = Color.red;

			}
			else
				textWaveZombie.text = waveZombieNumberCurrent.ToString () + " / " + waveZombieNumber.ToString ();

		}
		else
			textWaveZombie.text = "INCOMING";

	}

}
