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
	public int intMaximumSpeed;		// Maximum speed of the player
	public int intJumpForce;		// Vertical force coefficient applied on a jump

	private Animator anim;				// Animation controller
	private int intAnimStateInfo;		// Current state of the animation
	private int intAnimOldStateInfo;	// Previous state
	private int intAnimOtherState;		// Animation of the opponent
	private int intAnimOldOtherState;	// Animation of the opponent

	private float fltRotation = 90;					// Rotation of a player
	private float fltLastSynchronizationTime = 0f;	// Time of last synchronization
	private float fltSyncDelay = 0f;				// Delay of the synchronization
	private float fltSyncTime = 0f;					// time of synchronization

	private Vector3 v3SyncStartPosition = Vector3.zero;		// initial position of the player
	private Vector3 v3SyncEndPosition = Vector3.zero;		// final position of the player



	//These variables store the ID of the animation states (determined by name)
	static int intJumpID			= Animator.StringToHash ("Base.Jump");
	static int intWalkID			= Animator.StringToHash ("Base.Walk");
	static int intRunID				= Animator.StringToHash ("Base.Run");
	static int intGuardID			= Animator.StringToHash ("Base.Guard");
	static int intFightID			= Animator.StringToHash ("FightingIDLE");
	static int intLightStrikeID		= Animator.StringToHash ("LightStrike");
	static int intTauntID			= Animator.StringToHash ("Taunt");
	static int intHeavyStrikeID		= Animator.StringToHash ("HeavyStrike");
	static int intHitID				= Animator.StringToHash ("Hit");
	static int intDeathID			= Animator.StringToHash ("Death");
	static int intVictoryID			= Animator.StringToHash ("Victory");
	static int intIDLEID			= Animator.StringToHash ("IDLE");
	static int intClockID			= Animator.StringToHash ("Clock");
	static int intYawnID			= Animator.StringToHash ("Yawn");
	static int intSleepID			= Animator.StringToHash ("Sleep");
	static int intEntryID			= Animator.StringToHash ("Entry");
	static int intSpecialID			= Animator.StringToHash ("Special");
	static int intGrabID			= Animator.StringToHash ("Grab");
	static int intThrowID			= Animator.StringToHash ("Throw");
	static int intUltimateGrabID	= Animator.StringToHash ("Ultimate");
	static int intHoldID			= Animator.StringToHash ("Hold");
	static int intUltimateStrikeID	= Animator.StringToHash ("UltStrike");

	//////////////////////// EVENTS ///////////////////////////
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	void Update ()
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
		Vector3 v3SyncPosition = Vector3.zero;
		Vector3 v3SyncRotation = Vector3.zero;
		int intSyncAnimation = 0;

		if(bstStream.isWriting){
			v3SyncPosition = rigidbody.position;
			bstStream.Serialize(ref v3SyncPosition);

			v3SyncRotation = transform.localEulerAngles;
			bstStream.Serialize(ref v3SyncRotation);


			if(intAnimOldStateInfo != intAnimStateInfo){
				intSyncAnimation = intAnimStateInfo;
				bstStream.Serialize(ref intSyncAnimation);
			}

			intAnimOldStateInfo = intAnimStateInfo;
		}

		if(bstStream.isReading){
			bstStream.Serialize(ref v3SyncPosition);
			
			fltSyncTime = 0f;
			fltSyncDelay = Time.time - fltLastSynchronizationTime;
			fltLastSynchronizationTime = Time.time;

			v3SyncStartPosition = rigidbody.position;
			v3SyncEndPosition = v3SyncPosition;

			bstStream.Serialize(ref v3SyncRotation);
			transform.localEulerAngles = v3SyncRotation;

			bstStream.Serialize(ref intSyncAnimation);
			intAnimOtherState = intSyncAnimation;
		}
	}

	//////////////////////// PRIVATE FUNCTIONS /////////////////////////// 
	private void InputMovement(){
		if (anim && !MainController.blnMatchOver) {

			// get the current state of the animation
			intAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0).nameHash;

			//When the guard key is pressed, guard.
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
				Jump(intAnimStateInfo);
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
		}
	}

	private void SyncedMovement(){
		fltSyncTime += Time.deltaTime;

		rigidbody.position = Vector3.Lerp (v3SyncStartPosition, v3SyncEndPosition, fltSyncTime/fltSyncDelay);
		if(intAnimOldOtherState != intAnimOtherState){

			anim.SetTrigger (intAnimOtherState);
			anim.ResetTrigger (intAnimOldOtherState);
		}

		intAnimOldOtherState = intAnimOtherState;
		
	}

	private void Jump(int intAnimStateInfo){
		if (Mathf.Floor(Mathf.Abs(rigidbody.velocity.y)) == 0.0f && intAnimStateInfo != intJumpID) {
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