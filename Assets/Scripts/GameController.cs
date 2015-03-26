//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 05.03.15
// But : Script handling the start of a match and all the informations linked to it
//*********************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	

	//Variables
	public GameObject playerPrefabSusan;			// Prefab of the fighter

	public static NetworkViewID NetViewPlayer1;		// NetworkView of the first fighter
	public static NetworkViewID NetViewPlayer2;		// NetworkView of the 2nd fighter 

	public static GameObject player1;				// GameObject of the first player
	public static GameObject player2;				// GameObject of the 2nd player

	public static bool blnResetStage;				// Used to reset the stage once the two softwares are connected
	public static bool blnInPlay;					// if the game is in play
	
	public Text txtGameText;						// Text displayed on the screen
	public Text txtRoundText;						// Round we are at

	public Slider sldLifePlayer1;					// life of player 1
	public Slider sldLifePlayer2;					// life of player 2

	private Vector3 playerSpawnPosition;			// Spawn position
	private Quaternion playerSpawnRotation;			// Spawn rotation

	private GameObject[] playersToDestroy;			// Used to get all the objects that will be destroyed upon start

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start(){

		// Initialisation of variables
		blnResetStage = false;

		sldLifePlayer1.value = 100;
		sldLifePlayer2.value = 100;

		// Set the round displayed in the UI
		txtRoundText.text = MainController.intRound.ToString();

		// If two people are in the arena, do all the start
		if(Network.connections.Length == 1){
			blnInPlay = false;
			StartCoroutine("StartMatch");
		}
		else{
			blnInPlay = true;
		}

		// Spawn the player
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

		// In the case where the software is the one considered as the server
		// spawns his character facing the right, on the left part of the stage
		if (blnHost) {

			// Set the position and rotation
			playerSpawnPosition = new Vector3 (-20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, 90, 0);

			// Instantiate the prefab
			player1 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;

		// In the case where the software is the one considered as the client
		// spawns his character facing the left, on the right part of the stage
		} else {

			// Set the position and rotation
			playerSpawnPosition = new Vector3 (20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, -90, 0);

			// Instantiate the prefab
			player2 = Network.Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation,
				1
			) as GameObject;
		}
	}
	
	// *******************************************************************
	// Nom : StartMatch
	// But : Handle the management of the UI for the start of a match
	// Retour: IEnumerator
	// Param.: None
	// *******************************************************************
	private IEnumerator StartMatch(){

		// prepare the display for the current round
		txtGameText.text = "Round " + MainController.intRound.ToString();

		yield return new WaitForSeconds(5);

		// show the first text (Round x)
		txtGameText.gameObject.SetActive(true);

		yield return new WaitForSeconds(2);

		// change the text and let the players fight
		blnInPlay = true;
		txtGameText.text = "Start !";

		// fade out the text
		txtGameText.CrossFadeAlpha(0,3,true);
	}

	// *******************************************************************
	// Nom : ResetStage
	// But : Resetting the stage (called once both of the softwares are ready and connected 
	// since the first one, aka the server, will have his game starting as a pratice right at the start)
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void ResetStage(){

		Application.LoadLevel("Arena");
	}
}