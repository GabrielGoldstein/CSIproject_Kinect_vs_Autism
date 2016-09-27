using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Zzero : MonoBehaviour {

	//Programmer: Gabriel Goldstien

	//This Script is intended to be attached to "All objects intended to be picked up by the Player"
	//VARIABLES NEED TO BE ASSIGNED IN EDITOR: None

	//SUMMARY:
	//
	//FUNCTIONS:
	//

	//NOTES FOR COMMENTS:
	//Looks like nothing is attached to Zzero. Seems to be that needed variables are passed when needed
	//Boing SFX is passed from BodyPlaceHolder making "AudioClip boing" useless && snap
	//Need to check variables, what is being used and what is not being used. 
	//Not all variables dont need to be public. Default is Internal.
	//	Internal variables dont think can be accessed outside of the class 
	//

	/*THHINGS TO ADD:
		None 
	 */


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Script Variables
	GrabDropScript grabScript;
	public PecCard pcard;
	log logScript;

	Renderer rend; //Attached Renderer
	Color color;

	public List<GameObject> triggeredObjects = new List<GameObject>(); //Collection of triggered objects

	int p;
	public int PlayerIndex = -1; //Kinect Player index, set if the object is grabbed

	public bool isSnapped;
	public bool stillGrabbed = false;
	public bool stillReleased = false;

	float xPos, yPos = 0;
	Vector3 pos;
	public Vector3 origin; //Starting position of attached object
   
	public AudioSource source; //Attached AudioSource
	public AudioClip snap; //Correct SFX
	public AudioClip boing; //Incorrect SFX

    public bool CorrectPlaced
    { get; set; }
    public bool IsReleased {
        get {return PlayerIndex == -1;}
    }
	public bool isGrabbed {
		get {return PlayerIndex != -1;}
	}


	void Start () {
		//Associates to scripts in MainCamera
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		logScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<log>();
		pcard = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();

		//Associates to the Components in attached object
		rend = GetComponent<Renderer>();
		source = GetComponent<AudioSource>();

		origin = transform.position;
		isSnapped = false;
	}


	void Update () {
		//
		if(isGrabbed && (stillGrabbed == false) ) {
			p = PlayerIndex; // p store current player holding the body part

            logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player "+p+" grabs "
                                     + this.name); 

			stillGrabbed = true;
			stillReleased = false;
		}


		//
		if (!isSnapped && IsReleased && transform.position != origin ) {
            Debug.Log("IsReleased");
			if(stillReleased == false){

				if(pcard.pecStarted == false){
					logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player "+p+" releases "
			                         + this.name+" out side");
				}
				else {
					
					logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player "+p+" releases "+ this.name+" out side");
				}
				stillReleased = true;
			}
            transform.position = Vector3.Lerp(transform.position, origin, 5 * Time.deltaTime);
			stillGrabbed = false;
        }
	}


	//Function called from other classes, plays incorrect SFX from the BodyPart
	void playSound () {
		source.PlayOneShot(boing); //Plays incorrect SFX
	}


} 





