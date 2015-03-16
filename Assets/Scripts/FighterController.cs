//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
// Date : 18.02.15
// But : Player and animations controller script
//*********************************************************
using UnityEngine;
using System.Collections;

public class FighterController : MonoBehaviour {

	// Constants
	public int intToIdle = 100;		// Const. used to define the time before IDLE starts
	public int intToBoredom = 800;	// Const. used to define the time before boring state starts

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

	private int intCountToIdle		= 0;	// Used to count the time until it reaches the corresponding Const. (IDLE)
	private int intCountToBoredom	= 0;	// Used to count the time until it reaches the corresponding Const. (Bored state)

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


	//Not used (currently)
	/*
	static int intEntryID			= Animator.StringToHash ("Base.Entry");
	static int intThrowID			= Animator.StringToHash ("Base.Throw");
	static int intHoldID			= Animator.StringToHash ("Base.Hold");
	static int intUltimateStrikeID	= Animator.StringToHash ("Base.UltStrike");
	*/

	//////////////////////// EVENTS ///////////////////////////

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start ()
	{
		//Gets the Animator from Unity, will be used later
		anim = GetComponent<Animator>();
	}

	// *******************************************************************
	// Function called at each game frame
	// *******************************************************************
	void Update ()
	{

		// Idle management: as soon as no key press is detected, wait for a set number of frames 
		// and then change for the next state
		if(!Input.anyKey){

			// We shall stay in Fighting IDLE as long as inCountToIdle is below intToIdle
			if(intCountToIdle < intToIdle){
				intCountToIdle++;
			}

			// Or it goes into standard IDLE
			else if(intCountToBoredom < intToBoredom){
				if(intCountToBoredom == 0){
					anim.SetTrigger(intIdleID);
				}
				intCountToBoredom++;
				
				if(anim.GetInteger(intBoredomID) != 0){
					anim.SetInteger(intBoredomID,0);
				}
			}

			// Else, it goes into Bored IDLE, getting a random number between 1 and 3 and 
			// choosing the corresponding bored animation
			else{
				intCountToBoredom = 1;
				anim.SetInteger(intBoredomID,RandomInteger(1,3));
			}
		}
		//If a key is pressed, reinitialize the counting variables
		else{
			intCountToIdle = 0;
			intCountToBoredom = 0;
		}

		// Player object (Mine / Not mine) management
		if (networkView.isMine)
		{
			// If the character is mine, get his current distance from the other character
			fltPositionDelta = Mathf.Abs(transform.position.x - fltPositionOpponent);

			// If the character is mine, it will react to key presses
			InputMovement();
		}
		else
		{
			// If it ain't mine, it will move accordingly to the other player's key presses
			// (and his position will appear here)
			SyncedMovement();
		}

	}

	// *******************************************************************
	// Nom : OnSerializeNetworkView
	// But : (Unity function) knowing when I'm writing on the network stream and when I'm not, 
	// and what to do
	// Retour: Void
	// Param.: None
	// *******************************************************************
	void OnSerializeNetworkView(BitStream bstStream, NetworkMessageInfo nmiInfo){
		Vector3 v3SyncPosition = Vector3.zero;
		Vector3 v3SyncRotation = Vector3.zero;
		int intSyncAnimation = 0;
		bool blnSyncIsHit = false;

		// When the software is writing, gets the current position and rotation of the character and serializes it,
		// thus effectively sending it in a two-step process to the other player
		if(bstStream.isWriting){

			// Position
			v3SyncPosition = rigidbody.position;
			bstStream.Serialize(ref v3SyncPosition);

			// Rotation
			v3SyncRotation = transform.localEulerAngles;
			bstStream.Serialize(ref v3SyncRotation);

			// Animation part: if the animation state did change between the update, send the new state.
			// If it didn't change (same animation), don't send it.
			if(intAnimOldStateInfo != intAnimStateInfo){
				intSyncAnimation = intAnimStateInfo;
				bstStream.Serialize(ref intSyncAnimation);
			}

			// The current anim state is now considered as old, store its value
			intAnimOldStateInfo = intAnimStateInfo;

			// Sync the current "Hit ?" state. 
			blnSyncIsHit = blnIsHit;
			bstStream.Serialize(ref blnSyncIsHit);

		}

		// When the software is reading, obtains the serialized position and rotation of the character and applies
		// it to the local prefab of the character (enemy character)
		if(bstStream.isReading){
			bstStream.Serialize(ref v3SyncPosition);

			//This part is used for interpolation (avoiding too much network lag)
			fltSyncTime = 0f;
			//Gets the delay between the last sync and this one
			fltSyncDelay = Time.time - fltLastSynchronizationTime;
			//The current time is now the time of the last sync
			fltLastSynchronizationTime = Time.time;

			//Sync the position
			v3SyncStartPosition = rigidbody.position;
			v3SyncEndPosition = v3SyncPosition;

			//Sync the rotation
			bstStream.Serialize(ref v3SyncRotation);
			transform.localEulerAngles = v3SyncRotation;

			//Sync the animation
			bstStream.Serialize(ref intSyncAnimation);
			intAnimOtherState = intSyncAnimation;

			//Sync the "Hit ?" state
			bstStream.Serialize(ref blnSyncIsHit);
			blnIsHit = blnSyncIsHit;
		}
	}

	//////////////////////// PRIVATE FUNCTIONS /////////////////////////// 

	// *******************************************************************
	// Nom : InputMovement
	// But : Moves the character when a mov. key is pressed, unless another event prevents it
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void InputMovement(){
		if (anim && !MainController.blnMatchOver) {

			// Get the current state of the animation
			intAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0).nameHash;

			//If the "Hit ?" was on true when the soft synced, character is hit
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
			
			//When the up arrow key is pressed, calls the jump function
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				Jump(intAnimStateInfo);
			}
			
			//WALKING
			
			//When a walk key is pressed, walk. On release, get back to IDLE
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
			{
				// If the previous state was walking, next one is running
				if(anim.GetBool (intWalkID)== true){
					anim.SetBool (intRunID, true);
				}

				// Current state is walking
				anim.SetBool (intWalkID, true);

				//Depending of the key pressed (Right/Left), rotate right/left
				if(Input.GetKey(KeyCode.RightArrow)){
					transform.localEulerAngles = new Vector3(0.0f, fltRotation ,0.0f);
				}
				else if(Input.GetKey(KeyCode.LeftArrow)){
					transform.localEulerAngles = new Vector3(0.0f, -fltRotation ,0.0f);
				}

				//As long as the current speed is below the maxone, go faster
				if(Mathf.Abs(rigidbody.velocity.x) <= intMaximumSpeed){
					rigidbody.velocity += transform.forward * intMaximumSpeed / 12;
				}
			}
			//When no walking key is pressed, doesn't play (or stops playing) the walking/running animations
			else{
				anim.SetBool (intWalkID, false);
				anim.SetBool (intRunID, false);

				//If the keys are released or not pressed, don't move
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
			
			//When the second strike key is pressed, strike (harder)
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
			
			//When the ultimate key is pressed, ultimate (starting with a grab animation, in SUSAn' case)
			//NEED THE HEALTH OF THE ADVERSARY TO BE UNDER 33% <= NOT IMPLEMENTED YET
			if(Input.GetKeyDown(KeyCode.R))
			{
				anim.SetTrigger (intUltimateGrabID);
			}
		}
	}

	// *******************************************************************
	// Nom : SyncedMovement
	// But : Sync the movement/animations made by the other character (the one that isn't controlled)
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void SyncedMovement(){
		fltSyncTime += Time.deltaTime;

		//The opponent's position is based on the sync'd variables, interpolated between the start position, the end position,
		//using the time between the two, creating a "middle" position (interpolation) that helps reducing the lag, visually
		rigidbody.position = Vector3.Lerp (v3SyncStartPosition, v3SyncEndPosition, fltSyncTime/fltSyncDelay);

		//Sets the opponent's position
		fltPositionOpponent = rigidbody.position.x;

		//If the previous anim state is different, plays the new one
		if(intAnimOldOtherState != intAnimOtherState){

			anim.SetTrigger (intAnimOtherState);
			anim.ResetTrigger (intAnimOldOtherState);
		}

		//The current anim is now considered old
		intAnimOldOtherState = intAnimOtherState;
		
	}
	
	// *******************************************************************
	// Nom : Jump
	// But : Making the character jumps when the up arrow key is pressed
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void Jump(int intAnimStateInfo){
		//If the vertical velocity is null, and the current anim state isn't "Jumping"
		if (Mathf.Floor(Mathf.Abs(rigidbody.velocity.y)) == 0.0f && intAnimStateInfo != intJumpID) {
			//Jump (adding some vertical force)
			rigidbody.AddForce(Vector3.up * intJumpForce );
			//Character is now jumping
			anim.SetBool (intJumpID, true);
		}
	}

	// *******************************************************************
	// Nom : HitOpponent
	// But : Knowing when the oppponent is possibly hit during a hit animation
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void HitOpponent(){
		//If the distance between the two fighters is small enough
		if(fltPositionDelta <= 6.5f){
			blnIsHit = true;
		}
	}
	
	// *******************************************************************
	// Nom : Hit
	// But : synchronized action to the onnonent character
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void Hit(){
		anim.SetTrigger (intHitID);
	}
	 
	// *******************************************************************
	// Nom : RandomInteger
	// But : random int number generator
	// Retour: Void
	// Param.: None
	// *******************************************************************
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


	// *******************************************************************
	// Nom : EndJump
	// But : stop the jump (animation event)
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private void EndJump(){
		anim.SetBool (intJumpID, false);
	}
}