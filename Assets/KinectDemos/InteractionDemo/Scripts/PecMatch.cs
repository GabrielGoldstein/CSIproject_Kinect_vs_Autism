using UnityEngine;
using System.Collections;

public class PecMatch : MonoBehaviour {

	GrabDropScript grabScript;
	PecCard pecScript;

	Color color;
	Renderer rend;
	public Vector3 partOrigin;
	public GameObject emptyObject;
//	public AudioClip snap;
	Vector3 position;

	bool arraySet = true;

	MatchingModel matchTransaction;

	static bool isWaiting;

	public static bool IsWaiting {
		get { return isWaiting; }
		set { isWaiting = value; }
	}

	// Use this for initialization
	void Start () {
		matchTransaction = new MatchingModel();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		pecScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PecCard>();
		rend = GetComponent<Renderer>();
		partOrigin = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//determine a snap, send to MatchingModel.snap

	}

	void OnTriggerStay(Collider other) {
		//if a piece is let go
		if (grabScript.isGrabbed == false)
		{
			//PEC matches placeholder TAG
			if(other.gameObject.tag == gameObject.tag) {
				other.GetComponent<Zzero>().isSnapped = true;
				rend.material.color = color;
				position = gameObject.transform.position; //set to placeholder's position
				other.gameObject.transform.position = position;
				if(other.GetComponent<Zzero>().isSnapped){
					//other.GetComponent<AudioSource>().PlayOneShot(snap);

					if (arraySet)
					{
						if (pecScript.match[0] == null)
							pecScript.match[0] = other.gameObject;
						else if (pecScript.match[1] == null)
							pecScript.match[1] = other.gameObject;

						arraySet = false;
					}

					for(int i = 0; i < grabScript.draggableObjects.Length; i++){
						if(other.gameObject.tag == grabScript.draggableObjects[i].tag)
						{
							Debug.Log ("Current GameObject " + grabScript.draggableObjects[i].tag);
							grabScript.draggableObjects[i] = emptyObject;
							//matchTransaction.setSnapped(other.gameObject, i);
						}
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
