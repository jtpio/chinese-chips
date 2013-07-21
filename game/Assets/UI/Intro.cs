using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	
	public float speed = 1;
	
	protected float time;
			
	void Start () {
		audio.Play();
		time = 0;
	}
	
	void Update () {
		
		time += Time.deltaTime;
		
		if (time > 5) {
			if (Application.platform == RuntimePlatform.Android && Input.touchCount >= 1) {
				Application.LoadLevel(2);
			} else if (Input.GetKey(KeyCode.S) ) {
				// press escape to skip
				Application.LoadLevel(2);
			}
			
			if (transform.position.x > 0) {
				transform.Translate(speed * Time.deltaTime, 0, 0);	
			}
		}
	}
}
