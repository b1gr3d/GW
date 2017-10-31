using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pilot : Person {
	public int flight, focus, combat;

	public Pilot() {
		level = 1;
		exp = 0;
		flight = Random.Range (0, 2);
		focus = Random.Range (0, 2);
		combat = Random.Range (0, 5);
	}

	public void levelUp(){
		level = level++;
	}

	// pilot status
	/*public bool pilotALive(){
		if (health > 0) {
			return false;
		} else {
			return true;
		}
	}*/


}
