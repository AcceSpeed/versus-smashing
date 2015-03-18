//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 16.02.2015
// But : option class file
//*********************************************************

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsFunction : MonoBehaviour {

	// Objects for the settings
	public Slider sldVolume;
	public Slider sldResolution;
	public Toggle tglFullscreen;

	// display of the value for the settings
	public Text txtVolume;
	public Text txtResolution;

	// resolution of the game
	private int intScreenWidth;
	private int intScreenHeight;
	private Resolution[] arr_resolutions ;


	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start (){
		arr_resolutions = Screen.resolutions;					// Get all avaliable resolutions
		sldResolution.maxValue = arr_resolutions.Length - 1;	// Set the maximum of slide

		// default resolution to minimal
		Screen.SetResolution (arr_resolutions [0].width, arr_resolutions [0].height, false);
		txtResolution.text = arr_resolutions [0].width + "x" + arr_resolutions [0].height;	
	}

	// *******************************************************************
	// Nom : ChangeVolume
	// But : change the volume
	// Retour: Void
	// Param.: None
	// ******************************************************************* 
	public void ChangeVolume (){

		// change the global volume
		AudioListener.volume = sldVolume.value / 100;

		txtVolume.text = sldVolume.value.ToString();
	}

	// *******************************************************************
	// Nom : ChangeResolution
	// But : change the resolution
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public void ChangeResolution (){

		intScreenWidth = arr_resolutions [(int) sldResolution.value].width;
		intScreenHeight = arr_resolutions [(int) sldResolution.value].height;

		// set the resolution
		Screen.SetResolution (intScreenWidth, intScreenHeight, tglFullscreen.isOn);

		// change the label value
		txtResolution.text = intScreenWidth + "x" + intScreenHeight;
	}
}
