//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 05.03.15
// But : Script handling the start of a match and all the informations linked to it
//*********************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLoader : MonoBehaviour {
	

	//Variables
	public GameObject playerPrefabSusan;			//Prefab of the fighter

	public static NetworkViewID NetViewPlayer1;		//NetworkView of the first fighter
	public static NetworkViewID NetViewPlayer2;		//NetworkView of the 2nd fighter 

	public Slider sldLifePlayer1;	// life of player 1
	public Slider sldLifePlayer2;	// life of player 2

	private Vector3 playerSpawnPosition;			//Spawn position
	private Quaternion playerSpawnRotation;			//Spawn rotation

	private GameObject[] playersToDestroy;			//Used to get all the objects that will be destroyed upon start

	public static bool blnResetStage = false;		//Used to reset the stage once the two softwares are connected

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start(){
		sldLifePlayer1.value = 100;
		sldLifePlayer2.value = 100;

		SpawnPlayer (MainController.blnIsHost);
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update(){
		if (blnResetStage) {
			ResetStage();
		}

		// Update the life of the players 
		if(MainController.blnIsHost){
			sldLifePlayer1.value = FighterController.intHealthSelf;
			sldLifePlayer2.value = FighterController.intHealthOpponent;
		}
		else{
			sldLifePlayer2.value = FighterController.intHealthSelf;
			sldLifePlayer1.value = FighterController.intHealthOpponent;
		}
	}

	// *******************************************************************
	// Nom : SpawnPlayer
	// But : Spwaning the two fighters (in the right position) 
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void SpawnPlayer(bool blnHost){

		//In the case where the software is the one considered as the server, spawns his character facing the right, on the left part of the stage
		if (blnHost) {

			//Set the position and rotation
			playerSpawnPosition = new Vector3 (-20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, 90, 0);

			//Instantiate the prefab
			GameObject player1 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;

			NetViewPlayer1 = player1.networkView.viewID;


		//In the case where the software is the one considered as the client, spawns his character facing the left, on the right part of the stage
		} else {

			//Set the position and rotation
			playerSpawnPosition = new Vector3 (20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, -90, 0);

			//Instantiate the prefab
			GameObject player2 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;

			NetViewPlayer2 = player2.networkView.viewID;
		}

	}

	// *******************************************************************
	// Nom : ResetStage
	// But : Resetting the stage once both of the softwares are ready and connected 
	//(since the first one, aka the server, will have his game starting as a pratice right at the start)
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void ResetStage(){
		blnResetStage = false;

		//Finds all the objects tagged with "Player"
		playersToDestroy = GameObject.FindGameObjectsWithTag ("Player");

		//Destroys them
		foreach (GameObject player in playersToDestroy) {
			Destroy(player);
		}

		//Spawns the two fighters
		SpawnPlayer (MainController.blnIsHost);
	}
}