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
	
	}
}
