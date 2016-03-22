using UnityEngine;
using System.Collections;

public class PecMatch2 : MonoBehaviour {
	
	GrabDropScript grabScript;
	PecCard pecScript;
	
	Color color;
	Renderer rend;
	public Vector3 partOrigin;
	public GameObject emptyObject;
	Vector3 position;
	
	public AudioClip snap;
	
	public InteractionManager player1;
	public InteractionManager player2;
	
	
	
	public bool arraySet = true;
	
	MatchingModel matchTransaction;
	
	
	
	
	// Use this for initialization
	void Start () {
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		pecScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PecCard>();
		rend = GetComponent<Renderer>();
		partOrigin = gameObject.transform.position;
		color = rend.material.color;
		
		player1 = (GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0].playerIndex == 0) ? 
			GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0] : 
				GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		
		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		
	}
	
	// Update is called once per frame
	void Update () {
		//determine a snap, send to MatchingModel.snap
		
		
		
	}
	
	void OnTriggerExit(Collider other) {
		rend.material.color = color;
	}
	
	void OnTriggerStay(Collider other) {
		//if a piece is let go

		if (other.gameObject.tag == gameObject.tag)
		{
			rend.material.color = Color.green;
		}
		else
			rend.material.color = Color.red;


		if (player2.GetRightHandEvent() == InteractionManager.HandEventType.Release)
		{
			//PEC matches placeholder TAG
			if(other.gameObject.tag == gameObject.tag) {
				other.GetComponent<Zzero>().isSnapped = true;
				position = gameObject.transform.position; //set to placeholder's position
				other.gameObject.transform.position = position;
				
				
				if (arraySet)
				{
					other.GetComponent<AudioSource>().PlayOneShot(snap);
					if (pecScript.match[0] == null)
						pecScript.match[0] = other.gameObject;
					else if (pecScript.match[1] == null)
						pecScript.match[1] = other.gameObject;
					
					arraySet = false;
				}
				
				for(int i = 0; i < grabScript.draggableObjects.Length; i++){
					if(other.gameObject.tag == grabScript.draggableObjects[i].tag && other.gameObject.name == grabScript.draggableObjects[i].name)
					{
						grabScript.draggableObjects[i] = emptyObject;
						//matchTransaction.setSnapped(other.gameObject, i);
					}
				}
				
				
				Debug.Log("Position " + position);
				//The line below delays a check for an incorrect piece
				//prevents multiple correct/incorrect piece placement
				
			}
			//			else if (other.GetComponent<Zzero>().isSnapped == false){
			//				//StartCoroutine(delayReset(other));
			//				other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
			//				//other.GetComponent<AudioSource>().PlayOneShot (boing);
			//				Debug.Log("incorrect");
			//			}
		}
		
	}
	
}
