﻿using UnityEngine;
using System.Collections;
using System;
public class BodyPart1 : MonoBehaviour {

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
	bool isSnapped = false;
	
	// Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
        source = GetComponent<AudioSource>();
        pec = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();
    }
	
	// Update is called once per frame
	void Update () {

		pos = transform.position;
		pos.z = 0;
		transform.position = pos;




	}
	
	void OnTriggerEnter(Collider other) {
		
		color = rend.material.color;
        
        var obj = other.GetComponent<Zzero>();
        obj.triggeredObjects.Add(this.gameObject);
        if (this.gameObject.tag == other.gameObject.tag)
        {
            rend.material.color = Color.green;
        }
        else
        {
            rend.material.color = Color.red;
        }
	}

	void OnTriggerStay(Collider other) {

		//TODO: Diferentiate between player 1/2
        var obj = other.GetComponent<Zzero>();
        Debug.Log(string.Format("is grabbing {0}, player {1}", grabScript.isGrabbed1,obj.PlayerIndex));
		if ((!grabScript.isGrabbed1 && obj.PlayerIndex == 0) || (!grabScript.isGrabbed2 && obj.PlayerIndex==1))
	    {            
            if (other.gameObject.tag == gameObject.tag)
            {
                //if the body part matches
                
                pec.snapCounter++;
                rend.material.color = color;
                Vector3 position = gameObject.transform.position;
                other.gameObject.transform.position = position;

                Debug.Log("game mode: " + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode);
                if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode)
                {
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
            else 
            {
                other.GetComponent<Zzero>().PlayerIndex = -1;
                other.GetComponent<AudioSource>().PlayOneShot(boing);
                Debug.Log("incorrect");
            }
		}
		// player1 = grip, player2 = releae
		/*
		else if (player2.GetRightHandEvent () == InteractionManager.HandEventType.Release)
		{
			if (other.gameObject.tag != tag)
			{
				other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
				other.GetComponent<AudioSource>().PlayOneShot (boing);
			}
		}
		*/
	}
	

	void OnTriggerExit(Collider other)
    {        
        var obj = other.GetComponent<Zzero>();
        obj.triggeredObjects.Remove(this.gameObject);
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


























