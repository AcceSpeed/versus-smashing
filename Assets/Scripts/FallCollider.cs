//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 18.02.15
// But : Death Zone behaviour script
//*********************************************************
using UnityEngine;
using System.Collections;

public class FallCollider : MonoBehaviour {

	// When an object enters in contact with the zone, it's destroyed
	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
		//As only a player-controlled character is supposed to fall, and would die doing so, the round is over 
		MainController.blnMatchOver = true;
	}
}
