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

	public int intMaximumSpeed;
	public int intJumpForce;

	private float fltRotation = 90;

	private float fltLastSynchronizationTime = 0f;
	private float fltSyncDelay = 0f;
	private float fltSyncTime = 0f;
	private Vector3 v3SyncStartPosition = Vector3.zero;
	private Vector3 v3SyncEndPosition = Vector3.zero;

	//These variables store the ID of the animation states
	int intJumpID			= Animator.StringToHash ("Jump");
	int intWalkID			= Animator.StringToHash ("Walk");
	int intRunID			= Animator.StringToHash ("Run");
	int intGuardID			= Animator.StringToHash ("Guard");
	int intFightID			= Animator.StringToHash ("FightingIDLE");
	int intLightStrikeID	= Animator.StringToHash ("LightStrike");
	int intTauntID			= Animator.StringToHash ("Taunt");
	int intHeavyStrikeID	= Animator.StringToHash ("HeavyStrike");
	int intHitID			= Animator.StringToHash ("Hit");
	int intDeathID			= Animator.StringToHash ("Death");
	int intVictoryID		= Animator.StringToHash ("Victory");
	int intIDLEID			= Animator.StringToHash ("IDLE");
	int intClockID			= Animator.StringToHash ("Clock");
	int intYawnID			= Animator.StringToHash ("Yawn");
	int intSleepID			= Animator.StringToHash ("Sleep");
	int intEntryID			= Animator.StringToHash ("Entry");
	int intSpecialID		= Animator.StringToHash ("Special");
	int intGrabID			= Animator.StringToHash ("Grab");
	int intThrowID			= Animator.StringToHash ("Throw");
	int intUltimateGrabID	= Animator.StringToHash ("Ultimate");
	int intHoldID			= Animator.StringToHash ("Hold");
	int intUltimateStrikeID	= Animator.StringToHash ("UltStrike");

	// EVENTS
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	void FixedUpdate ()
	{	
		if (networkView.isMine)
		{
			InputMovement();
		}
		else
		{
			SyncedMovement();
		}
	}

	void OnSerializeNetworkView(BitStream bstStream, NetworkMessageInfo nmiInfo){
		Debug.Log ("OnSerializeNetworkView");
		Vector3 v3SyncPosition = Vector3.zero;
		if(bstStream.isWriting){
			Debug.Log ("Writing");
			v3SyncPosition = rigidbody.position;
			bstStream.Serialize(ref v3SyncPosition);
		}

		if(bstStream.isReading){
			Debug.Log ("Reading");
			bstStream.Serialize(ref v3SyncPosition);
			
			fltSyncTime = 0f;
			fltSyncDelay = Time.time - fltLastSynchronizationTime;
			fltLastSynchronizationTime = Time.time;

			v3SyncStartPosition = rigidbody.position;
			v3SyncEndPosition = v3SyncPosition;
		}
	}

	// PRIVATE FUNCTIONS

	private void InputMovement(){
		if (anim && !MainController.blnMatchOver) {
			
			// get the current state of the animation
			AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
			
			//INPUTS
			
			//GUARDING
			
			//When the guard key is pressed, guard. On release, get back to IDLE
			if(Input.GetKey(KeyCode.T))
			{
				anim.SetTrigger (intGuardID);
			}
			else{
				anim.ResetTrigger (intGuardID);
			}
			
			//When the up arrow key is pressed, jump
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				Jump(animStateInfo);
			}
			
			//WALKING
			
			//When the walk key is pressed, walk. On release, get back to IDLE
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
			{
				if(anim.GetBool (intWalkID)== true){
					anim.SetBool (intRunID, true);
				}
				
				anim.SetBool (intWalkID, true);
				
				if(Input.GetKey(KeyCode.RightArrow)){
					transform.localEulerAngles = new Vector3(0.0f, fltRotation ,0.0f);
				}
				else if(Input.GetKey(KeyCode.LeftArrow)){
					transform.localEulerAngles = new Vector3(0.0f, -fltRotation ,0.0f);
				}
				
				if(Mathf.Abs(rigidbody.velocity.x) <= intMaximumSpeed){
					rigidbody.velocity += transform.forward * intMaximumSpeed / 12;
				}
			}
			else{
				anim.SetBool (intWalkID, false);
				anim.SetBool (intRunID, false);
				
				rigidbody.velocity = new Vector3(0.0f,rigidbody.velocity.y,rigidbody.velocity.z);
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
			
			if(Input.GetKeyDown(KeyCode.H)){
				Hit();
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
			
			//INPUT-NEEDLESS ANIMATIONS
			
			
			//Debug on the current animation state
			//Debug.Log (animStateInfo.nameHash);
		}
	}

	private void SyncedMovement(){
		fltSyncTime += Time.deltaTime;

		rigidbody.position = Vector3.Lerp (v3SyncStartPosition, v3SyncEndPosition, fltSyncTime/fltSyncDelay);
	}

	private void Jump(AnimatorStateInfo animStateInfo){
		if (Mathf.Floor(Mathf.Abs(rigidbody.velocity.y)) == 0.0f && animStateInfo.nameHash != intJumpID) {
			rigidbody.AddForce(Vector3.up * intJumpForce );
			anim.SetBool (intJumpID, true);
		}
	}

	private void Hit(){
		anim.SetTrigger (intHitID);
	}
	
	private void EndJump(){
		anim.SetBool (intJumpID, false);
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