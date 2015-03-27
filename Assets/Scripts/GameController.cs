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
	public GameObject textPrefab;					// Prefab for the text

	public GameObject GameUI;						// GameObject containing the game UI

	public static NetworkViewID NetViewPlayer1;		// NetworkView of the first fighter
	public static NetworkViewID NetViewPlayer2;		// NetworkView of the 2nd fighter 

	public static GameObject player1;				// GameObject of the first player
	public static GameObject player2;				// GameObject of the 2nd player

	public static bool blnResetStage;				// Used to reset the stage once the two softwares are connected
	public static bool blnInPlay;					// if the game is in play

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

		//Sets the gravity that will be used for the game
		Physics.gravity = new Vector3(0f,-50f,0f);

		// Initialisation of variables
		blnResetStage = false;

		sldLifePlayer1.value = 100;
		sldLifePlayer2.value = 100;

		// Set the round displayed in the UI
		txtRoundText.text = MainController.intRound.ToString();

		// If two people are in the arena, do all the start
		if(Network.connections.Length == 1){
			blnInPlay = false;
			StartMatch();
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

		// TODO + remove blnIsHost and replace it by Network.isServer
		/*if(sldLifePlayer1 == 0 || sldLifePlayer2 == 0){
			blnInPlay = false;
		}*/
	}

	// *******************************************************************
	// Nom : SpawnPlayer
	// But : Spwaning the two fighters (in the right position) 
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void SpawnPlayer(bool blnHost){

		// if the player is connected
		if(Network.connections.Length == 1){

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
		else{
			// Set the position and rotation
			playerSpawnPosition = new Vector3 (-20f, 5.5f, 0f);
			playerSpawnRotation.eulerAngles = new Vector3 (0, 90, 0);
			
			// Instantiate the prefab out of network
			player1 = Instantiate (
				playerPrefabSusan,
				playerSpawnPosition,
				playerSpawnRotation
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

		// display for the current round
		yield return StartCoroutine(DisplayTextFadeOut("Round " + MainController.intRound.ToString()));

		// change the text and let the players fight
		blnInPlay = true;
		yield return StartCoroutine(DisplayTextFadeOut("Start !"));
	}
	
	// *******************************************************************
	// Nom : DisplayText
	// But : Instantiate a text from a prefab
	// Retour: void
	// Param.: [string] strTextToDisplay
	// *******************************************************************
	private void DisplayText(string strTextToDisplay){
		
		GameObject gobjGameText;	// GameObject of the text
		Text txtGameText;			// Text displayed on the screen
		
		gobjGameText = Instantiate(
			textPrefab,
			textPrefab.transform.position,
			textPrefab.transform.rotation
			) as GameObject;
		
		gobjGameText.transform.SetParent(GameUI.transform, false);
		
		txtGameText			= gobjGameText.GetComponent<Text>();
		txtGameText.text	= strTextToDisplay;
	}

	// *******************************************************************
	// Nom : DisplayTextFadeOut
	// But : DisplayTextFadeOutCoroutine
	// Retour: void
	// Param.: [string] strTextToDisplay
	// *******************************************************************
	private IEnumerator DisplayTextFadeOut(string strTextToDisplay){
		yield return StartCoroutine(DisplayTextFadeOut(strTextToDisplay, 1.5F));
	}

	// *******************************************************************
	// Nom : DisplayTextFadeOut
	// But : Instantiate a text from a prefab and Fade it out
	// Retour: IEnumerator
	// Param.: [string] strTextToDisplay
	// Param.: [float] fltFadeDuration
	// *******************************************************************
	private IEnumerator DisplayTextFadeOut(string strTextToDisplay, float fltFadeDuration){
		
		GameObject gobjGameText;	// GameObject of the text
		Text txtGameText;			// Text displayed on the screen
		
		gobjGameText = Instantiate(
			textPrefab,
			textPrefab.transform.position,
			textPrefab.transform.rotation
			) as GameObject;
		
		gobjGameText.transform.SetParent(GameUI.transform, false);
		
		txtGameText			= gobjGameText.GetComponent<Text>();
		txtGameText.text	= strTextToDisplay;
		
		// fade out the text
		yield return new WaitForSeconds(1);
		
		txtGameText.CrossFadeAlpha(0,fltFadeDuration,true);
		yield return new WaitForSeconds(fltFadeDuration);
		
		GameObject.DestroyObject(txtGameText.gameObject);
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