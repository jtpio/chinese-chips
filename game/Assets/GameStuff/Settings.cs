using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit();	
			}
		}
	}
}
