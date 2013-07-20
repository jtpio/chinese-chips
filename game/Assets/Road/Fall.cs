using UnityEngine;
using System.Collections;
using Utils;

public class Fall : MonoBehaviour {
	
	public float maxAngle = 0.3f;
	public float lifeDuration = 10.0f;
	
	protected Timer timer;
	
	void Start () {
		transform.RotateAround(transform.forward, Random.Range(-maxAngle, maxAngle));
		transform.gameObject.AddComponent<Rigidbody>();
		
		timer = new Timer(lifeDuration);
	}
	
	void Update () {
		timer.Update(Time.deltaTime);
		if (timer.IsFinished()) {
			Destroy(gameObject);
		}
	}
}
