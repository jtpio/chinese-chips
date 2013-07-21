using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	public Transform explosionPrefab;
	
	void Start () {
	}

	void Update () {
		
	}
	
	public void Explode() {
		Transform explosion = Instantiate(explosionPrefab, transform.position, Quaternion.LookRotation(transform.forward)) as Transform;
		explosion.parent = transform;
	}
}
