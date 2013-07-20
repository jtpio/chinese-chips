using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	// prefab
	public Transform roadStraightPrefab;
	public Transform roadTurnPrefab;
	public Transform portalPrefab;
	
	// parameters
	public int timeStep;
	public int roadSegmentLength;
	public int nbSegments;
	public int yDelta;
	public float waypointDistance;
	public float fallingRoadChance;
	
	protected Settings settings;
	protected MovePlayer movePlayer;
	protected LinkedList<Node> roadNodes;
	protected Vector3 spawningPoint;
	protected Vector3 direction;
	protected float nodeTime;
	protected Vector3 roadTileSize;
	
	public LinkedListNode<Node> playerNode;
	protected LinkedListNode<Node> middleNode;
	
	void Awake() {
		settings = GameObject.Find("GameStuff").GetComponent<Settings>();
		movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
		roadTileSize = roadStraightPrefab.FindChild("baseSquare").renderer.bounds.size + new Vector3(0,0,10);
	}
	
	void Start () {
		GenerateTrack();
	}
	
	void Update () {
		HandlePlayer();
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
		playerNode = roadNodes.First.Next;
		middleNode = roadNodes.First.Next;
		ShiftMiddleNode(roadNodes.Count / 2);
	}
	
	protected int GenerateSegments() {
		int nbGenerated = 0;
		for (int i = 0; i < nbSegments; i++) {
			for (int j = 0; j < roadSegmentLength; j++) {
				Transform transform = Instantiate(roadStraightPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Node node = new Node(spawningPoint, direction, nodeTime, transform, NodeType.Normal);
				roadNodes.AddLast(node);
				nbGenerated++;
				StepForward(direction, roadTileSize);
				nodeTime += roadTileSize.z;
			}
			
			bool turn = MathUtils.RandomBool();
			if (turn) {
				bool left = MathUtils.RandomBool();
				Vector3 prefabOrientation = direction;
				if (left) {
					direction = MathUtils.RotateLeft(direction);
					prefabOrientation = direction;
				} else {
					direction = MathUtils.RotateRight(direction);
					prefabOrientation = MathUtils.RotateRight(direction);
				}
				Transform transform = Instantiate(roadTurnPrefab, spawningPoint, Quaternion.LookRotation(prefabOrientation)) as Transform;
				NodeType type = (turn)?NodeType.Left:NodeType.Right;
				Node node = new Node(spawningPoint, direction, nodeTime, transform, type);
				roadNodes.AddLast(node);
				nbGenerated++;
				StepForward(direction, roadTileSize);
				nodeTime += roadTileSize.z;
			} else {
				Transform inPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Node inP = new Node(spawningPoint, direction, nodeTime, inPortal, NodeType.PortalIn);
				roadNodes.AddLast(inP);
				nbGenerated++;
				direction = MathUtils.RandomTurn(direction);
				StepForward(direction, roadTileSize);
				
				// second portal with y offset
				StepUpward(direction, roadTileSize);
				Transform outPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Node outP = new Node(spawningPoint, direction, nodeTime, outPortal, NodeType.PortalOut);
				roadNodes.AddLast(outP);
				nbGenerated++;
				StepForward(direction, roadTileSize);
				nodeTime += roadTileSize.z;
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
		if (roadNodes.Count < 1) return;
		Vector3 moveVector = playerNode.Value.position - movePlayer.basePosition;
		float dist = waypointDistance;
		//if (playerNode.Value.type == NodeType.Left || playerNode.Value.type == NodeType.Right) dist = 20.0f;
		if (moveVector.magnitude < dist) {
			playerNode = playerNode.Next;
			
			if (WavesManager.status == Status.Wave) {
				DropPanels(playerNode.Next.Value.transform);
			}
			
			if (playerNode.Previous.Value.type == NodeType.PortalIn) {
				movePlayer.time = playerNode.Value.time;
				movePlayer.SetPosition(playerNode.Value.position);
				movePlayer.InstantRotation(playerNode.Value.direction);
			}
			//movePlayer.direction = playerNode.Value.direction;
			Recycle();
		}
		movePlayer.SetMoveVector(moveVector);
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
	
	protected void DropPanels(Transform roadStraight) {
		
		if (MathUtils.RandomChance(fallingRoadChance)) {
			Transform randomLeftPanel = roadStraight.FindChild("LeftPanel0"+Random.Range(1, 4));
			if (randomLeftPanel) {
				randomLeftPanel.gameObject.AddComponent<Throw>();
			}
			
			Transform randomRightPanel = roadStraight.FindChild("RightPanel0"+Random.Range(1, 4));
			if (randomRightPanel) {
				randomRightPanel.gameObject.AddComponent<Throw>();
			}
		}
		
		if (MathUtils.RandomChance(fallingRoadChance)) {
			roadStraight.gameObject.AddComponent<Fall>();
		}
		
	}
}
