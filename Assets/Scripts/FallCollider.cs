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
