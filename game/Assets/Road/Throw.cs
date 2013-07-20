using UnityEngine;
using System.Collections;
using Utils;

public class Throw : MonoBehaviour {
	
	Timer timer;
	
	void Start () {
		float xOffset = Random.Range(0,1.0f);
		float yOffset = Random.Range(0,1.0f);
		float zOffset = Random.Range(0,1.0f);

		gameObject.AddComponent<Rigidbody>();
		gameObject.GetComponent<Rigidbody>().mass = 100;
		gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up * 30, transform.position + new Vector3(xOffset, yOffset, zOffset));
		
		timer = new Timer(3.0f);
	}
	
	void Update () {
		timer.Update(Time.deltaTime);
		if (timer.IsFinished()) {
			Destroy(gameObject);
		}
	}
}
