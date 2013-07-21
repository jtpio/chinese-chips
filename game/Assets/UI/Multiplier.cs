using UnityEngine;
using System.Collections;

public class Multiplier : MonoBehaviour {

	protected TextMesh mesh;
	
	void Start () {
		mesh = GetComponent<TextMesh>();
	}
	
	void Update () {
		mesh.text = "x" + Scoring.multiplier;
	}
}
