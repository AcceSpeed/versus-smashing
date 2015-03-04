//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 13.02.2015
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
using System.Collections;

public class MainController : MonoBehaviour {

	public static bool blnMatchOver;
	public static string strPlayerName;

	// Use this for initialization
	void Start () {
		blnMatchOver = false;

		Physics.gravity = new Vector3(0f,-50f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (strPlayerName);
	}
}
