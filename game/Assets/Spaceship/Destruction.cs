using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destruction : MonoBehaviour {
	
	Transform ship;
	List<string> jets;
	List<string> doors;
	int damaged;
	Transform doorsChild;
	
	public Transform smokePrefab;
	
	public AudioSource coinSound;
	public AudioSource mine;
		
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		coinSound = sources[0];
		mine = sources[1];
		
		ship = transform.FindChild("Ship");
		jets = new List<string>();
		doors = new List<string>();
		
		// jets
		for (int i = 1; i <=3; i++) {
			jets.Add("Jet"+i);	
		}
		
		// doors
		for (int i = 1; i <=12; i++) {
			doors.Add("Door"+i);	
		}

		damaged = 0;

		doorsChild = ship.Find("Doors");
	}
	
	void Update () {

	}
	
	public void DestroyPart() {
		if (doors.Count > 0) {
			int randomDoor = Random.Range(0, doors.Count);
			Transform child = doorsChild.FindChild(doors[randomDoor]);
			Destroy(child.gameObject);
			doors.RemoveAt(randomDoor);
			damaged++;
		}
		
		if (damaged % 3 == 0) {
			if (jets.Count > 0) {
				Transform jet = ship.FindChild(jets[0]);
				Vector3 pos = jet.position;
				Quaternion rot = jet.rotation;
				Transform smoke = Instantiate(smokePrefab, pos, Quaternion.LookRotation(transform.forward)) as Transform;
				smoke.parent = ship;
				smoke.position = jet.position;
				smoke.localPosition = jet.localPosition;
				
				Destroy(jet.gameObject);
				jets.RemoveAt(0);
				
				if (jets.Count < 2) {
					Transform roof = doorsChild.FindChild("Roof");
					if (roof) Destroy(roof.gameObject);
				}
				
				if (jets.Count == 0) {
					WavesManager.status = Status.GameOver;	
				}
				
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		bool toDestroy = true;
		if (other.name != "Coin(Clone)") {
			DestroyPart();
			GetComponent<Explosion>().Explode();
			
			if (other.name == "LaserBullet") {
				toDestroy = false;
			} else {
				mine.Play();	
			}
			
			Scoring.multiplier = 0;
			
		} else {
			if (Scoring.multiplier == 0) Scoring.multiplier = 1;
			Scoring.multiplier *= 2;
			coinSound.Play();
		}
		
		if (toDestroy) {
			Destroy(other.gameObject);	
		}		
	}
}
