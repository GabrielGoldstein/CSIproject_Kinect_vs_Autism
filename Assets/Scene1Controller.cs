using UnityEngine;
using System.Collections;

public class Scene1Controller : MonoBehaviour {

	public bool gameOver;

	public GameObject obj1;

	GrabDropScript grabScript;

	// Use this for initialization
	void Start () {
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();

	}
	
	// Update is called once per frame
	void Update () {
		//Detect if Player grabbed headpiece to start gameover
		if(grabScript.draggedObject1 == obj1)
		{
			gameOver = true;
		}
		//Starts Fade when game is over
		if(gameOver == true) 
		{
			//gameObject.GetComponent<Animator>().SetTrigger ("GameOver");
			//Debug.Log("GAMEOVER!!!");
			//StartCoroutine(Wait());
			Application.LoadLevel("Scene 2");
		}
	}

	IEnumerator Wait() {
	yield return new WaitForSeconds(2);
	Application.LoadLevel("Scene 2");
	}
}
