using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {
	[SerializeField] Camera top = null, iso = null;
	bool activeCamera;

	// Use this for initialization
	void Start () {
		activeCamera = true;
		top.gameObject.SetActive (activeCamera);
		iso.gameObject.SetActive (!activeCamera);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void switchView() {
		activeCamera = !activeCamera;
		top.gameObject.SetActive (activeCamera);
		iso.gameObject.SetActive (!activeCamera);
	}

	public void setIsoView() {
		top.gameObject.SetActive (false);
		iso.gameObject.SetActive (true);
	}

	public void setTopView() {
		top.gameObject.SetActive (true);
		iso.gameObject.SetActive (false);
	}
}
