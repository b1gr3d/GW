using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoutShip : PlayerShips {
	public void Start() {
		health = 10;
		armor = 0;
		shield = health * 2;
		level = 1;
		energy = 5;
		range = 4;

		HealthBar.maxValue = health;
		HealthBar.value = health;

		ShieldBar.maxValue = shield;
		ShieldBar.value = shield;
	}
}
