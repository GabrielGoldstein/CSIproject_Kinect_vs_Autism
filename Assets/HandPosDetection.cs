using UnityEngine;
using System.Collections;

public class HandPosDetection : MonoBehaviour {


	InteractionManager interaction1;
	InteractionManager interaction2;
	public GameObject obj;

	// Use this for initialization
	void Start () {
		interaction1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if hand is within the center of the screen
		//Right Hand Detection
		if (interaction1.GetRightHandScreenPos().x >= .4375 && interaction1.GetRightHandScreenPos().x <= .5625){
			if (interaction1.GetRightHandScreenPos().y >= .4375 && interaction1.GetRightHandScreenPos().y <= .5625){
				Debug.Log ("Hand is in the middle 1!!");
				obj.SetActive(true);
				gameObject.SetActive (false);
			}
		}
	
		//Left Hand Detection
		if (interaction1.GetLeftHandScreenPos().x >= .4375 && interaction1.GetLeftHandScreenPos().x <= .5625){
			if (interaction1.GetLeftHandScreenPos().y >= .4375 && interaction1.GetLeftHandScreenPos().y <= .5625){
				Debug.Log ("Hand is in the middle 1!!");
				obj.SetActive(true);
				gameObject.SetActive (false);
			}
		}





	}





}
