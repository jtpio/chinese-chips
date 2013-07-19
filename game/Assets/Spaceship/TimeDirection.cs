using UnityEngine;
using System.Collections;

public class TimeDirection {
	
	public Vector3 direction;
	public float time;
	
	public TimeDirection() {
	}
	
	public TimeDirection(Vector3 direction, float time) {
		this.direction = direction;
		this.time = time;
	}
}
