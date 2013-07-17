using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class RoadManager : MonoBehaviour {
	
	public Transform roadPrefab;
	public Transform portalPrefab;
	
	protected LinkedList<Transform> roadTransforms;
	protected Vector3 spawningPoint;
	
	void Start () {
		roadTransforms = new LinkedList<Transform>();
		spawningPoint = new Vector3(0,0,0);
		
		Vector3 size = roadPrefab.renderer.bounds.size;
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
	}
	
	void Update () {
		
	}

}
