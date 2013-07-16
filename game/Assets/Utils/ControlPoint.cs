using UnityEngine;
using System.Collections;

public class ControlPoint {

	public Vector3 pos;
	public Quaternion rot;
	public float time;
	public Vector2 easeIO;
	
	public ControlPoint(Vector3 p, Quaternion q, float t) {
		pos = p;
		rot = q;
		time = t;
		easeIO = new Vector2(0, 1);
	}
	
	public ControlPoint(Vector3 p, Quaternion q, float t, Vector2 io) {
		pos = p;
		rot = q;
		time = t;
		easeIO = io;
	}
	public ControlPoint(ControlPoint o) {
		pos = o.pos;
		rot = o.rot;
		time = o.time;
		easeIO = o.easeIO;
	}
}
