﻿using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

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

}
