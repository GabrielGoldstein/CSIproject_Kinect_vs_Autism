using UnityEngine;
using System.Collections;

public class PecCard : MonoBehaviour {

	GrabDropScript grabScript;
	InteractionManager[] interactionManager;

	public int snapCounter;
	public int numPieces;
	public int arraySize;
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
	public GameObject PECCards;

	public GameObject resultPanel;
	public log scriptLog;
	public Timer result;
	public bool pecStarted = false;
	bool temp = true;

	public bool temp2 = false; //Makes sure the if statement only happens once

	int attempt = 1;
	// Use this for initialization
	void Start () {
		//pec = new GameObject[arraySize];
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		//interactionManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractionManager>();
		interactionManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>();

		source = GetComponent<AudioSource>();

		result = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Timer>();

		scriptLog = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<log>();

			
	}
	
	// Update is called once per frame
	void Update () {
	
//Checks if all body parts are snapped in
		if((snapCounter == numPieces)&&(temp2 == false)){
			Debug.Log("BEGIN PEC MODE");
			scriptLog.file.WriteLine("\n"+System.DateTime.Now.ToString("hh:mm:ss")+"  Pec Mode starts");
			bodyMatchMode = false;
			PECCards.SetActive (true);
			temp2 = true;
			//p2Holder.SetActive(true);
			pecStarted = true;
			/*for (int i = 0; i < pec.Length; i++) {
				//pec.GetLength(arraySize)
				pec[i].SetActive(true);

			}*/

		}


		if (match[0] != null && match[1] != null){ 
			if (match[0].name == match[1].name) {
				//START HEAD PART 
				Debug.Log ("CONTINUE");

				if (temp)
				{
					source.PlayOneShot(clapping);
					smokeEffect.SetActive(true);
					temp = false;

					//Wait 3 Secs to activate model and deactivate Parts,PEC Card;
					StartCoroutine(wait());



				}
			}
			else {

				Debug.Log ("RESTART");

				attempt++;
				scriptLog.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  Incorret PEC restarts, attempts: "+attempt); 
				source.PlayOneShot (boing);

				//Sets PEC cards back to there original positions
				match[0].gameObject.transform.position = match[0].gameObject.GetComponent<Zzero>().origin;
				match[1].gameObject.transform.position = match[1].gameObject.GetComponent<Zzero>().origin;

				//Sets the Array set fucntion to true so function can repeate multiple times.
				if (placeHolders[0].gameObject.tag == "P1"){
					placeHolders[0].gameObject.GetComponent<PecMatch1>().arraySet = true;
					placeHolders[0].gameObject.GetComponent<PecMatch1>().occupied = false;
				}
				else if (placeHolders[0].gameObject.tag == "P2"){
					placeHolders[0].gameObject.GetComponent<PecMatch2>().arraySet = true;
					placeHolders[0].gameObject.GetComponent<PecMatch2>().occupied = false;
				}
				if (placeHolders[1].gameObject.tag == "P1"){
					placeHolders[1].gameObject.GetComponent<PecMatch1>().arraySet = true;
					placeHolders[1].gameObject.GetComponent<PecMatch1>().occupied = false;
				}
				else if (placeHolders[1].gameObject.tag == "P2"){
					placeHolders[1].gameObject.GetComponent<PecMatch2>().arraySet = true;
					placeHolders[1].gameObject.GetComponent<PecMatch2>().occupied = false;
				}


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
		result.finish();
		yield return new WaitForSeconds(3);
		avatar.SetActive (true);
		PECCards.SetActive(false);
		bodyParts.SetActive(false);
		yield return new WaitForSeconds(7);
		//Disable Avitar
		avatar.SetActive (false);
		//Enable Results page
		resultPanel.SetActive(true);
		//Disable Hands 
		interactionManager[0].useHandCursor = false;
		interactionManager[1].useHandCursor = false;


	}


}
