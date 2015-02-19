//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
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

public class FighterController : MonoBehaviour {

	//variables
	Animator anim;
	bool blnGuardUp = false;
	bool blnWalking = false;

	//These variables store the ID of the animation states
	int intJumpID = Animator.StringToHash("Jump");
	int intWalkID = Animator.StringToHash("Walk");
	int intRunID = Animator.StringToHash ("Run");
	int intGuardID = Animator.StringToHash("Guard");
	int intFightID = Animator.StringToHash("FightingIDLE");
	int intLightStrikeID = Animator.StringToHash ("LightStrike");
	int intTauntID = Animator.StringToHash ("Taunt");
	int intHeavyStrikeID = Animator.StringToHash ("HeavyStrike");
	int intHitID = Animator.StringToHash ("Hit");
	int intDeathID = Animator.StringToHash ("Death");
	int intVictoryID = Animator.StringToHash ("Victory");
	int intIDLEID = Animator.StringToHash ("IDLE");
	int intClockID = Animator.StringToHash ("Clock");
	int intYawnID = Animator.StringToHash ("Yawn");
	int intSleepID = Animator.StringToHash ("Sleep");
	int intEntryID = Animator.StringToHash ("Entry");
	int intSpecialID = Animator.StringToHash ("Special");
	int intGrabID = Animator.StringToHash ("Grab");
	int intThrowID = Animator.StringToHash ("Throw");
	int intUltimateGrabID = Animator.StringToHash ("UltGrab");
	int intHoldID = Animator.StringToHash ("Hold");
	int intUltimateStrikeID = Animator.StringToHash ("UltStrike");



	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	
	void Update ()
	{	
		//INPUTS

		//When the up arrow key is pressed, jump
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			anim.SetTrigger (intJumpID);
		}

		//When the taunt key is pressed, taunt
		if(Input.GetKeyDown(KeyCode.P))
		{
			anim.SetTrigger (intTauntID);
		}

		//When the grab key is pressed, try to grab
		if(Input.GetKeyDown(KeyCode.G))
		{
			anim.SetTrigger (intGrabID);
		}

		//When strike key is pressed, strike
		if(Input.GetKeyDown(KeyCode.Q))
		{
			anim.SetTrigger (intLightStrikeID);
		}

		//When the second strike key is pressed, strike
		if(Input.GetKeyDown(KeyCode.W))
		{
			anim.SetTrigger (intHeavyStrikeID);
		}

		//When the special strike key is pressed, special strike
		if(Input.GetKeyDown(KeyCode.E))
		{
			anim.SetTrigger (intSpecialID);
		}

		//When the ultimate key is pressed, ultimate (starting with a grab animation)
		//NEED THE HEALTH OF THE ADVERSARY TO BE UNDER 33%
		if(Input.GetKeyDown(KeyCode.R))
		{
			anim.SetTrigger (intUltimateGrabID);
		}

		//GUARDING

		//When the guard key is pressed, guard. On release, get back to IDLE
		if(Input.GetKeyDown(KeyCode.T))
		{
			blnGuardUp = true;
			anim.Play(intGuardID);

		}
		if(Input.GetKeyUp(KeyCode.T) && blnGuardUp==true)
		{
			blnGuardUp = false;
			anim.Play(intFightID);
			
		}

		//WALKING

		//When the walk key is pressed, walk. On release, get back to IDLE
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			blnWalking = true;
			anim.Play(intWalkID);
			
		}
		if(Input.GetKeyUp(KeyCode.RightArrow) && blnWalking==true)
		{
			blnWalking = false;
			anim.Play(intFightID);
			
		}

		//When the walk key is pressed, walk. On release, get back to IDLE
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			blnWalking = true;
			anim.Play(intWalkID);
			
		}
		if(Input.GetKeyUp(KeyCode.LeftArrow) && blnWalking==true)
		{
			blnWalking = false;
			anim.Play(intFightID);
			
		}

		//INPUT-NEEDLESS ANIMATIONS



		//Debug on the current animation state
		AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
		Debug.Log (animStateInfo.nameHash);

	}
}


// *******************************************************************
// Nom : InitGame
// But : Initialiser le jeu
// Retour: 0 si l initialisation s est deroulee correctement
// 1 dans le cas contraire
// Param.: intNbPlayers (in) Entier qui donne le nombre de joueurs
// intNbArmsOctopus (ref) Entier qui donne le nombre de bras
// de la pieuvre
// intCrustaceanAverage (out) Float qui retourne la moyenne de
// crustaces
// *******************************************************************