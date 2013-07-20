using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	
	protected Transform player;
	public int sign;
	
	void Start () {
		player = GameObject.Find("Player").transform;
	
	}
	
	void Update () {
		transform.rotation = Quaternion.LookRotation(sign * (transform.position-player.position));
	
	}
}
