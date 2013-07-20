using UnityEngine;
using System.Collections;

public class AnimateShip : MonoBehaviour {
	
	public float shakeAnimSpeed;
	
	void Start () {
	}
	
	void Update () {
		animation["ShakeAnim"].speed = shakeAnimSpeed;
	}
}
