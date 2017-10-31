using UnityEngine;
using System.Collections;

public class TileUI : MonoBehaviour {
	[SerializeField] GameObject targetUI = null;

	public void SetActiveUI(bool status) {
		if (targetUI != null) {
			targetUI.SetActive (status);
		}
	}
}
