using UnityEngine;
using System.Collections;

public class Animate: MonoBehaviour {
	
	public float rotateSpeed;
	
	void Start () {
		
	}
	
	void Update () {
		float angle = rotateSpeed * Time.deltaTime;
		transform.Rotate(transform.forward, angle);
	}
}