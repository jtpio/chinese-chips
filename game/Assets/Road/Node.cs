using UnityEngine;
using System.Collections;

public class Node {
	
	public Vector3 position;
	public Vector3 direction;
	public float distance; // distance so far
	
	public Node() {
		position = new Vector3(0, 0, 0);
		direction = new Vector3(0, 0, 0);
		distance = 0;
	}
	
	public Node(Vector3 position, Vector3 direction, float distance) {
		this.position = position;
		this.direction = direction;
		this.distance = distance;
	}
}
