using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System;
public class BodyPart1 : MonoBehaviour {

	Vector3 pos;
	GrabDropScript grabScript;
	log logScript;
	bool isGrabbed;
	Color color;
	Renderer rend;
	Vector3 partOrigin;
	public GameObject emptyObject;

	public GameObject scorePrefab;
	public Vector3 scoreVector;

	public AudioClip snap;
	public AudioClip boing;

	public PecCard pec;
	public AudioSource source;
	public socket scoreKeep;
	bool isSnapped = false;





	// Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();

		logScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<log>();
		source = GetComponent<AudioSource>();
        pec = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();
		scoreKeep =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<socket>();
		scoreVector = new Vector3 (0,25,0);




	
	}
	// Update is called once per frame
	void Update () {


		pos = transform.position;
		pos.z = 0;
		transform.position = pos;




	}

    void OnTriggerEnter(Collider other)
    {
        color = rend.material.color;
        var obj = other.GetComponent<Zzero>();


        obj.triggeredObjects.Add(this.gameObject);
        if (this.gameObject.tag == other.gameObject.tag)
        {
            obj.CorrectPlaced = true;
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
        //Debug.Log(string.Format("is grabbing {0}, player {1}", grabScript.isGrabbed1,obj.PlayerIndex));

		int pvalue = obj.PlayerIndex;
		//calcScore(obj.PlayerIndex);
        if ((!grabScript.isGrabbed1 && obj.PlayerIndex == 0) || (!grabScript.isGrabbed2 && obj.PlayerIndex == 1))
        {
				

            if (other.gameObject.tag == gameObject.tag)
            {
                //if the body part matches

				calcScore(gameObject.transform.position, other.gameObject.transform.position, pvalue);
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
					other.GetComponent<BoxCollider>().enabled=false;
                    keepInPlace(other);
                    //MatchingModel.setSnapped()
                }
                Debug.Log("Position " + position);
                //The line below delays a check for an incorrect piece
                //prevents multiple correct/incorrect piece placement
            }
            else
                if (!obj.CorrectPlaced)
                {
				logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player "+obj.PlayerIndex+" releases "+obj.tag
				                         +", incorrect, into slot: "+this.tag);
                    other.GetComponent<Zzero>().PlayerIndex = -1;
                    other.GetComponent<AudioSource>().PlayOneShot(boing);


                    Debug.Log("incorrect");
                }
        }
        else
            if ((grabScript.isGrabbed1 && obj.PlayerIndex == 0) || (grabScript.isGrabbed2 && obj.PlayerIndex == 1))
            {
                if (obj.CorrectPlaced && other.gameObject.tag != gameObject.tag)
                {
                    rend.material.color = color;
                }
                else if (!obj.CorrectPlaced)
                {
                    if (other.gameObject.tag == gameObject.tag)
                    {
                        rend.material.color = Color.green;
                    }
                    else
                    {
                        rend.material.color = Color.red;
                    }

                }
            }
		// player1 = grip, player2 = releaes
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

	public void calcScore( Vector3 holderPos, Vector3 partPos, int player){
		
		float score = Math.Abs( holderPos.x - partPos.x) + Math.Abs(holderPos.y - partPos.y);

		if(score <= 0.5)
			score = 100;
		else if(score <= 1.0)
			score = 75;
		else if( score <= 1.5)
			score = 50;
		else 
			score = 25;

		//Debug.Log("DEBUG score: " + score);

        logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player "+player+" releases "+this.tag
                                 + ", correct, score: "+score);
		initScorePrefab(score,player);
		scoreKeep.updateScore(score,player);
	}


	public void initScorePrefab(float score, float player){


		GameObject tmp = Instantiate(scorePrefab)as GameObject;
		//RectTransform tmpRect = tmp.GetComponent<RectTransform>();

		tmp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>());
		tmp.SetActive(true);
		//tmpRect.transform.position = (gameObject.transform.position + scoreVector);

		//if(player == 0)
		//	tmp.GetComponent<Text>().color = Color.green;
		//else
		//	tmp.GetComponent<Text>().color = Color.blue;


		//tmp.GetComponent<Text>().text = score.ToString();
		Debug.Log ("GameObject.transform.position: " + gameObject.transform.position);
		Debug.Log ("ScoreVector: " + scoreVector);
		Debug.Log ("VECTOR ADDITION: " + (gameObject.transform.position + scoreVector));

		if(gameObject.tag == "leftArm" || gameObject.tag == "rightArm"){
		tmp.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x * 40, 
		                                               gameObject.transform.localPosition.y + 50, 
		                                               gameObject.transform.localPosition.z);
		}
		else if (gameObject.tag == "leftLeg"){
		 tmp.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x - 40, 
			                                               gameObject.transform.localPosition.y - 50 , 
			                                               gameObject.transform.localPosition.z);

		

		}
		else if (gameObject.tag == "rightLeg"){
			tmp.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x + 40, 
			                                               gameObject.transform.localPosition.y -50 , 
			                                               gameObject.transform.localPosition.z);
			
			
			
		}
		else{
			tmp.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x , 
			                                               gameObject.transform.localPosition.y, 
			                                               gameObject.transform.localPosition.z - 10);

		}

		//tmpRect.transform.localScale = scorePrefab.transform.localScale;




	}




	void OnTriggerExit(Collider other)
    {        
        var obj = other.GetComponent<Zzero>();
        if (this.gameObject.tag == other.gameObject.tag)
        {
            obj.CorrectPlaced = false;
        }
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


























