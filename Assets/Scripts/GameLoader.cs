using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	public GameObject playerPrefabSusan;

	private Vector3 playerSpawnPosition;
	private Quaternion playerSpawnRotation;

	private GameObject[] playersToDestroy;

	public static bool blnResetStage = false;

	void Start(){
		SpawnPlayer (MainController.blnIsHost);
	}

	void Update(){
		if (blnResetStage) {
			ResetStage();
		}
	}

	private void SpawnPlayer(bool blnHost){
		
		if (blnHost) {
			playerSpawnPosition = new Vector3 (-20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, 90, 0);
		} else {
			playerSpawnPosition = new Vector3 (20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, -90, 0);
		}

		Network.Instantiate (playerPrefabSusan, playerSpawnPosition, playerSpawnRotation, 1);
	}

	private void ResetStage(){
		blnResetStage = false;

		playersToDestroy = GameObject.FindGameObjectsWithTag ("Player");
		
		foreach (GameObject player in playersToDestroy) {
			Destroy(player);
		}
		
		SpawnPlayer (MainController.blnIsHost);
	}
}
