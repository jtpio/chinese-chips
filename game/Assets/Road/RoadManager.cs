using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	public Transform roadPrefab;
	public Transform portalPrefab;
	public int roadSegmentLength;
	public int nbSegments;
	
	protected LinkedList<Transform> roadTransforms;
	protected Vector3 spawningPoint;
	protected List<Node> roadPoints;
	protected float distance;
	protected int playerPos;

	void Start () {
		roadTransforms = new LinkedList<Transform>();
		spawningPoint = new Vector3(0,0,0);
		roadPoints = new List<Node>();
		distance = 0;
		playerPos = 0;
		Vector3 size = roadPrefab.renderer.bounds.size;
		
		/*
		for (int i = 0; i < 10; i++) {
			Instantiate(roadPrefab, spawningPoint, Quaternion.identity);
			spawningPoint += Vector3.Scale(size, Vector3.forward);
		}
		
		Transform inPortal = Instantiate(portalPrefab, spawningPoint - Vector3.Scale(size, Vector3.forward), Quaternion.identity) as Transform;
		Teleport tpIN = inPortal.gameObject.AddComponent<Teleport>();
		spawningPoint += 5 * Vector3.up;
		Transform outPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.identity) as Transform;
		tpIN.outPortal = outPortal;
		
		for (int i = 0; i < 10; i++) {
			Instantiate(roadPrefab, spawningPoint, Quaternion.identity);
			spawningPoint += Vector3.Scale(size, Vector3.forward);
		}
		*/
		
		Vector3 direction = Vector3.forward;
		
		for (int i = 0; i < nbSegments; i++) {
			for (int j = 0; j < roadSegmentLength; j++) {
				Instantiate(roadPrefab, spawningPoint, Quaternion.identity);
				roadPoints.Add(new Node(spawningPoint, direction, distance));
				Step (size, direction);
			}
			
			if (MathUtils.RandomBool()) {
				Transform inPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Teleport tpIN = inPortal.gameObject.AddComponent<Teleport>();
				spawningPoint += MathUtils.RandomSign() * 5 * Vector3.up;
				Transform outPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				tpIN.outPortal = outPortal;
				Step (size, direction);
			} else {
				direction = MathUtils.RandomTurn(direction);
				
//				if (MathUtils.RandomChance(30)) {
//					spawningPoint += MathUtils.RandomSign() * 5 * Vector3.up;
//				}
			}
		}
	}
	
	void Update () {
		
	}
	
	public Vector3 GetDirectionAtDistance(float distance) {
		if (roadPoints[playerPos].distance < distance) {
			playerPos++;
		}
		return roadPoints[playerPos].direction;
	}
	
	protected void Step(Vector3 size, Vector3 direction) {
		spawningPoint += Vector3.Scale(size, direction);
		distance += Vector3.Scale(size, direction).magnitude;	
	}
	
}
