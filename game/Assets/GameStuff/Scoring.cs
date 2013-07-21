using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {
	
	// parameters
	public GUIStyle style;
	
	public TextMesh scoreText;
	
	public static bool scoring;
	public static int multiplier;
	
	protected int score;
	protected float time;
	
	void Start () {
		time = 0;
		score = 0;
		scoring = false;
	}
	
	void Update () {
		if (!scoring) return;
		
		if (WavesManager.status != Status.GameOver) {
			int temp = (int)(Time.deltaTime * 100);
			multiplier = Mathf.Clamp(multiplier, 1, 16);
			score += temp * multiplier;
		}
		
		scoreText.text = score+"";
	}

}
