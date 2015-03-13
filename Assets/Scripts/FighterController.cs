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

	// Constants
	public int intToIdle = 100;
	public int intToBoredom = 800;

	// variables
	public int intMaximumSpeed;		// Maximum speed of the player
	public int intJumpForce;		// Vertical force coefficient applied on a jump

	private Animator anim;				// Animation controller
	private int intAnimStateInfo;		// Current state of the animation
	private int intAnimOldStateInfo;	// Previous state
	private int intAnimOtherState;		// Animation of the opponent
	private int intAnimOldOtherState;	// Animation of the opponent

	private float fltRotation					= 90f;	// Rotation of a player
	private float fltLastSynchronizationTime	= 0f;	// Time of last synchronization
	private float fltSyncDelay					= 0f;	// Delay of the synchronization
	private float fltSyncTime					= 0f;	// time of synchronization

	private static float fltPositionOpponent	= 0f;		// position of opponent
	private static float fltPositionDelta		= 0f;		// horizontal distance between the players

	private static bool blnIsHit = false;

	private int intCountToIdle		= 0;
	private int intCountToBoredom	= 0;

	private Vector3 v3SyncStartPosition	= Vector3.zero;		// initial position of the player
	private Vector3 v3SyncEndPosition	= Vector3.zero;		// final position of the player

		

	//These variables store the ID of the animation states (determined by name)
	static int intTauntID			= Animator.StringToHash ("Base.Taunt");
	static int intHitID				= Animator.StringToHash ("Base.Hit");
	static int intDeathID			= Animator.StringToHash ("Base.Death");
	static int intVictoryID			= Animator.StringToHash ("Base.Victory");
	static int intUltimateGrabID	= Animator.StringToHash ("Base.Ultimate");
	static int intLightStrikeID		= Animator.StringToHash ("Base.LightStrike");
	static int intHeavyStrikeID		= Animator.StringToHash ("Base.HeavyStrike");
	static int intSpecialID			= Animator.StringToHash ("Base.Special");
	static int intGrabID			= Animator.StringToHash ("Base.Grab");
	static int intFightID			= Animator.StringToHash ("Base.Fighting");
	static int intIdleID			= Animator.StringToHash ("Base.Idle");
	static int intGuardID			= Animator.StringToHash ("Base.Guard");
	static int intJumpID			= Animator.StringToHash ("Base.Jump");
	static int intWalkID			= Animator.StringToHash ("Base.Walk");
	static int intRunID				= Animator.StringToHash ("Base.Run");
	static int intBoredomID			= Animator.StringToHash ("Base.Boredom");


	/*

	static int intEntryID			= Animator.StringToHash ("Base.Entry");
	static int intThrowID			= Animator.StringToHash ("Base.Throw");
	static int intHoldID			= Animator.StringToHash ("Base.Hold");
	static int intUltimateStrikeID	= Animator.StringToHash ("Base.UltStrike");
	*/

	//////////////////////// EVENTS ///////////////////////////
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	void Update ()
	{

		// idle management
		if(!Input.anyKey){
			if(intCountToIdle < intToIdle){
				intCountToIdle++;
			}
			else if(intCountToBoredom < intToBoredom){
				if(intCountToBoredom == 0){
					anim.SetTrigger(intIdleID);
				}
				intCountToBoredom++;
				
				if(anim.GetInteger(intBoredomID) != 0){
					anim.SetInteger(intBoredomID,0);
				}
			}
			else{
				intCountToBoredom = 1;
				anim.SetInteger(intBoredomID,RandomInteger(1,3));
			}
		}
		else{
			intCountToIdle = 0;
			intCountToBoredom = 0;
		}

		if (networkView.isMine)
		{
			fltPositionDelta = Mathf.Abs(transform.position.x - fltPositionOpponent);

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
		bool blnSyncIsHit = false;

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

			blnSyncIsHit = blnIsHit;
			bstStream.Serialize(ref blnSyncIsHit);

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

			bstStream.Serialize(ref blnSyncIsHit);
			blnIsHit = blnSyncIsHit;
		}
	}

	//////////////////////// PRIVATE FUNCTIONS /////////////////////////// 
	private void InputMovement(){
		if (anim && !MainController.blnMatchOver) {

			// get the current state of the animation
			intAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0).nameHash;

			if(blnIsHit){
				Hit();
				blnIsHit = false;
			}

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
				HitOpponent();

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
		}
	}

	private void SyncedMovement(){
		fltSyncTime += Time.deltaTime;

		rigidbody.position = Vector3.Lerp (v3SyncStartPosition, v3SyncEndPosition, fltSyncTime/fltSyncDelay);

		fltPositionOpponent = rigidbody.position.x;

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

	private void HitOpponent(){
		if(fltPositionDelta <= 6.5f){
			blnIsHit = true;
		}
	}

	// synchronized action to the onnonent character
	private void Hit(){
		anim.SetTrigger (intHitID);
	}

	// random int number generator
	private int RandomInteger(int intStart, int intEnd){

		if(intStart == intEnd){
			return intStart;
		}
		else if(intStart > intEnd){
			int intTemp = intStart;
			intStart = intEnd;
			intEnd = intTemp;
		}

		int intDeltaValues = Mathf.Abs(intEnd-intStart);

		int intRandomValue = Random.Range(0, 10 * intDeltaValues);
		intRandomValue %= (intDeltaValues + 1);
		intRandomValue += intStart;		

		return intRandomValue;
	}


	// stop the jump (animation event)
	private void EndJump(){
		anim.SetBool (intJumpID, false);
	}
}