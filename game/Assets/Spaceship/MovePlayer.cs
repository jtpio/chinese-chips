using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour {
	
	// parameters
	public float speed;
	public float offsetSpeed;
	public float maxOffset;
	public float turnSpeed;
	public float maxTiltAngle;
	public float tiltRotationSpeed;
	public float tiltAnimationSpeed;
	public bool invertTilt;
	
	// internals public
	public Vector3 yOffset;
	public float time;
	public Vector3 direction;
	public float offset;
	
	public static Vector3 position;
	
	public Vector3 basePosition;
	protected RoadManager roadManager;
	protected Transform ship;
	
	protected float initSpeed;
	
	// tilt
	protected float tiltAngle;
	protected float tiltTime;
	
	AudioSource engineStart;
	AudioSource engineSound;
	
	void Start () {
		
		AudioSource[] sources = GetComponents<AudioSource>();
		engineSound = sources[2];
		engineStart = sources[3];
		
		roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
		ship = transform.FindChild("Ship");
		time = 0;
		direction = transform.forward * speed;
		offset = 0;
		basePosition = new Vector3(0, 0, 0);
		tiltTime = 0;
		initSpeed = speed;
		speed = initSpeed / 1.5f;
		
		engineStart.Play();
		engineSound.loop = true;
		engineSound.Play();	
	}
	
	void Update () {	
		if (WavesManager.status == Status.Rest) {
			speed = initSpeed / 1.5f;
		} else if (WavesManager.status == Status.GameOver) {
			speed = initSpeed / 3;
		} else {
			speed = initSpeed;
		}
		
		time += Time.deltaTime;
		basePosition += direction * Time.deltaTime;

		Quaternion newRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed);
		transform.rotation = rotation;
		
		float xTouch =  -1;
		if (WavesManager.status != Status.GameOver) {
			if (Application.platform == RuntimePlatform.Android && Input.touchCount == 1) {
				xTouch =  Input.GetTouch(0).position.x;
			}
			
			if ((xTouch >= 0 && xTouch < Screen.width / 2) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) {
				offset -= offsetSpeed * Time.deltaTime;
				tiltAngle -= tiltRotationSpeed * Time.deltaTime;
				tiltTime += tiltAnimationSpeed * Time.deltaTime;
			} else if ((xTouch >= 0 && xTouch >= Screen.width / 2) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
				offset += offsetSpeed * Time.deltaTime;
				tiltAngle += tiltRotationSpeed * Time.deltaTime;
				tiltTime += tiltAnimationSpeed * Time.deltaTime;
			} else {
				tiltTime -= tiltAnimationSpeed * Time.deltaTime;	
			}
		}

		offset = Mathf.Clamp(offset, -maxOffset, maxOffset);
		tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
		
		tiltTime = Mathf.Clamp(tiltTime, 0, 1);
		float tAngle = tiltAngle * Tween.Easing.EaseIn(tiltTime, Tween.EasingType.Sine);
		
		transform.position = basePosition + offset * transform.right + yOffset;
		ship.transform.rotation = Quaternion.AngleAxis(((invertTilt)?-1:1) * tAngle, transform.forward) * transform.rotation;
		Camera.main.transform.rotation = Quaternion.AngleAxis(((invertTilt)?-1:1) * tAngle * 0.2f, transform.forward) * transform.rotation;
		position = transform.position;
	}
	
	public void SetPosition(Vector3 pos) {
		basePosition = pos;	
	}
	
	public void SetMoveVector(Vector3 moveVector) {
		direction = moveVector.normalized * speed;
	}
	
	public void InstantRotation(Vector3 direction) {
		this.direction = direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
