using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	
	protected Transform player;
	public int sign;
	
	public static float playerDistance;
	
	void Start () {
		player = GameObject.Find("Player").transform;
		playerDistance = 0;
	}
	
	void Update () {
		transform.rotation = Quaternion.LookRotation(sign * (transform.position-player.position+Vector3.up*4));
		playerDistance = Vector3.Distance(transform.position, player.position);
	}
}
