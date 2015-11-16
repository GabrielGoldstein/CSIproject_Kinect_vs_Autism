using UnityEngine;
using System.Collections;

public class PecCard : MonoBehaviour {

	GrabDropScript grabScript;

	public int snapCounter = 0;
	public int numPieces = 0;
	public int arraySize = 0;
	public bool bodyMatchMode = true;

	public GameObject[] match;
	public GameObject[] pec ;

	// Use this for initialization
	void Start () {
		//pec = new GameObject[arraySize];
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
			
	}
	
	// Update is called once per frame
	void Update () {
	

		if(snapCounter == numPieces){
			Debug.Log("BEGIN PEC MODE");
			bodyMatchMode = false;
			//p2Holder.SetActive(true);
			for (int i = 0; i < pec.Length; i++) {
				//pec.GetLength(arraySize)
				pec[i].SetActive(true);

			}

		}


		if (match[0] != null && match[1] != null){ 
			if (match[0].name == match[1].name) {
				//START HEAD PART 
				Debug.Log ("CONTINUE");
			}
			else {

				Debug.Log ("ELSESLE");
				Debug.Log (match[0].gameObject.GetComponent<Zzero>().origin);

				//Sets PEC cards back to there original positions
				match[0].gameObject.transform.position = match[0].gameObject.GetComponent<Zzero>().origin;
				match[1].gameObject.transform.position = match[1].gameObject.GetComponent<Zzero>().origin;

				//Restores PEC cards grab
				grabScript.draggableObjects[0] = match[0];
				grabScript.draggableObjects[1] = match[1];

				match[0] = null;
				match[1] = null;




			}
		}

	}

}
