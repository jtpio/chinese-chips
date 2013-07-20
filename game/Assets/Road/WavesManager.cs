using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class WavesManager : MonoBehaviour {
	
	// globals
	public float waveTime;
	public float restTime;
	public int itemsPerTile;
	
	// mine
	public int nbMines;
	public Vector3 yOffsetMine;

	// prefabs
	public Transform minePrefab;
	
	protected float time;
	protected float waveStartTime;
	protected Countdown countdown;
	protected RoadManager roadManager;
	protected LinkedListNode<Node> lastPlayerNode;
	
	
	void Start () {
		time = 0;
		waveStartTime = waveTime;
		countdown = GameObject.Find("GameStuff").GetComponent<Countdown>();
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		lastPlayerNode = roadManager.playerNode;
	}
	
	void Update () {
		time += Time.deltaTime;
		
		bool inWave = time > waveStartTime;
		
		if (inWave && lastPlayerNode != roadManager.playerNode) {
			int nbItems = Random.Range(1,itemsPerTile+1);
			Vector3 spawningPos = roadManager.playerNode.Next.Next.Value.position;
			for (int i = 0; i < nbItems; i++) {
				Vector3 itemPos = new Vector3(Random.Range(0, 20f), 0, Random.Range(0, 20f));
				itemPos += spawningPos + yOffsetMine;
				Instantiate(minePrefab, itemPos, Quaternion.identity);
			}
			lastPlayerNode = roadManager.playerNode;
		}
		
		
	}
}
