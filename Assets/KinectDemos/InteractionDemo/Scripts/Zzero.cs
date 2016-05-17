using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Zzero : MonoBehaviour {

	float xPos, yPos = 0;
	Color color;
	Renderer rend;
	Vector3 pos;
	GrabDropScript grabScript;
	log logScript;
    public bool isGrabbed
    {
        get { return PlayerIndex != -1; }
    }
	public bool isSnapped;
	public bool stillGrabbed = false;

	public bool stillReleased = false;
	public Vector3 origin;

	public AudioClip snap;
	public AudioClip boing;
	public AudioSource source;
    public bool CorrectPlaced
    { get; set; }
    /// <summary>
    /// Kinect playe index, set if the object is grabbed
    /// </summary>
    public int PlayerIndex = -1;
	int p;
    public bool IsReleased
    {
        get
        { return PlayerIndex == -1; }
    }
    /// <summary>
    /// collection of triggered objects
    /// </summary>
    public List<GameObject> triggeredObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		logScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<log>();
		source = GetComponent<AudioSource>();
		origin = transform.position;
		isSnapped = false;
	}

	// Update is called once per frame
	void Update () 
    {

		if(isGrabbed && (stillGrabbed == false) )
		{
			// p store current player holding the body part
			p = PlayerIndex;

			logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+" player "+p+" grabs "
			                         + this.name); 


			stillGrabbed = true;
			stillReleased = false;
		}


        if (!isSnapped && IsReleased && transform.position != origin )
        {
            Debug.Log("IsReleased");

			if(stillReleased == false){
			logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+" player "+p+" releases "
			                         + this.name+" out side");
				stillReleased = true;

			}
            transform.position = Vector3.Lerp(transform.position, origin, 5 * Time.deltaTime);
			stillGrabbed = false;


        }




		//if(!isGrabbed)
		//	transform.position = new Vector3(-4, 1, 0);

        //pos = transform.position;
        //pos.z = 0;
        //transform.position = pos;
	}

    void OnTriggerEnter(Collider other)
    {

    }

	void playSound ()
	{
		source.PlayOneShot(boing);
	}
}
