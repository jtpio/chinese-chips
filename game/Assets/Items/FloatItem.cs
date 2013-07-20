using UnityEngine;
using System.Collections;

public class FloatItem : MonoBehaviour {
	
	public float yMaxOffset;
	public float yInitOffset;
	public float moveSpeed;
	public float lifeTime;
	
	protected float time;
	protected Vector3 basePosition;

	void Start () {
		time = 0;
		basePosition = transform.position;
		StartCoroutine(WaitAndDie(lifeTime));
	}
	
	void Update () {
		time += Time.deltaTime * moveSpeed;
		float yOffset = basePosition.y + Mathf.PingPong(time, yMaxOffset) + yInitOffset;
		transform.position = new Vector3(basePosition.x, yOffset, basePosition.z);
	}
	
	void OnTriggerEnter(Collider other) {
		//Debug.Log ("collision between " + other.name + " and " + gameObject.name);
		
		if (other.name == "Player") {
			
			if (name == "Coin(Clone)") {
				// do things
			} else {
				Destruction destr = other.GetComponent<Destruction>();
				if (destr) destr.DestroyPart();
			}
		}
		
		Destroy(gameObject);
	}
	
	IEnumerator WaitAndDie(float lifeTime) {
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
