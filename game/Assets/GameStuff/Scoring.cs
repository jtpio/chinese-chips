using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {
	
	// parameters
	public GUIStyle style;
	
	protected int score;
	protected float time;
	
	void Start () {
		time = 0;
		score = 0;
	}
	
	void Update () {
		time += Time.deltaTime;
		score = (int)Mathf.Round(time * 10);
	}
	
	void OnGUI () {
		GUI.Label(new Rect(0, 0, 100, 100), 
			score+"", style
		);
	}
	
}
