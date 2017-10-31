using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class optionsScript : MonoBehaviour {

	public Canvas soundMenu;
	public Canvas videoMenu;
	public Canvas keybindingsMenu;

	// Use this for initialization
	void Start () 
	{
		//soundMenu = soundMenu.GetComponent<Canvas> ();
		//videoMenu = videoMenu.GetComponent<Canvas> ();
		//keybindingsMenu = keybindingsMenu.GetComponent<Canvas> ();
	}
	
	public void MainMenu()

	{
		Application.LoadLevel (0);
	}
}