using UnityEngine;
using System.Collections;

public class SplineInterpolator {
	
	public static float TENSION = 0.5f;
	
	public static Vector3 Hermite(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		float t2 = t * t;
		float t3 = t2 * t;
		Vector3 m0 = TENSION * (p2 - p0);
		Vector3 m1 = TENSION * (p3 - p1);
		
		float a0 = 2 * t3 - 3 * t2 + 1;
		float a1 = t3 - 2 * t2 + t;
		float a2 = -2 * t3 + 3 * t2;
		float a3 = t3 - t2;
		
		return a0 * p1 + a1 * m0 + a2 * p2 + a3 * m1;
	}
	
	/* Returns normalized tangent */
	public static Vector3 HermiteTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		float t2 = t * t;
		Vector3 m0 = TENSION * (p2 - p0);
		Vector3 m1 = TENSION * (p3 - p1);
		
		float a0 = 6 * t2 - 6 * t;
		float a1 = 3 * t2 - 4 * t + 1;
		float a2 = -6 * t2 + 6 *t;
		float a3 = 3 * t2 - 2 * t;
		
		Vector3 tangent = a0 * p1 + a1 * m0 + a2 * p2 + a3 * m1;
		return tangent.normalized;
	}
	
	/* Returns normalized normal */
	public static Vector3 HermiteNormal(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		Vector3 m0 = TENSION * (p2 - p0);
		Vector3 m1 = TENSION * (p3 - p1);
		
		float a0 = 12 * t - 6;
		float a1 = 6 * t - 4;
		float a2 = -12 * t + 6;
		float a3 = 6 * t - 2;
		
		Vector3 normal = a0 * p1 + a1 * m0 + a2 * p2 + a3 * m1;
		return normal.normalized;
	}
	
	public static Vector3 CubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		float t2 = t * t;
		float t3 = t2 * t;
		float oneMinusT = 1 - t;
		float oneMinusT2 = oneMinusT * oneMinusT;
		float oneMinusT3 = oneMinusT2 * oneMinusT;
		Vector3 m0 = TENSION * (p2 - p0);
		Vector3 m1 = TENSION * (p3 - p1);
		
		float a0 = oneMinusT3;
		float a1 = 3 * oneMinusT2 * t;
		float a2 = 3 * oneMinusT * t2;
		float a3 = t3;
		
		return a0 * p1 + a1 * m0 + a2 * p2 + a3 * m1;
	}

}
