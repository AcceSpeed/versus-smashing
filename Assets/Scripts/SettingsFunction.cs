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
	private Resolution[] arr_resolutions ;
	
	private int intPrefScreenWidth;
	private int intPrefScreenHeight;
	private int intResolution;
	private float fltPrefVolume;
	
	private bool blnPrefFullScreen;

	void Awake(){

		arr_resolutions = Screen.resolutions;					// Get all avaliable resolutions
		sldResolution.maxValue = arr_resolutions.Length - 1;	// Set the maximum of slide

		// Get the player prefs
		blnPrefFullScreen = PlayerPrefs.GetInt("blnFullScreen", 0) == 1;

		intPrefScreenWidth = PlayerPrefs.GetInt("intPrefScreenWidth",  arr_resolutions [0].width);
		intPrefScreenHeight = PlayerPrefs.GetInt("intPrefScreenHeight",  arr_resolutions [0].height);

		intResolution = PlayerPrefs.GetInt("intResolution", 0);

		fltPrefVolume = PlayerPrefs.GetFloat("fltPrefVolume", 100);
	}

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start (){

		// set the displayed values for the resolution and volume
		Screen.SetResolution (intPrefScreenWidth, intPrefScreenHeight, blnPrefFullScreen);

		txtResolution.text = intPrefScreenWidth + "x" + intPrefScreenHeight;
		sldResolution.value = intResolution;

		if(blnPrefFullScreen){
			tglFullscreen.isOn = true;
		}
	}

	// *******************************************************************
	// Nom : ChangeVolume
	// But : change the volume
	// Retour: Void
	// Param.: None
	// ******************************************************************* 
	public void ChangeVolume (){

		// change the global volume
		fltPrefVolume = sldVolume.value / 100;

		txtVolume.text = fltPrefVolume.ToString();
	}

	// *******************************************************************
	// Nom : ChangeResolution
	// But : change the resolution
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public void ChangeResolution (){
		// change the values of the parameters
		intResolution = (int) sldResolution.value;

		intPrefScreenWidth = arr_resolutions [intResolution].width;
		intPrefScreenHeight = arr_resolutions [intResolution].height;

		blnPrefFullScreen = tglFullscreen.isOn;

		// change the label value
		txtResolution.text = intPrefScreenWidth + "x" + intPrefScreenHeight;
	}

	public void ApplyAndSave(){
		// set the resolution and the volume
		Screen.SetResolution (intPrefScreenWidth, intPrefScreenHeight, blnPrefFullScreen);
		AudioListener.volume = fltPrefVolume;

		//Set the Player Prefs
		PlayerPrefs.SetInt("intPrefScreenWidth",  intPrefScreenWidth);
		PlayerPrefs.SetInt("intPrefScreenHeight",  intPrefScreenHeight);

		PlayerPrefs.SetInt("blnFullScreen", (blnPrefFullScreen ? 1 : 0));
		
		PlayerPrefs.SetFloat("fltPrefVolume", fltPrefVolume);

		PlayerPrefs.SetInt("intResolution", intResolution);

		// save the changes
		PlayerPrefs.Save();
	}
}
