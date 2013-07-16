using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoMovePlayer : MonoBehaviour {
	
	public float speed;
	
	protected RoadManager roadManager;
	protected float time;
	protected Vector3 prevPos;
	
	void Start () {
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		time = 0;
		prevPos = transform.position;
	}
	
	void Update () {
		time += Time.deltaTime * speed;
		
		transform.position = prevPos + transform.up * 0.5f;
		SplinePos currPos = roadManager.GetPositonAtTime(time);
		Vector3 direction = currPos.point - prevPos;
		Quaternion rotation = Quaternion.LookRotation(direction, currPos.normal);
		transform.rotation = rotation;
		prevPos = currPos.point;
	}
}
