using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using System;

public class GameManager : MonoBehaviour {
	public static GameManager main;
	public static Transform mainCamera;

	public float respawnDelay = 2f;

	int health;
	public int kills = -1;
	public int maxEnemies = 100;

	public Text mainText;
	public Text[] playerTexts;

	WaitForSeconds delay;
	
	private void Awake() {
		if (main != null && main != this) {
			Destroy(gameObject);
			return;
		}
		main = this;
		mainCamera = Camera.main.transform;
		delay = new WaitForSeconds(respawnDelay);
		StartCoroutine(GetReady(0));
	}

	public void KillUI() {
		kills += 1;
		playerTexts[1].text = kills + "/" + maxEnemies;
		if (kills >= maxEnemies)
			mainText.text = "Victory!";
	}

	public void HealthUI(float playerHealth) {
		health = (int)playerHealth;
		playerTexts[0].text = "HP: " + health;
	}

	public void RestartUI() {
		StartCoroutine(GetReady(kills));
	}

	IEnumerator GetReady(int lastKills) {
		mainCamera.transform.position = new Vector3(0, 10, 0);
		mainText.text = "Get Ready";
		yield return delay;
		mainText.text = "";
		kills -= (lastKills +1);
		KillUI();
		HealthUI(100);

	}
}
