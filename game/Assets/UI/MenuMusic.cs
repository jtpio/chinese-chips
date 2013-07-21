using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {

	AudioSource introMusic;
	
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		introMusic = sources[0];
		introMusic.loop = true;
		introMusic.Play();
	}
	
	void Update () {
		
		if (Application.platform == RuntimePlatform.Android && Input.touchCount >= 1) {
			Application.LoadLevel(1);
		} else if (Input.GetKey(KeyCode.S) ) {
			// press escape to skip
			Application.LoadLevel(1);
		}
	
	}
}
