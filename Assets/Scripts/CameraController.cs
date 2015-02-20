using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform tranPlayer;

	private Transform tranCamera ;

	void Start () {
		tranCamera = this.transform;
	}

	// Update is called once per frame
	void Update () {

		transform.position = new Vector3(
			tranPlayer.position.x,
			tranCamera.position.y,
			tranCamera.position.z
		);
	}
}
