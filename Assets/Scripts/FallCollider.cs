//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 18.02.15
// But : Player and animations controller script
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

public class FallCollider : MonoBehaviour {

	void Start(){
	}

	// When object in contact
	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
		MainController.blnMatchOver = true;
	}
}
