using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Person : MonoBehaviour {
	[SerializeField] protected int level;
	[SerializeField] protected int exp;
	int damage { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setExp(int exp){
		this.exp = exp;
	}

	public int getExp(){
		return this.exp;
	}

	public void setLevel(int level){
		this.level = level;
	}

	public int getLevel(){
		return this.level;
	}

	public void levelUp(){
		level = level++;
	}
}
