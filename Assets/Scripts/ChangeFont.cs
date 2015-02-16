using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**********************************************************
 Définition de la classe de changement de la police de tout le jeu.
 ***********************************************************/
public class ChangeFont : MonoBehaviour {
	
	public Font fontFont;
	private Text[] arr_textTexts;

	// Use this for initialization
	void Start () {

		arr_textTexts = GetComponentsInChildren<Text> (true);	// tableau des textes

		// change la police de tous les textes si spécifiée
		if(fontFont != null){
			foreach(Text text in arr_textTexts){
				text.font = fontFont;
			};
		};
	}
}
