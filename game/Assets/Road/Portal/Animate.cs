using UnityEngine;
using System.Collections;

public class Animate: MonoBehaviour {
	
	public float rotateSpeed;
	public Vector3 offset;
	
	void Start () {
		transform.position += offset;		
	}
	
	void Update () {
		float angle = rotateSpeed * Time.deltaTime;
		transform.RotateAround(transform.forward, angle);
	}
}
