using UnityEngine;
using System.Collections;

public class GameStatus : MonoBehaviour {
	
	public GUIStyle style;
	public string gameOverText;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void OnGUI () {
		if (WavesManager.status == Status.GameOver) {
			GUI.Label(new Rect(Screen.width / 2 - style.fixedWidth / 2, Screen.height / 2 - 2 * style.fixedHeight / 3, 100, 100), 
				gameOverText, style
			);
		}
	}
}
