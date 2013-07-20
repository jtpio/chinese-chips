using UnityEngine;
using System.Collections;
using Utils;

public class WavesManager : MonoBehaviour {
	
	// mine
	public int nbMines;
	public Vector3 yOffsetMine;
	
	// waves
	public float wavesPeriod;
	
	// prefabs
	public Transform minePrefab;
	
	protected float time;
	protected float wavesThreshold;
	protected Countdown countdown;
	protected RoadManager roadManager;
	
	void Start () {
		time = 0;
		wavesThreshold = wavesPeriod;
		countdown = GameObject.Find("GameStuff").GetComponent<Countdown>();
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
	}
	
	void Update () {
		time += Time.deltaTime;
		if (time > wavesThreshold) {
			Vector3 spawningPos = roadManager.playerNode.Next.Value.position;
			for (int i = 0; i < nbMines; i++) {
				Instantiate(minePrefab, spawningPos + roadManager.playerNode.Next.Value.direction * i * 20 + yOffsetMine, Quaternion.identity);
			}
			wavesThreshold += wavesPeriod;
		}
	}
}
