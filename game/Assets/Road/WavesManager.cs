using UnityEngine;
using System.Collections;
using Utils;

public class WavesManager : MonoBehaviour {
	
	public int wavesPeriod;
	public int nbBlocks;
	public Transform blockPrefab;
	
	protected Countdown countdown;
	protected Timer timer;
	
	void Start () {
		countdown = GameObject.Find("GameStuff").GetComponent<Countdown>();
		timer = new Timer(wavesPeriod);
	}
	
	void FloodBlocks() {
		Vector3 pos = MovePlayer.position;
		for (int i = 0; i < nbBlocks; i++) {
			Vector3 blockPos = new Vector3(pos.x + 100 + Random.Range(25, 100), pos.y + 20, pos.z + Random.Range(-20, 20));
			Transform block = Instantiate(blockPrefab, blockPos, Quaternion.identity) as Transform;
			block.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 10);
		}
	}
	
	void Update () {
		if (timer.IsFinished()) {
			countdown.StartTimer(new Countdown.CallBack(this.FloodBlocks));
		} else {
			timer.Update(Time.deltaTime);	
		}
	}
}
