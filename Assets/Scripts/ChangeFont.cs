//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 16.02.15
// But : Font management script
//*********************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**********************************************************
 Définition de la classe de changement de la police de tout le jeu.
 ***********************************************************/
public class ChangeFont : MonoBehaviour {
	
	public Font fontFont;
	private Text[] arr_textTexts;

	//
	//TODO: include the prefab texts
	//

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {

		//Array of the texts
		arr_textTexts = GetComponentsInChildren<Text> (true);


		//Change the font of all the texts if specified
		if(fontFont != null){
			foreach(Text text in arr_textTexts){
				text.font = fontFont;
			};
		};
	}
}
