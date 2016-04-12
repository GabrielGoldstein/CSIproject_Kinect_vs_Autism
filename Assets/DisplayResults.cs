using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayResults : MonoBehaviour {
	public socket displayResult;
	public GameObject info, snap1, snap2;

	public GameObject p1Snapped;
	public GameObject p2Snapped;
	public GameObject score_p1;
	public GameObject score_p2;

	// Use this for initialization
	void Start () {

		displayResult = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<socket>();
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log ("p1Snapped"+displayResult.p1snapCount.ToString());
		p1Snapped.GetComponent<Text>().text = "Snapped: " + displayResult.p1snapCount.ToString();
		p2Snapped.GetComponent<Text>().text = "Snapped: " + displayResult.p2snapCount.ToString();

		score_p1.GetComponent<Text>().text = "Score: " + displayResult.p1Score.ToString();
		score_p2.GetComponent<Text>().text = "Score:  " + displayResult.p2Score.ToString();
	

//		snap1.GetComponent<Text>().text =  "Snapped: "+displayResult.p1snapCount.ToString();
		//snap2.GetComponent<Text>().text = "Snapped: "+displayResult.p2snapCount.ToString();
	}
}
