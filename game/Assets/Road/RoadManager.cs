using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	// prefab
	public Transform roadStraightPrefab;
	public Transform roadTurnPrefab;
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
	protected Vector3 roadTileSize;
	
	protected LinkedListNode<Node> playerNode;
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
		playerNode = roadNodes.First;
		middleNode = roadNodes.First;
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
				//if (MathUtils.RandomBool()) transform.renderer.enabled = false;
//				Destroy(transform.FindChild("LeftPanel0"+Random.Range(1, 4)).gameObject);
//				Destroy(transform.FindChild("RightPanel0"+Random.Range(1, 4)).gameObject);
				StepForward(direction, roadTileSize);
				nodeTime += roadTileSize.z;
			}
			
			bool turn = MathUtils.RandomBool();
			if (turn) { // turn
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
				//transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
				NodeType type = (turn)?NodeType.Left:NodeType.Right;
				Node node = new Node(spawningPoint, direction, nodeTime, transform, type);
				roadNodes.AddLast(node);
				nbGenerated++;
				//if (MathUtils.RandomBool()) transform.renderer.enabled = false;
				StepForward(direction, roadTileSize);
				nodeTime += roadTileSize.z;
			} else {
				Transform inPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				Node node = new Node(spawningPoint, direction, nodeTime, inPortal, NodeType.PortalIn);
				roadNodes.AddLast(node);
				nbGenerated++;
				direction = MathUtils.RandomTurn(direction);
				StepForward(direction, roadTileSize);
				
				// second portal with y offset
				StepUpward(direction, roadTileSize);
				Transform outPortal = Instantiate(portalPrefab, spawningPoint, Quaternion.LookRotation(direction)) as Transform;
				node = new Node(spawningPoint, direction, nodeTime, outPortal, NodeType.PortalOut);
				roadNodes.AddLast(node);
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
		float time = movePlayer.time;	
		if (time > playerNode.Next.Value.time) {
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
