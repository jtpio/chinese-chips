using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour {
	
	public float initSpeed;
	public float sideSpeed;
	public Vector3 yOffset;
	public static Vector3 position;
	
	protected RoadManager roadManager;
	protected Vector3 direction;
	protected float distance;
	
	void Start () {
		yOffset = new Vector3(0.0f, 0.2f, 0.0f);
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		distance = 0;
		direction = transform.forward;
	}
	
	void Update () {
		float speed = initSpeed * Time.deltaTime;
		Vector3 deltaPos = direction * speed;
		transform.Translate(deltaPos, Space.World);
		distance += deltaPos.magnitude;
		
		Quaternion newRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.2f);
		transform.rotation = rotation;
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) {
			transform.Translate(-transform.right * sideSpeed);
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Translate(transform.right * sideSpeed);
		}
		
		direction = roadManager.GetDirectionAtDistance(distance);
		
		position = transform.position;
	}
	
	public void SetPosition(Vector3 pos) {
		transform.position = pos + yOffset;	
	}
}
