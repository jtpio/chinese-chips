using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour {
	
	public float speed;
	public float sideSpeed;
	public static Vector3 position;
	
	protected RoadManager roadManager;
	protected float time;
	protected Vector3 prevPos;
	protected Quaternion prevRot;
	protected float offset;
	
	void Start () {
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		time = 0;
		offset = 0;
		prevPos = transform.position;
	}
	
	void Update () {
		time += Time.deltaTime * speed;
		
		SplinePos currPos = roadManager.GetPositonAtTime(time);
		Vector3 direction = currPos.point - prevPos;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			offset -=  sideSpeed;
		}
		
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			offset += sideSpeed;
		}
		
		rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
		
		transform.position = prevPos + transform.up * 0.2f;
		transform.rotation = rotation;
		prevPos = currPos.point;
		prevRot = rotation;
		
		roadManager.Recycle(time);
		position = transform.position;
	}
}
