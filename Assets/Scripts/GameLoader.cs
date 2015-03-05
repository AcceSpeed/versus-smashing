using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	public GameObject playerPrefabSusan;

	private Vector3 playerSpawnPosition;
	private Quaternion playerSpawnRotation;

	void Start(){
		SpawnPlayer (MainController.blnIsHost);
	}

	private void SpawnPlayer(bool blnHost){
		
		if (blnHost) {
			playerSpawnPosition = new Vector3 (-20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, 90, 0);
		} else {
			playerSpawnPosition = new Vector3 (20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, -90, 0);
		}
		
		Network.Instantiate (playerPrefabSusan, playerSpawnPosition, playerSpawnRotation, 0);
	}
}
