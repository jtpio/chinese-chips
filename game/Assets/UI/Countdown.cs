using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
	
	public GUIStyle style;
	public int duration;
	public int step;
	
	protected bool display;
	protected string current;
	
	public delegate void CallBack();
	
	protected CallBack callback;

	void Start () {
		Reset();
	}
	
	void Reset() {
		display = false;	
		current = duration.ToString();
	}
	
	void Update () {
		
	}
	
	public void StartTimer(CallBack cb) {
		//StopTimer();
		this.callback = cb;
		display = true;
		StartCoroutine(WaitAndPrint(duration, step));
	}
	
	public void StopTimer() {
		StopCoroutine("WaitAndPrint");
		display = false;
	}
	
	IEnumerator WaitAndPrint(int duration, int step) {
		int time = duration;
		while (time > 0) {
			current = time.ToString();
			yield return new WaitForSeconds(step);
			time -= step;
		}
		current = "Wave!";
		yield return new WaitForSeconds(step);
		display = false;
		callback();
	}
	
	void OnGUI () {
		if (display) {
			GUI.Label(new Rect(Screen.width / 2 - style.fixedWidth / 2, Screen.height / 2 - style.fixedHeight / 2, 100, 100), 
				current, style
			);
		}
	}
	
}
