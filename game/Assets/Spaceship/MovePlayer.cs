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
	public float offset;
	
	public static Vector3 position;
	
	protected Vector3 basePosition;
	protected RoadManager roadManager;
	protected Transform ship;
	
	void Start () {
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		ship = transform.FindChild("Ship");
		time = 0;
		direction = transform.forward;
		offset = 0;
		basePosition = new Vector3(0, 0, 0);
	}
	
	void Update () {
		float delta = Time.deltaTime * speed;
		time += delta;
		
		basePosition += delta * direction;
		//transform.Translate(delta * direction, Space.World);

		Quaternion newRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed);
		transform.rotation = rotation;
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) {
			offset -= sideSpeed;
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			offset += sideSpeed;
		}
		
		offset = Mathf.Clamp(offset, -20, 20);
		
		transform.position = basePosition + offset * transform.right + yOffset;
		//ship.transform.rotation = Quaternion.AngleAxis(offset, transform.forward);
		position = transform.position;
	}
	
	public void SetPosition(Vector3 pos) {
		basePosition = pos;	
	}
	
	public void InstantRotation(Vector3 direction) {
		this.direction = direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
