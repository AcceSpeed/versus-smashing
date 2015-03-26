﻿//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 18.02.2015
// But : Main Controller class file who manage the entire fight
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
using UnityEngine.UI;
using System.Collections;

public class MainController : MonoBehaviour {

	//Constants

	public const int INT_MAX_ROUNDS = 3;
	
	public const string STR_QUEUE_TYPE_SIMPLE = "Simple";
	public const string STR_QUEUE_TYPE_MATCH = "Match";
	public const string STR_QUEUE_TYPE_TRAIN = "Train";

	//Variables 

	public static Text txtPlayerName;			// Player's Name

	public static int intRound = 1;				// Round we are at
	public static bool blnMatchOver;			// Used when a round is over
	public static string strPlayerName = "";	// Player's name, entered at the launch and stored for use in the Display Rooms

	public static bool blnIsHost;				// Used to know if the current software is a server or a client for that game

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {

		//To avoid loosing informations (like the player's name), the class isn't destroyed on load of another scene
		DontDestroyOnLoad (this);

		blnMatchOver = false;

		//Sets the gravity that will be used for the game
		Physics.gravity = new Vector3(0f,-50f,0f);
	}
}
