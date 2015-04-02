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

	public static FontManager fontManager;	// instance of the FontManager
	
	public static FontManager instance{
		get{
			if(fontManager == null){
				fontManager = GameObject.FindObjectOfType<FontManager>();

				// redundant if the script remains in the MainController
				DontDestroyOnLoad (fontManager.gameObject);
			}
			
			return fontManager;
		}
	}

	// *******************************************************************
	// Function called at the instantiation of the class (before Start())
	// *******************************************************************
	void Awake(){
		if(fontManager == null){
			// if it's the first instance, put it in fontManager
			fontManager = this;
			DontDestroyOnLoad(this);
			
		}
		else{
			// if an instance already exists, destroy this one
			if(this != fontManager){
				Destroy(this.gameObject);
			}
		}
	}

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start() {
		ChangeAllFonts();
	}

	// *******************************************************************
	// Function called when a level is loaded (don't include the one where this script was instantiated)
	// *******************************************************************
	void OnLevelWasLoaded(int level) {
		ChangeAllFonts();
	}

	// *******************************************************************
	/// Nom : ChangeAllFonts
	/// But : Get all the texts in the level and change the font
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	void ChangeAllFonts (){
		//Array of the texts
		arr_textTexts = GameObject.FindGameObjectWithTag("UI").GetComponentsInChildren<Text> (true);
		
		//Change the font of all the texts if specified
		if(fontFont != null){
			foreach(Text text in arr_textTexts){
				text.font = fontFont;
			};
		};
	}

	// *******************************************************************
	/// Nom : ChangeFont
	/// But : Get all the texts in the level and change the font
	/// Retour: Void
	/// Param.: None
	// *******************************************************************
	public void ChangeFont (Text txtText){

		//Change the font of all the texts if specified
		if(fontFont != null){
			txtText.font = fontFont;
		}
	}
}
