using UnityEngine;
using System.Collections;

public class FloatItem : MonoBehaviour {
	
	public float yMaxOffset;
	public float yInitOffset;
	public float moveSpeed;
	public float lifeTime;
	public bool follow;
	
	protected float time;
	protected Vector3 basePosition;

	void Start () {
		time = 0;
		basePosition = transform.position;
		StartCoroutine(WaitAndDie(lifeTime));
	}
	
	void Update () {
		if (follow) {
			basePosition.x = transform.position.x;
			basePosition.z = transform.position.z;
		}
		time += Time.deltaTime * moveSpeed;
		float yOffset = basePosition.y + Mathf.PingPong(time, yMaxOffset) + yInitOffset;
		transform.position = new Vector3(basePosition.x, yOffset, basePosition.z);
	}
	
	IEnumerator WaitAndDie(float lifeTime) {
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
