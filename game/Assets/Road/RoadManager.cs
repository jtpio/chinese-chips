using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	public Transform roadPrefab;
	public Transform portalPrefab;
	public int timeStep;
	public int roadSegmentLength;
	public int nbSegments;
	public int yDelta;
	
	protected Settings settings;
	protected MovePlayer movePlayer;
	protected LinkedList<Node> roadNodes;
	protected Vector3 spawningPoint;
	protected Vector3 direction;
	protected float nodeTime;
	
	protected LinkedListNode<Node> playerNode;
	protected LinkedListNode<Node> middleNode;
	protected TimeDirection timeDirection;
	
	void Awake() {
		settings = GameObject.Find("GameStuff").GetComponent<Settings>();
		movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
		timeDirection = new TimeDirection();
	}
	
	void Start () {
		GenerateTrack();
	}
	
	void Update () {
		HandlePlayer();
	}
	
	protected void Step(Vector3 size, Vector3 direction) {
		
	}
	
	protected void ShiftMiddleNode(int offset) {
		for (int i = 0; i < offset; i++) {
			middleNode = middleNode.Next;
		}
	}
	
	protected void GenerateTrack() {
		roadNodes = new LinkedList<Node>();
		nodeTime = 0;
		spawningPoint = settings.startPosition;
		direction = settings.startDirection;
		
		GenerateSegments();
		playerNode = roadNodes.First;
		middleNode = roadNodes.First;
		ShiftMiddleNode(roadNodes.Count / 2);
		
		/*
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
		
		*/	
	}
	
	protected int GenerateSegments() {
		int nbGenerated = 0;
		Vector3 size = roadPrefab.renderer.bounds.size;
		for (int i = 0; i < nbSegments; i++) {
			for (int j = 0; j < roadSegmentLength; j++) {
				Transform transform = Instantiate(roadPrefab, spawningPoint, Quaternion.identity) as Transform;
				Node node = new Node(spawningPoint, direction, nodeTime, transform, NodeType.Normal);
				roadNodes.AddLast(node);
				nbGenerated++;
				//if (MathUtils.RandomBool()) transform.renderer.enabled = false;
				StepForward(direction, size);
				nodeTime += size.x;
			}
			
			
			bool turn = MathUtils.RandomBool();
			if (turn) { // turn
				direction = MathUtils.RandomTurn(direction);
				Transform transform = Instantiate(roadPrefab, spawningPoint, Quaternion.identity) as Transform;
				NodeType type = (turn)?NodeType.Left:NodeType.Right;
				Node node = new Node(spawningPoint, direction, nodeTime, transform, type);
				roadNodes.AddLast(node);
				nbGenerated++;
				//if (MathUtils.RandomBool()) transform.renderer.enabled = false;
				StepForward(direction, size);
				nodeTime += size.x;
			} else {
				Transform inPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Node node = new Node(spawningPoint, direction, nodeTime, inPortal, NodeType.PortalIn);
				roadNodes.AddLast(node);
				nbGenerated++;
				direction = MathUtils.RandomTurn(direction);
				StepForward(direction, size);
				
				// second portal with y offset
				StepUpward(direction, size);
				Transform outPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				node = new Node(spawningPoint, direction, nodeTime, outPortal, NodeType.PortalOut);
				roadNodes.AddLast(node);
				nbGenerated++;
				StepForward(direction, size);
				nodeTime += size.x;
			}
		}
		
		return nbGenerated;
	}
	
	protected void StepForward(Vector3 direction, Vector3 size) {
		spawningPoint += Vector3.Scale(size, direction);
	}
	
	protected void StepUpward(Vector3 direction, Vector3 size) {
		spawningPoint += MathUtils.RandomSign() * yDelta * Vector3.up;
	}
	
	public void HandlePlayer() {
		float time = movePlayer.time;	
		if (time > playerNode.Next.Value.time - 5) {
			playerNode = playerNode.Next;
			if (playerNode.Previous.Value.type == NodeType.PortalIn) {
				movePlayer.time = playerNode.Value.time;
				movePlayer.SetPosition(playerNode.Value.position);
				movePlayer.InstantRotation(playerNode.Value.direction);
			}
			movePlayer.direction = playerNode.Value.direction;
			Recycle();
		}
	}
	
	public void Recycle() {
		if (playerNode.Value.time > middleNode.Value.time) {
			middleNode = middleNode.Next;
			Node toRemove = roadNodes.First.Value;
			Destroy(toRemove.transform.gameObject);
			roadNodes.RemoveFirst();
			
			int nbNewParts = GenerateSegments();
			ShiftMiddleNode(nbNewParts);
		}
	}
}
