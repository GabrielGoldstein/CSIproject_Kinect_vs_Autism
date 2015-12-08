﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Zzero : MonoBehaviour {

	float xPos, yPos = 0;
	Color color;
	Renderer rend;
	Vector3 pos;
	GrabDropScript grabScript;

    public bool isGrabbed
    {
        get { return PlayerIndex != -1; }
    }
	public bool isSnapped;

	public Vector3 origin;

	public AudioClip snap;
	public AudioClip boing;
	public AudioSource source;
    /// <summary>
    /// Kinect playe index, set if the object is grabbed
    /// </summary>
    public int PlayerIndex = -1;
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
		source = GetComponent<AudioSource>();
		origin = transform.position;
		isSnapped = false;
	}

	// Update is called once per frame
	void Update () 
    {
        if (!isSnapped && IsReleased && transform.position != origin)
        {
            Debug.Log("IsReleased");
            transform.position = Vector3.Lerp(transform.position, origin, 5 * Time.deltaTime);
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
