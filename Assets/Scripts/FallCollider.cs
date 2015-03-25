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
		Network.Destroy(other.gameObject);
	}
}
