using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	public Transform explosionPrefab;
	
	public Material moneyMaterial;
	public Material fireMaterial;
	
	void Start () {
	}

	void Update () {
		
	}
	
	public void Explode(bool fire) {
		Transform explosion = Instantiate(explosionPrefab, transform.position, Quaternion.LookRotation(transform.forward)) as Transform;
		if (fire) explosion.GetComponent<ParticleRenderer>().material = fireMaterial;
		else explosion.GetComponent<ParticleRenderer>().material = moneyMaterial;
		explosion.parent = transform;
	}
}
