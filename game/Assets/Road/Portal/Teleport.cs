using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	
	public Transform outPortal;
	public MovePlayer movePlayer;
	
	void Start () {
		movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
	}
	
	void Update () {

	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("on collision!");
		if (outPortal) {
			movePlayer.SetPosition(outPortal.transform.position);
		}
	}
	
}
