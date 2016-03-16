using UnityEngine;
using System.Collections;

public class socket : MonoBehaviour {

	public float p1Score,p2Score = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.Log("p1 score: " + p1Score + "\n p2 score: " + p2Score);
	}

	public void updateScore(float score, int pIndex){

		if(pIndex == 0)
			p1Score += score;
		else
			p2Score += score;



	}




}
