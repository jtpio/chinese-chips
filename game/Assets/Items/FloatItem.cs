using UnityEngine;
using System.Collections;

public class FloatItem : MonoBehaviour {
	
	public float yMaxOffset;
	public float yInitOffset;
	public float moveSpeed;
	public float lifeTime;
	
	protected float time;

	void Start () {
		time = 0;
		StartCoroutine(WaitAndDie(lifeTime));
	}
	
	void Update () {
		time += Time.deltaTime * moveSpeed;
		float yOffset = Mathf.PingPong(time, yMaxOffset) + yInitOffset;
		transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
	}
	
	void OnTriggerEnter(Collider other) {
		Destroy(gameObject);
	}
	
	IEnumerator WaitAndDie(float lifeTime) {
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
