using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsFunction : MonoBehaviour {

	// Objects for the settings
	public Slider sldVolume;
	public Slider sldResolution;
	public Toggle tglFullscreen;

	// display of the value for the settings
	public Text strVolume;
	public Text strResolution;

	// resolution of the game
	private int intScreenWidth;
	private int intScreenHeight;

	private Resolution[] arr_resolutions ;

	void Start (){
		arr_resolutions = Screen.resolutions;

		sldResolution.maxValue = arr_resolutions.Length - 1;

		Screen.SetResolution (arr_resolutions [0].width, arr_resolutions [0].height, false);
		strResolution.text = arr_resolutions [0].width + "x" + arr_resolutions [0].height;	
	}

	public void ChangeVolume (){
		AudioListener.volume = sldVolume.value / 100;

		strVolume.text = sldVolume.value.ToString();
	}

	public void ChangeResolution (){

		intScreenWidth = arr_resolutions [(int) sldResolution.value].width;
		intScreenHeight = arr_resolutions [(int) sldResolution.value].height;

		Screen.SetResolution (intScreenWidth, intScreenHeight, tglFullscreen.isOn);

		strResolution.text = intScreenWidth + "x" + intScreenHeight;
	}
}
