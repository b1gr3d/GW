using UnityEngine;
using System.Collections;

public class LazerScript : MonoBehaviour
{
	public float speed;
	
	void Start ()
	{
		transform.GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}