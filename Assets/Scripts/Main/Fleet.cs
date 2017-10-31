using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {
	public static Fleet instance = null;
	[SerializeField] GameObject[] fleet;
	[SerializeField] GameObject[] aiFleet;

	void Awake() {
		instance = this;
	}

	public GameObject[] getFleet() {
		return fleet;
	}

	public GameObject[] getAiFleet() {
		return aiFleet;
	}
}
