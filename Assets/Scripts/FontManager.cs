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
public class FontManager : MonoBehaviour {
	
	public Font fontFont;
	private Text[] arr_textTexts;
	
	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start() {
		//To avoid loosing informations (like the player's name), the class isn't destroyed on load of another scene
		DontDestroyOnLoad (this);

		ChangeFont();
	}

	// *******************************************************************
	// Function called when a level is loaded (don't include the one where this script was instantiated)
	// *******************************************************************
	void OnLevelWasLoaded(int level) {
		ChangeFont();
	}

	// *******************************************************************
	/// Nom : ChangeFont
	/// But : Get all the texts in the level and change the font
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	void ChangeFont (){
		//Array of the texts
		arr_textTexts = GameObject.FindGameObjectWithTag("UI").GetComponentsInChildren<Text> (true);
		
		//Change the font of all the texts if specified
		if(fontFont != null){
			foreach(Text text in arr_textTexts){
				text.font = fontFont;
			};
		};
	}
}
