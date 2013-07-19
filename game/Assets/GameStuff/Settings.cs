using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	
	public Vector3 startPosition;
	public Vector3 startDirection;
	
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
