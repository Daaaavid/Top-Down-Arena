using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
	public static GameManager main;

	public GameObject ballPrefab;

	public float xBound = 3f;
	public float yBound = 3f;
	public float ballSpeed = 3f;
	public float respawnDelay = 2f;
	public int[] playerScores;

	public Text mainText;
	public Text[] playerTexts;

	Entity ballEntityPrefab;
	EntityManager manager;

	WaitForSeconds oneSecond;
	WaitForSeconds delay;

	private void Awake() {
		if(main != null && main != this) {
			Destroy(gameObject);
				return;
		}
		main = this;
		playerScores = new int[2];

		oneSecond = new WaitForSeconds(1f);
		delay = new WaitForSeconds(respawnDelay);

		StartCoroutine(CountDownAndSpwanBall());
	}

	public void PlayerScored(int playerID) {
		playerScores[playerID]++;
		for(int i = 0; i < playerScores.Length && i < playerTexts.Length; i++) {
			playerTexts[i].text = playerScores[i].ToString();
		}
		StartCoroutine(CountDownAndSpwanBall());
	}

	IEnumerator CountDownAndSpwanBall() {
		mainText.text = "Get Ready";
		yield return delay;

		mainText.text = "3";
		yield return oneSecond;

		mainText.text = "2";
		yield return oneSecond;

		mainText.text = "1";
		yield return oneSecond;
	}

}
