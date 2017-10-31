using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button optionsText;
	public Button exitText;

	// Use this for initialization
	void Start () 
	
	{
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		optionsText = optionsText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		quitMenu.enabled = false;

	}

	public void ExitPress()
	
	{
		quitMenu.enabled = true;
		startText.enabled = false;
		optionsText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress()
	
	{
		quitMenu.enabled = false;
		startText.enabled = true;
		optionsText.enabled = true;
		exitText.enabled = true;
	}

	public void StartLevel()

	{
		Application.LoadLevel (1);
	}

	public void OptionsMenu()
		
	{
		Application.LoadLevel (2);
	}

	public void ExitGame ()

	{
		Application.Quit ();
	}
}