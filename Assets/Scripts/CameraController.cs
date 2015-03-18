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
	public Transform tranPlayer;
	private Transform tranCamera ;
	private int HeightDecal = 10;

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {
		tranCamera = this.transform;
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update () {

		//When the player moves, follows him
		if (tranPlayer) {
			transform.position = new Vector3(
				tranPlayer.position.x,
				tranPlayer.position.y + HeightDecal,
				tranCamera.position.z
			);	
		}

	}
}
