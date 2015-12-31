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

	public GameObject[] placeHolders;

	public AudioSource source;
	public AudioClip boing;
	public AudioClip clapping;

	public GameObject smokeEffect;
	public GameObject avatar;
	public GameObject bodyParts;
	public GameObject pecCards;

	bool temp = true;

	// Use this for initialization
	void Start () {
		//pec = new GameObject[arraySize];
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
			
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

				if (temp)
				{
					source.PlayOneShot(clapping);
					smokeEffect.SetActive(true);
					pecCards.SetActive(false);
					//Wait 3 Secs to activate model and deactivate Parts,PEC Card;
					StartCoroutine(wait());

//					avatar.SetActive (true);
//					bodyParts.SetActive(false);
//					pecCards.SetActive(false);
					temp = false;
				}
			}
			else {

				Debug.Log ("RESTART");

				source.PlayOneShot (boing);

				//Sets PEC cards back to there original positions
				match[0].gameObject.transform.position = match[0].gameObject.GetComponent<Zzero>().origin;
				match[1].gameObject.transform.position = match[1].gameObject.GetComponent<Zzero>().origin;

				//Sets the Array set fucntion to true so function can repeate multiple times.
				if (placeHolders[0].gameObject.tag == "P1")
					placeHolders[0].gameObject.GetComponent<PecMatch1>().arraySet = true;
				else if (placeHolders[0].gameObject.tag == "P2")
					placeHolders[0].gameObject.GetComponent<PecMatch2>().arraySet = true;

				if (placeHolders[1].gameObject.tag == "P1")
					placeHolders[1].gameObject.GetComponent<PecMatch1>().arraySet = true;
				else if (placeHolders[1].gameObject.tag == "P2")
					placeHolders[1].gameObject.GetComponent<PecMatch2>().arraySet = true;


				for(int i = 0; i < grabScript.draggableObjects.Length; i++){
					if(grabScript.draggableObjects[i].tag == "Junk")
					{
						if (match[0] != null) {
							grabScript.draggableObjects[i] = match[0];
							match[0] = null;
						}
						else if (match[1] != null){
							grabScript.draggableObjects[i] = match[1];
							match[1] = null;
						}
					}
				}
				




			}
		}

	}

	IEnumerator wait() {
		yield return new WaitForSeconds(4);
		avatar.SetActive (true);
		bodyParts.SetActive(false);

	}


}
