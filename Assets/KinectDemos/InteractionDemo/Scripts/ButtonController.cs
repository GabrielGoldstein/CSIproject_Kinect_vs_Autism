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


	public void toLevel1(){
		Application.LoadLevel ("Scene 1");
	}
	public void toLevel2() {
		Application.LoadLevel ("Scene 2");
	}
	public void toLevel3(){
		Application.LoadLevel ("Scene 3");
	}
	public void toLevel4(){
		Application.LoadLevel ("Scene 4");
	}


	public void miniGameButton(){
		Debug.Log ("Last Loaded Level: " + Application.loadedLevel);

		PlayerPrefs.SetInt("Level", Application.loadedLevel );
		Application.LoadLevel ("Eraser Mini Game");
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
