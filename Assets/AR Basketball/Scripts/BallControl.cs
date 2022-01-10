using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
	// Luc nem
	public float m_ThrowForce = 50f;

	// x va y bieu thi huong nem
	public float m_ThrowDirectionX = 0.17f;
	public float m_ThrowDirectionY = 0.67f;

	//khoang cach cua bong voi camera
	public Vector3 m_BallCameraOffset = new Vector3(0f, -1.4f, 3f);

	// nhung bien sau day mieu ta trang thai cu nem 
	private Vector3 startPosition;
	private Vector3 direction;
	private float startTime;
	private float endTime;
	private float duration;
	private bool directionChosen = false;
	private bool throwStarted = false;

	[SerializeField]
	GameObject ARCam;

	[SerializeField]
	ARSessionOrigin m_SessionOrigin;

	Rigidbody rb;

	private void Start(){
		rb = gameObject.GetComponent<Rigidbody>();
		m_SessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
		ARCam = m_SessionOrigin.transform.Find("AR Camera").gameObject;
		transform.parent = ARCam.transform;
		ResetBall();
	}

	private void Update(){

		// Bat dau nem
		if(Input.GetMouseButtonDown(0)){ 
			startPosition = Input.mousePosition;
			startTime = Time.time;
			throwStarted = true;
			directionChosen = false;
		} 
		// Ket thuc nem
		else if (Input.GetMouseButtonUp(0)) { 
			endTime = Time.time;
			duration = endTime - startTime;
			direction = Input.mousePosition - startPosition;
			directionChosen = true;
		}

		// huong duoc chon , se nem bong theo huong do
		if (directionChosen) {
			rb.mass = 1;
			rb.useGravity = true;

			rb.AddForce(
				ARCam.transform.forward * m_ThrowForce / duration +
				ARCam.transform.up * direction.y * m_ThrowDirectionY +
				ARCam.transform.right * direction.x * m_ThrowDirectionX);

			startTime = 0.0f;
			duration = 0.0f;

			startPosition = new Vector3(0, 0, 0);
			direction = new Vector3(0, 0, 0);

			throwStarted = false;
			directionChosen = false;
		}

		// reset lai bong sau khoang thoi gian 3 giay
		if(Time.time - endTime >= 3 && Time.time - endTime <= 4)
			ResetBall();

	}

	public void ResetBall(){
		rb.mass = 0;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		endTime = 0.0f;

		Vector3 ballPos = ARCam.transform.position + ARCam.transform.forward * m_BallCameraOffset.z + ARCam.transform.up * m_BallCameraOffset.y;
		transform.position = ballPos;
	}

}
