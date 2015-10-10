using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	Vector3 pos;
	GrabDropScript grabScript;
	bool isGrabbed;
	Color color;
	Renderer rend;
	Vector3 partOrigin;

	public AudioClip snap;
	public AudioClip boing;

	public AudioSource source;

	bool isSnapped = false;
	
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
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

		if (grabScript.isGrabbed == false)
		{
			if(other.gameObject.tag == gameObject.tag) { //if the body part matches
				rend.material.color = color;
				Vector3 position = gameObject.transform.position;
				other.gameObject.transform.position = position;
				isSnapped = true;
				if(isSnapped){
					other.GetComponent<AudioSource>().PlayOneShot(snap);
					gameObject.SetActive(false);
					keepInPlace(other);
				}
				Debug.Log("Position " + position);
			} else { //reset part back to origin
				other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
				other.GetComponent<AudioSource>().PlayOneShot (boing);
			}
		}
	}

	void OnTriggerExit(Collider other){
		rend.material.color = color; //revert color to original
	}

	void keepInPlace(Collider other)
	{
		//nulls the element in the array with the same name as current object that just snapped
		for(int i = 0; i < grabScript.draggableObjects.Length; i++){
			if(other.gameObject.name == grabScript.draggableObjects[i].name)
				grabScript.draggableObjects[i] = null;
		}
	}
}


























