using UnityEngine;
using System.Collections;

public class MathUtils {
	
	/*
	 * Returns -1 or 1 randomly
	 */
	public static int RandomSign() {
		return (Random.Range(0,2) == 0)?1:-1;
	}
	
	/* 
	 * Return either true or false randomly
	 */
	public static bool RandomBool() {
		return (Random.Range(0,2) == 0);	
	}
	
	/*
	 * Returns a "1 chance over <rate>" probability
	 */
	public static bool RandomChance(float rate) {
		return (Random.Range(0.0f, 100.0f) <= rate);	
	}
	
	public static Vector3 RandomTurn(Vector3 direction) {
		return Quaternion.AngleAxis(RandomSign() * 90, Vector3.up) * direction;
	}
	
}
