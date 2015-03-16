//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 16.02.15
// But : Menu and buttons controller script
//*********************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonFunctions : MonoBehaviour {

	public GameObject GObjRoomsContainer;
	public GameObject GObjRoomButton;
	public GameObject[] UIElements;
	public Text txtPlayerName;

	private GameObject roomButtonInstantiate;
	private int intButtonDecal = 10;
	private int intButtonMultiple = 0;
	private int intTimerStatus = 50;

	private const int INT_TIMER_REFRESH = 50;

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {

		//In the case where the scene loaded is the Main Menu, activates only the second UI part
		if (Application.loadedLevelName == "MainMenu") {
			UIElements [0].SetActive (true);
			UIElements [1].SetActive (false);
		}
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update(){

		//When the Main Menu is loaded, start to refresh the DisplayRooms function every 50 frames (defined by a const.)
		if (Application.loadedLevelName == "MainMenu" && UIElements[1].activeSelf) {
			if(intTimerStatus == INT_TIMER_REFRESH){
				DisplayRooms();
				intTimerStatus = 0;
			}
			intTimerStatus++;
		}
	}

	// *******************************************************************
	// Nom : QuitGame
	// But : Exit the application when the button is pressed
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public void QuitGame(){
		Application.Quit();
	}

	// *******************************************************************
	// Nom : MatchMaking
	// But : Sets up the match by searching for an opponent when the button is pressed
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public void MatchMaking(){
		NetworkManager.FindOpponent();
	}

	// *******************************************************************
	// Nom : CreateGame
	// But : 
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void CreateGame (){

	}

	// *******************************************************************
	// Nom : DisplayRooms
	// But : Create buttons corresponding to the active game rooms
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void DisplayRooms (){

		//Gets the host list from the network script
		NetworkManager.RefreshHostList ();

		//If it's not empty, gets its length
		if (NetworkManager.hostList != null) {
			string count = NetworkManager.hostList.GetLength(0).ToString();
			intButtonMultiple = 0;

			//As long as there are hosts in the list, creates buttons with the name of the room's creator on it
			foreach (var host in NetworkManager.hostList) {
				
				roomButtonInstantiate = Instantiate(
					GObjRoomButton
				) as GameObject;

				//Button instantiation with the text
				roomButtonInstantiate.transform.SetParent(GObjRoomsContainer.transform, false);
				roomButtonInstantiate.transform.position += Vector3.down * intButtonMultiple * intButtonDecal ;
				roomButtonInstantiate.GetComponentInChildren<Text>().text = host.gameName   ;
					
				intButtonMultiple++;
			}	
		}
	}

	// *******************************************************************
	// Nom : ChooseName
	// But : Gets the name entered by the player and switch for the main menu
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public void ChooseName(){

		//If the name is not an empty string, store it and activates the second UI part
		if (txtPlayerName.text != "") {
			MainController.strPlayerName = txtPlayerName.text;

			UIElements [0].SetActive (false);
			UIElements [1].SetActive (true);
		}
	}
}
