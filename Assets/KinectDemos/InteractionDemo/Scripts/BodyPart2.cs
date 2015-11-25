using UnityEngine;
using System.Collections;
using System;
public class BodyPart2 : MonoBehaviour {
	
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

	public InteractionManager player1;
	public InteractionManager player2;
	
	bool isSnapped = false;
	
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
		pec = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();
		player1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0];
		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
	}
	
	// Update is called once per frame
	void Update () {
		
		pos = transform.position;
		pos.z = 0;
		transform.position = pos;
		

		
		
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
		Debug.Log("Player2 Grab: " + player2.GetRightHandEvent());

	

		if (player2.GetRightHandEvent() == InteractionManager.HandEventType.Release)
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
				if(player1.GetRightHandEvent () != InteractionManager.HandEventType.Grip)
				{
					other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
					other.GetComponent<AudioSource>().PlayOneShot (boing);
					Debug.Log("incorrect");
				}
			}

		}
		/*else if (player1.GetRightHandEvent () == InteractionManager.HandEventType.Release)
		{
			other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
			other.GetComponent<AudioSource>().PlayOneShot (boing);
		}*/

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














