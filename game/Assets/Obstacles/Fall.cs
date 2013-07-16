using UnityEngine;
using System.Collections;
using Utils;

public class Fall : MonoBehaviour {
	
	public float life;
	
	protected Timer timer;
	
	void Start () {
		timer = new Timer(life);	
	}
	
	void Update () {
		timer.Update(Time.deltaTime);
		if (timer.IsFinished()) {
			Destroy(gameObject);
		}
	}	
}
