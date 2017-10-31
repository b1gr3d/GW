using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EngineeringShip : PlayerShips {

	void Start () {
		health = 15;
		armor = 5;
		shield = health * 2;
		energy = 5;
		range = 3;
		level = 1;

		HealthBar.maxValue = health;
		HealthBar.value = health;
	}
}
