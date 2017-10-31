using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	[SerializeField] bool isXAxis = false, isYAxis = false;
	[SerializeField] GameObject target = null;
	[SerializeField] float speed = 5f;
	private Vector3 point;
	private Vector3 original;
	
	private float ZoomAmount = 0f; //With Positive and negative values
	[SerializeField] float MaxToClamp = 10;
	
	// Use this for initialization
	void Start () {
		point = target.transform.position;
		transform.LookAt (point);
		original = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)) {
			// Debug.Log ("onMouseButton clicked.");
			if (isYAxis) 
				transform.RotateAround (point, new Vector3 (1.0f, 0.0f, 0.0f), Input.GetAxis("Mouse Y") * -speed);
			if (isXAxis)
				transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), Input.GetAxis("Mouse X") * speed);
		}
		
		ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
		ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
		float translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp - Mathf.Abs(ZoomAmount));
		gameObject.transform.Translate(0,0,translate * speed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
	}

	public void resetLookAt() {
		transform.LookAt (original);
	}

	public void setLookAt(Vector3 pos) {
		point = pos;
	}
}