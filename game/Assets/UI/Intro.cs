using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	
	public float speed = 1;
			
	void Start () {
		audio.Play();
	}
	
	void Update () {
		if (Application.platform == RuntimePlatform.Android && Input.touchCount >= 1) {
			Application.LoadLevel(1);
		} else if (Input.GetKey(KeyCode.S) ) {
			// press escape to skip
			Application.LoadLevel(1);
		}
		
		if (transform.position.x > 0) {
			transform.Translate(speed * Time.deltaTime, 0, 0);	
		}
	}
}
