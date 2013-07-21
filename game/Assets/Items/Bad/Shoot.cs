using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
		
	public Transform bulletPrefab;	
	
	Transform laserBullet;
	
	AudioSource laser;
	
	void Start () {
		laserBullet = transform.FindChild("LaserBullet");
		
		AudioSource[] sources = GetComponents<AudioSource>();
		laser = sources[0];
	}
	
	void Update () {
		if (LookAt.playerDistance < 60) {
			laserBullet.gameObject.renderer.enabled = true;
			if (!laser.isPlaying) {
				laser.Play();
			}
		} else {
			laser.Stop();
			laserBullet.gameObject.renderer.enabled = false;
		}
	}
}
