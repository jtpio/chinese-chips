using UnityEngine;
using System.Collections;

public class Node {
	
	public Vector3 position;
	public Vector3 direction;
	public float time;
	public Transform transform;
	public NodeType type;
	
	public Node() {
		position = new Vector3(0, 0, 0);
		direction = new Vector3(0, 0, 0);
		time = 0;
		transform = null;
		type = NodeType.Normal;
	}

	public Node(Vector3 position, Vector3 direction, float time, Transform transform, NodeType type) {
		this.position = position;
		this.direction = direction;
		this.time = time;
		this.transform = transform;
		this.type = type;
	}
}
