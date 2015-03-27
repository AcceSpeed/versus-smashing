//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 16.02.15
// But : Menu and buttons controller script
//*********************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonFunctions : MonoBehaviour {

	// Constants
	private const string STR_UI_NAME_LOGIN		= "Login";
	private const string STR_UI_NAME_MENU		= "Menu";
	private const string STR_UI_NAME_SETTINGS	= "Settings";
	private const string STR_UI_NAME_CREDITS	= "Credits";
	private const string STR_UI_NAME_EXIT		= "ExitConfirm";

	private const int INT_TIMER_REFRESH = 50;

	public Text txtPlayerName;						// Name written by the player
	public GameObject GObjRoomsContainer;			// Container of the rooms
	public GameObject GObjRoomButton;				// Button prefab to instantiate
	public GameObject[] UIElements;					// Array of the UI elements

	private Dictionary<string, int> UINumByName;	// Dictionnary of UInames and index
	private GameObject[] buttonsToDestroy;			//Used to get all the buttons that will be destroyed upon refresh


	private GameObject roomButtonInstantiate;		// Instantiation of a button
	private int intButtonGap = 10;					// Gap between each button
	private int intButtonMultiple = 0;				// Step of the button we are at (for decal)
	private int intTimerStatus = INT_TIMER_REFRESH;	// Increment for
	private string strNameRoomToJoin;				// 


	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {

		//In the case where the scene loaded is the Main Menu, activates only the second UI part
		if (Application.loadedLevelName == "MainMenu") {

			// Instantiate and full in the dictionnary of UI Elements
			UINumByName = new Dictionary<string, int>();

			for (int i = 0; i < UIElements.Length; i++) {
				UINumByName.Add(UIElements[i].name,i);
			}

			ActivateMenu(STR_UI_NAME_LOGIN);
		}
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update(){

		if(GetUIElement(STR_UI_NAME_LOGIN).activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))){
			ChooseName();
		}

		//When the Main Menu is loaded, start to refresh the DisplayRooms function every 50 frames (defined by a const.)
		if (Application.loadedLevelName == "MainMenu" && GetUIElement(STR_UI_NAME_MENU).activeSelf) {
			if(intTimerStatus == INT_TIMER_REFRESH){
				DisplayRooms();
				intTimerStatus = 0;
			}
			intTimerStatus++;
		}
	}

	// *******************************************************************
	/// Nom : QuitGame
	/// But : Exit the application when the button is pressed
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void QuitGame(){
		Application.Quit();
	}

	// *******************************************************************
	/// Nom : MatchMaking
	/// But : Sets up the match by searching for an opponent when the button is pressed
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void MatchMaking(){
		NetworkManager.FindOpponent();
	}

	// *******************************************************************
	/// Nom : CreateGame
	/// But : Create a room for players to join (displayed in the right)
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void CreateGame (){
		NetworkManager.StartServer (MainController.STR_QUEUE_TYPE_SIMPLE);
	}

	// *******************************************************************
	/// Nom : TrainingGame
	/// But : Join a room alone for training
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void TrainingGame (){
		NetworkManager.StartServer (MainController.STR_QUEUE_TYPE_TRAIN);
	}

	// *******************************************************************
	/// Nom : ActiveMenu
	/// But : Activate the UIElement to display (Title always showed)
	/// Retour: Void
	/// Param.: STRING strUIName : Name of the element to activate
	// *******************************************************************
	private void ActivateMenu (string strUIName){
		foreach (GameObject gobjUIElement in UIElements) {
			if(gobjUIElement.name == strUIName){
				gobjUIElement.SetActive(true);
			}
			else{
				gobjUIElement.SetActive(false);
			}
		}
	}

	// *******************************************************************
	/// Nom : GetUIElement
	/// But : Activate the UIElement to display (Title always showed)
	/// Retour: Void
	/// Param.: STRING strUIName : Name of the element to activate
	// *******************************************************************
	private GameObject GetUIElement (string strUIName){
		return UIElements[UINumByName[strUIName]];
	}

	// *******************************************************************
	/// Nom : DisplayRooms
	/// But : Create buttons corresponding to the active game rooms
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	private void DisplayRooms (){

		//Gets the host list from the network script
		NetworkManager.RefreshHostList ();

		//If it's not empty, gets its length
		if (NetworkManager.hostList != null) {
			string count = NetworkManager.hostList.GetLength(0).ToString();
			intButtonMultiple = 0;

			buttonsToDestroy = GameObject.FindGameObjectsWithTag ("RoomButton");
			//Destroys them
			foreach (GameObject button in buttonsToDestroy) {
				Destroy(button);
			}

			//As long as there are hosts in the list, creates buttons with the name of the room's creator on it
			foreach (var host in NetworkManager.hostList) {

				//If, of course, they are not matchmaking rooms or training rooms
				if(host.comment==MainController.STR_QUEUE_TYPE_SIMPLE && host.connectedPlayers < 2){
				
					roomButtonInstantiate = Instantiate(
						GObjRoomButton
					) as GameObject;

					//Button instantiation with the text
					roomButtonInstantiate.transform.SetParent(GObjRoomsContainer.transform, false);
					roomButtonInstantiate.transform.position += Vector3.down * intButtonMultiple * intButtonGap ;
					roomButtonInstantiate.GetComponentInChildren<Text>().text = host.gameName;

					Button roomButton = roomButtonInstantiate.GetComponent<Button>();
					roomButton.onClick.AddListener(() => NetworkManager.JoinServer(host));

					intButtonMultiple++;
				}
			}	
		}
	}


	// *******************************************************************
	/// Nom : ChooseName
	/// But : Gets the name entered by the player and switch for the main menu
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void ChooseName(){
		//If the name is not an empty string, store it and activates the second UI part
		if (txtPlayerName.text != "") {
			MainController.strPlayerName = txtPlayerName.text;

			ActivateMenu(STR_UI_NAME_MENU);
		}
	}
}
