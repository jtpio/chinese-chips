using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class WavesManager : MonoBehaviour {
	
	// globals
	public float waveDuration;
	public float restDuration;
	public int itemsPerTile;
	public int countdownDuration;
	
	// mine
	public int nbMines;
	public Vector3 yOffsetMine;

	// prefabs
	public Transform minePrefab;
	public Transform coinPrefab;
	
	// static
	public static Status status;
	
	// protected
	protected float time;
	protected float waveStartTime;
	protected Countdown countdown;
	protected RoadManager roadManager;
	protected LinkedListNode<Node> lastPlayerNode;
	
	
	void Start () {
		time = 0;
		waveStartTime = restDuration;
		countdown = GameObject.Find("GameStuff").GetComponent<Countdown>();
		countdown.SetCountdownDuration(countdownDuration);
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		lastPlayerNode = roadManager.playerNode;
		status = Status.Rest;
	}
	
	void Update () {
		time += Time.deltaTime;
		
		bool inWave = (time > waveStartTime) && (time < waveStartTime + waveDuration);
		
		float timeBeforeWave = waveStartTime - time;
		if (timeBeforeWave < countdown.duration && timeBeforeWave > 0 && !countdown.IsPlaying()) {
			countdown.StartTimer();	
		}
		
		if (time > waveStartTime) {
			status = Status.Wave;	
		}
		
		if (inWave && lastPlayerNode != roadManager.playerNode) {
			int nbItems = Random.Range(1, itemsPerTile+1);
			Vector3 spawningPos = roadManager.playerNode.Next.Next.Value.position;
			for (int i = 0; i < nbItems; i++) {
				Vector3 itemPos = spawningPos + new Vector3(Random.Range(0, 20f), 0, Random.Range(0, 20f));
				Instantiate(minePrefab, itemPos, Quaternion.identity);
			}
			lastPlayerNode = roadManager.playerNode;
		}
		
		if (time >= waveStartTime + waveDuration) {
			waveStartTime = waveStartTime + waveDuration + restDuration;
			status = Status.Rest;
		}
	}
}
