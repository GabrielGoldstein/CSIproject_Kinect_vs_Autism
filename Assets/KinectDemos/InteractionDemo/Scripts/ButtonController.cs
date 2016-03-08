using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public GameObject levelSelectPanel;

	public GameObject scorePrefab;
	public GameObject stuff;
	public Vector3 stuffVector;

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void startGameButton(){
		Application.LoadLevel ("KinectInteractionDemo");
	}
	public void startTutorialButton() {
		Application.LoadLevel ("Scene 1");
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


	public void miniGameButton(){
		Application.LoadLevel ("Mini Game DEMO");
	}
	public void videoButton(){
		//Load Video
		//Play Level
		//Go to next level
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





	public void hitNumber() {
		stuffVector = new Vector3 (0,25,0);
		stuff.transform.position = stuffVector;

		GameObject tmp = Instantiate(scorePrefab)as GameObject;
		RectTransform tmpRect = tmp.GetComponent<RectTransform>();
		//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<socket>();
		//tmp.transform.SetParent(gameObject.transform);
		tmp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>());
		//tmpRect.transform.localPosition = scorePrefab.transform.localPosition;
		tmpRect.transform.localPosition = stuff.transform.localPosition;
		tmpRect.transform.localScale = scorePrefab.transform.localScale;
		Destroy (tmp, 2.0f);


	}










}
