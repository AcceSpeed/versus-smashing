//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 05.03.15
// But : Player and animations controller script
//*********************************************************
// Modifications:
// Date :
// Auteur :
// Raison :
//*********************************************************
// Date :
// Auteur :
// Raison :
//*********************************************************
using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	public GameObject playerPrefabSusan;

	public static NetworkViewID NetViewPlayer1;
	public static NetworkViewID NetViewPlayer2;

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

			GameObject player1 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;

			NetViewPlayer1 = player1.networkView.viewID;

		} else {
			playerSpawnPosition = new Vector3 (20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, -90, 0);

			GameObject player2 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;

			NetViewPlayer2 = player2.networkView.viewID;
		}

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
