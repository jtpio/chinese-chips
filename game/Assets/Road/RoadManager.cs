using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	public Transform roadPrefab;
	public float interpolateStep;
	public float generateDelay;
	public float roadWidth;
	public float checkPointStep;
	public float xStep;
	public float roadVariation;
	public int initialRoadPartNumber;
	
	protected LinkedListNode<ControlPoint> cpPointer;
	protected float lastTime;
	protected float checkPointTime;
	
	protected LinkedList<ControlPoint> controlPoints;
	protected LinkedList<Transform> roadTransforms;
	protected Matrix4x4 lastMatrix;
	
	// player position
	protected LinkedListNode<ControlPoint> playerControlPoint;
	
	void Start () {
		controlPoints = new LinkedList<ControlPoint>();
		roadTransforms = new LinkedList<Transform>();
		lastMatrix = Matrix4x4.identity;
		checkPointTime = 0;
		
		// Temporary control points
		Vector3 pos0 = new Vector3(0,0,0);
		
		controlPoints.AddLast(new ControlPoint(
			pos0,
			Quaternion.identity,
			checkPointTime
		));
		cpPointer = controlPoints.First;
		checkPointTime += checkPointStep;
		playerControlPoint = cpPointer;
		
		for (int i = 0; i < initialRoadPartNumber + 3; i++) {
			ControlPoint lastCP = controlPoints.Last.Value;
			Vector3 newPos = new Vector3(lastCP.pos.x + xStep, Mathf.Round(Random.Range(-roadVariation, roadVariation) / 5.0f), 0);
			controlPoints.AddLast(new ControlPoint(
				newPos,
				Quaternion.identity,
				checkPointTime
			));	
			checkPointTime += checkPointStep;
		}
		
		for (int i = 0; i < initialRoadPartNumber; i++) {
			GenerateRoadPart(cpPointer);
			cpPointer = cpPointer.Next;	
		}
		
		lastTime = 0;
	}
	
	void Update () {
		
	}
	
	void CreateNewRoadPart() {
		ControlPoint lastCP = controlPoints.Last.Value;
		Vector3 newPos = new Vector3(lastCP.pos.x + xStep, lastCP.pos.y + Mathf.Round(Random.Range(-roadVariation, roadVariation)), lastCP.pos.z + Mathf.Round(Random.Range(-roadVariation, roadVariation)));
		controlPoints.AddLast(new ControlPoint(
			newPos,
			Quaternion.identity,
			checkPointTime
		));
		checkPointTime += checkPointStep;
		
		GenerateRoadPart(cpPointer);
		cpPointer = cpPointer.Next;
		lastTime = Time.time;
	}
	
	void GenerateRoadPart(LinkedListNode<ControlPoint> node) {	
		// compute sections
		List<SplinePos> sections = new List<SplinePos>();
		for (float t = 0; t <= 1.0f + interpolateStep; t += interpolateStep) {
			SplinePos section = new SplinePos();
			section.point = Interpolate(node, t);
			section.tangent = InterpolateTangent(node, t);
			section.normal = InterpolateNormal(node, t);
			if (sections.Count > 0) {
				//Debug.DrawLine(section.point, sections[sections.Count-1].point, Color.red, 100);
			}
			//Debug.DrawLine(section.point, section.point + section.normal * 1.5f, Color.green, 100);
			sections.Add(section);
		}
		//sections.Reverse();
		
		Quaternion rot = Quaternion.LookRotation(sections[0].point - sections[1].point, Vector3.up);
		Transform roadPart = Instantiate(roadPrefab, sections[0].point, rot) as Transform;
		MeshFilter meshFilter = roadPart.GetComponent<MeshFilter>();
		Mesh srcMesh = meshFilter.sharedMesh;
		MeshExtrusion.Edge[] precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
		
		// compute matrices
		Matrix4x4[] finalSections = new Matrix4x4[sections.Count];
		Matrix4x4 worldToLocal = roadPart.transform.worldToLocalMatrix;
		Quaternion previousRotation = roadPart.rotation;
		Vector3 scaleVector = new Vector3(roadWidth, 0.5f, 1) / 3.5f;
		for (int i = 0; i < sections.Count; i++) {
			if (i < sections.Count - 1) {
				Vector3 direction = sections[i].point - sections[i+1].point;
				Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
				if (i != 0) {
					if (Quaternion.Angle(previousRotation, rotation) > 20) {
						rotation = Quaternion.Slerp(previousRotation, rotation, 0.1f);
					}
					previousRotation = rotation;	
				}
				finalSections[i] = worldToLocal * Matrix4x4.TRS(sections[i].point, rotation, scaleVector);
			} else {
				finalSections[i] = finalSections[i-1];
			}
		}
	
		//finalSections[sections.Count] = lastMatrix;
		
		// extrude
		MeshExtrusion.ExtrudeMesh(srcMesh, meshFilter.mesh, finalSections, precomputedEdges, false);
		roadTransforms.AddLast(roadPart);
		
		roadPart.gameObject.AddComponent<MeshCollider>();
		
		//lastMatrix = finalSections[0];
	}
	
	// used by the player to know where it is
	public SplinePos GetPositonAtTime(float time) {	
		SplinePos res = new SplinePos();
		if (time >= controlPoints.Last.Value.time) {
			res.point = controlPoints.Last.Value.pos;	
			return res;
		}
		
		if (time > playerControlPoint.Next.Value.time) {
			playerControlPoint = playerControlPoint.Next;	
		}
		
		float t = (time - playerControlPoint.Value.time) / (playerControlPoint.Next.Value.time - playerControlPoint.Value.time);
		
		res.point = Interpolate(playerControlPoint, t);
		res.tangent = InterpolateTangent(playerControlPoint, t);
		res.normal = InterpolateNormal(playerControlPoint, t);
		
		return res;
	}
	
	/* time: between 0 and 1 */
	public Vector3 Interpolate(LinkedListNode<ControlPoint> node, float time) {
		Vector3 point = SplineInterpolator.Hermite(
			time,
			(node == controlPoints.First)? node.Value.pos:node.Previous.Value.pos,
			node.Value.pos,
			node.Next.Value.pos,
			node.Next.Next.Value.pos
		);
		
		return point;
	}
	
	/* time: between 0 and 1 */
	public Vector3 InterpolateTangent(LinkedListNode<ControlPoint> node, float time) {
		Vector3 tangent = SplineInterpolator.HermiteTangent(
			time,
			(node == controlPoints.First)? node.Value.pos:node.Previous.Value.pos,
			node.Value.pos,
			node.Next.Value.pos,
			node.Next.Next.Value.pos
		);
		
		return tangent;
	}
	
	/* time: between 0 and 1 */
	public Vector3 InterpolateNormal(LinkedListNode<ControlPoint> node, float time) {
		Vector3 tangent = SplineInterpolator.HermiteNormal(
			time,
			(node == controlPoints.First)? node.Value.pos:node.Previous.Value.pos,
			node.Value.pos,
			node.Next.Value.pos,
			node.Next.Next.Value.pos
		);
		
		return tangent;
	}
	
	public void Recycle(float time) {
		if (time > controlPoints.First.Next.Next.Next.Value.time) {
			controlPoints.RemoveFirst();
			Destroy(roadTransforms.First.Value.gameObject);
			roadTransforms.RemoveFirst();
			
			// create a new one
			CreateNewRoadPart();
		}
	}
}
