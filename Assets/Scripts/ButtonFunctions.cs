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

	// Use this for initialization
	void Start () {
		
		if (Application.loadedLevelName == "MainMenu") {
			UIElements [0].SetActive (true);
			UIElements [1].SetActive (false);
		}
	}

	void Update(){
		if(intTimerStatus == INT_TIMER_REFRESH){
			DisplayRooms();
			intTimerStatus = 0;
		}
		intTimerStatus++;
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void MatchMaking(){
		NetworkManager.FindOpponent();
		LoadLevel();
	}

	private void LoadLevel (){
		Application.LoadLevel ("Arena");
	}

	private void CreateGame (){

	}

	private void DisplayRooms (){
		NetworkManager.RefreshHostList ();

		if (NetworkManager.hostList != null) {
			string count = NetworkManager.hostList.GetLength(0).ToString();
			intButtonMultiple = 0;

			foreach (var host in NetworkManager.hostList) {
				
				roomButtonInstantiate = Instantiate(
					GObjRoomButton
				) as GameObject;
				
				roomButtonInstantiate.transform.SetParent(GObjRoomsContainer.transform, false);
				roomButtonInstantiate.transform.position += Vector3.down * intButtonMultiple * intButtonDecal ;
				roomButtonInstantiate.GetComponentInChildren<Text>().text = host.gameName   ;
					
				intButtonMultiple++;
			}	
		}
	}

	public void ChooseName(){
		MainController.strPlayerName = txtPlayerName.text;
	}
}
