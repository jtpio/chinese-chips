using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
	
	public GUIStyle style;
	public int step;	
	public int duration;
	public string goText;
	
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
	
	public void SetCountdownDuration(int duration) {
		this.duration = duration;	
	}
	
	public bool IsPlaying() {
		return display;
	}
	
	public void StartTimer(CallBack cb) {
		this.callback = cb;
		StartTimer();
	}
	
	public void StartTimer() {
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
		current = goText;
		yield return new WaitForSeconds(step);
		display = false;
		if (callback != null) callback();
	}
	
	void OnGUI () {
		if (display) {
			GUI.Label(new Rect(Screen.width / 2 - style.fixedWidth / 2, Screen.height / 2 - style.fixedHeight / 2, 100, 100), 
				current, style
			);
		}
	}
	
}
