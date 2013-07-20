using UnityEngine;
using System.Collections;

public class Destruction : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		
		if (MathUtils.RandomChance(10)) {
			int randomChild = Random.Range(Mathf.Min (2, transform.childCount-1), transform.childCount);
			Transform child = transform.GetChild(randomChild);
			if (child.name != "group" && child.name != "joint1") {
				if (!child.GetComponent<Throw>()) child.gameObject.AddComponent<Throw>();
			}
		}
	
	}
}
