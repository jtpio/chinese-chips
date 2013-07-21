using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	
	protected Transform player;
	public int sign;
	public bool block;
	
	public static float playerDistance;
	
	void Start () {
		player = GameObject.Find("Player").transform;
		playerDistance = 0;
	}
	
	void Update () {
		Quaternion rot = Quaternion.LookRotation(sign * (transform.position-player.position+Vector3.up*4));
		if (block) rot = Quaternion.Euler(new Vector3(0, rot.eulerAngles.y, rot.eulerAngles.z));
		transform.rotation = rot;
		playerDistance = Vector3.Distance(transform.position, player.position);
	}
}
