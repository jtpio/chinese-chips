using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour {
	
	// parameters
	public float speed;
	public float sideSpeed;
	public float turnSpeed;
	
	// internals public
	public Vector3 yOffset;
	public float time;
	public Vector3 direction;

	public static Vector3 position;
	
	protected RoadManager roadManager;
	
	void Start () {
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		time = 0;
		direction = transform.forward;
	}
	
	void Update () {
		float delta = Time.deltaTime * speed;
		time += delta;
		transform.Translate(delta * direction, Space.World);

		Quaternion newRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed);
		transform.rotation = rotation;
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) {
			transform.Translate(-transform.right * sideSpeed, Space.World);
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Translate(transform.right * sideSpeed, Space.World);
		}

		position = transform.position;
	}
	
	public void SetPosition(Vector3 pos) {
		transform.position = pos + yOffset;	
	}
	
	public void InstantRotation(Vector3 direction) {
		this.direction = direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
