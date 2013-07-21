using UnityEngine;
using System.Collections;

public class LaseBulletDie : MonoBehaviour {
	
	public float life;

	float time;
	
	void Start () {
		time = 0;
	}
	
	void Update () {
		time += Time.deltaTime;
		if (time > life) {
			Destroy(gameObject);	
		}
	}
}
