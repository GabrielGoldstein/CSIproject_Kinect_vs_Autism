using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public GameObject levelSelectPanel;

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void startGameButton(){
		Application.LoadLevel ("KinectInteractionDemo");
	}
	public void optionButton(){
	}
	public void startTutorialButton() {
		Application.LoadLevel ("Scene 1");
	}


	public void levelSelectButton() {
		//Disable Main Menu Panel
		gameObject.SetActive (false);
		//Activate LevelSelectPanel
		levelSelectPanel.SetActive(true);
	}
	public void leveltoMain(){
		//Level Select window to Main Menu
		levelSelectPanel.SetActive (false);
		gameObject.SetActive(true);
	}


	public void level1Button(){
		Application.LoadLevel ("Scene 1");
	}
	public void level2Button() {
		Application.LoadLevel ("Scene 2");
	}
	public void level3Button(){
		Application.LoadLevel ("Scene 3");
	}
	public void level4Button(){
		Application.LoadLevel ("Scene 4");
	}










}
