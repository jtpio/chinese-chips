using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	
	public Transform player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(-transform.position+player.position);
	
	}
}
