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
	
	public float speedFov;
	
	protected float initFov;
	
	// items
	public Transform[] itemPrefabs;
	public float[] spawnRates;
	public int[] nbItems;
	
	// static
	public static Status status;
	
	// protected
	protected float time;
	protected float waveStartTime;
	public static int waveCounter;
	protected Countdown countdown;
	protected RoadManager roadManager;
	protected LinkedListNode<Node> lastPlayerNode;
	
	void Start () {
		time = 0;
		waveStartTime = restDuration;
		waveCounter = 1;
		countdown = GameObject.Find("GameStuff").GetComponent<Countdown>();
		countdown.SetCountdownDuration(countdownDuration);
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		lastPlayerNode = roadManager.playerNode;
		status = Status.Rest;
		initFov = Camera.main.fov;
	}
	
	void Update () {
		
		if (status == Status.GameOver) return;
		
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
			Camera.main.fov = speedFov;
			Scoring.scoring = true;
			//for (int i = 0; i < itemPrefabs.Length; i++) {
			if (roadManager.playerNode.Next.Next.Value.type != NodeType.PortalIn && roadManager.playerNode.Next.Next.Value.type != NodeType.PortalOut) {
				int nbItems = itemsPerTile;
				int i = Random.Range(0, itemPrefabs.Length);
				Vector3 spawningPos = roadManager.playerNode.Next.Next.Value.position;
				
				for (int j = 0; j < nbItems; j++) {
					Vector3 itemPos = spawningPos;
					if (i == 1) { // turret
						itemPos += new Vector3(MathUtils.RandomSign() * 23f, 0, MathUtils.RandomSign() * 23f);
					} else {
						itemPos += new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
					}
					
					Instantiate(itemPrefabs[i], itemPos, Quaternion.identity);
				}
			}
			lastPlayerNode = roadManager.playerNode;
		}
		
		if (time >= waveStartTime + waveDuration) {
			waveStartTime = waveStartTime + waveDuration + restDuration;
			status = Status.Rest;
			Camera.main.fov = initFov;
			waveCounter++;
		}
	}
}
