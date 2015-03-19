//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 20.02.15
// But : Camera behaviour controller script (Not used yet)
//*********************************************************
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//variables
	private GameObject[] arr_objPlayers;

	private Transform tranCameraDefault ;	
	public int HeightDecal;

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {
		tranCameraDefault = this.transform;
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update () {

		arr_objPlayers = GameObject.FindGameObjectsWithTag("Player");

		switch (arr_objPlayers.Length) {
			case 1:
				transform.position = new Vector3(
					arr_objPlayers[0].transform.position.x,
					arr_objPlayers[0].transform.position.y + HeightDecal,
					tranCameraDefault.position.z
				);
				break;
			case 2:
				transform.position = new Vector3(
					(arr_objPlayers[0].transform.position.x + arr_objPlayers[1].transform.position.x) / 2,
					(arr_objPlayers[0].transform.position.y + arr_objPlayers[1].transform.position.y) / 2 + HeightDecal,
					tranCameraDefault.position.z
				);
				break;
			default:
				break;
		}

		if(arr_objPlayers.Length == 1){
			//When the player moves, follows him
			transform.position = new Vector3(
				arr_objPlayers[0].transform.position.x,
				arr_objPlayers[0].transform.position.y + HeightDecal,
				tranCameraDefault.position.z
			);	
		}
	}
}
