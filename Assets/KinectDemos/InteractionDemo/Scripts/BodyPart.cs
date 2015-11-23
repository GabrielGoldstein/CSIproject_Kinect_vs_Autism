using UnityEngine;
using System.Collections;
using System;
public class BodyPart : MonoBehaviour {

	Vector3 pos;
	GrabDropScript grabScript;
	bool isGrabbed;
	Color color;
	Renderer rend;
	Vector3 partOrigin;
	public GameObject emptyObject;

	public AudioClip snap;
	public AudioClip boing;

	public PecCard pec;
	public AudioSource source;

	public int player1Index;
	public int player2Index;

	public InteractionManager player1;
	public InteractionManager player2;

	bool isSnapped = false;
	
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
		pec = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();

		player1 = (GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0].playerIndex == 0) ? 
				GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0] : 
				GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];

		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
	}
	
	// Update is called once per frame
	void Update () {

		pos = transform.position;
		pos.z = 0;
		transform.position = pos;

		Debug.Log ("Player 1 Status: " + player1.GetRightHandEvent ().ToString());
		Debug.Log ("Player 2 Status: " + player2.GetRightHandEvent ().ToString());
		Debug.Log ("Player 2 Status: " + player2.GetLastRightHandEvent ().ToString());


	}
	
	void OnTriggerEnter(Collider other) {
		
		color = rend.material.color;

		if(this.gameObject.tag == other.gameObject.tag)
		{
			if (grabScript.isGrabbed == true)
			{
				rend.material.color = Color.green;
			}
		} else {
			rend.material.color = Color.red;
		}
	}

	void OnTriggerStay(Collider other) {

		//TODO: Diferentiate between player 1/2
		if ((player1.GetRightHandEvent() == InteractionManager.HandEventType.Release) ||
		    (player2.GetRightHandEvent() == InteractionManager.HandEventType.Release))
			{
	
			if(other.gameObject.tag == gameObject.tag) { //if the body part matches
				other.GetComponent<Zzero>().isSnapped = true;
				pec.snapCounter ++;
				rend.material.color = color;
				Vector3 position = gameObject.transform.position;
				other.gameObject.transform.position = position;

				Debug.Log("game mode: " + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode);
				if(other.GetComponent<Zzero>().isSnapped && GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode){
					gameObject.SetActive(false);
					other.GetComponent<AudioSource>().PlayOneShot(snap);
					Debug.Log("Snapped " + other.gameObject.tag);
					keepInPlace(other);
					//MatchingModel.setSnapped()
				}
				Debug.Log("Position " + position);
				//The line below delays a check for an incorrect piece
				//prevents multiple correct/incorrect piece placement

			} 

			else if (other.GetComponent<Zzero>().isSnapped == false){
				//StartCoroutine(delayReset(other));
				other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
				other.GetComponent<AudioSource>().PlayOneShot (boing);
				Debug.Log("incorrect");
			}
		}
	}

	//Explanation
	//http://forum.unity3d.com/threads/how-can-i-make-a-c-method-wait-a-number-of-seconds.61011/
	IEnumerator delayReset(Collider other){
		yield return new WaitForSeconds(0.5f);
		if (other.GetComponent<Zzero>().isSnapped == false){

//			other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
//			other.GetComponent<AudioSource>().PlayOneShot (boing);
			Debug.Log ("Both");
		}
	}

	void OnTriggerExit(Collider other){
		rend.material.color = color; //revert color to original
	}

	public void keepInPlace(Collider other)
	{
		int i = 0;
		
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		//nulls the element in the array with the same name as current object that just snapped
		Debug.Log ("Sent " + other.gameObject.tag);

		for(i = 0; i < grabScript.draggableObjects.Length; i++) {


			if(other.gameObject.tag == grabScript.draggableObjects[i].tag)
			{
				Debug.Log ("Current GameObject " + grabScript.draggableObjects[i].tag);
				grabScript.draggableObjects[i] = emptyObject;
			}

			//else Debug.Log ("Current GameObject outside " + grabScript.draggableObjects[i].tag);
		}
		


	}
}


























