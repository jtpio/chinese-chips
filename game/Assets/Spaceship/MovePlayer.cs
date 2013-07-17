using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour {
	
	public float initSpeed;
	public float sideSpeed;
	public Vector3 yOffset;
	public static Vector3 position;
	
	void Start () {
		yOffset = new Vector3(0.0f, 0.2f, 0.0f);
	}
	
	void Update () {
		float speed = initSpeed * Time.deltaTime;
		transform.Translate(transform.forward * speed);
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) {
			transform.Translate(-transform.right * sideSpeed);
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Translate(transform.right * sideSpeed);
		}
		
		position = transform.position;
	}
	
	public void SetPosition(Vector3 pos) {
		transform.position = pos + yOffset;	
	}
}
