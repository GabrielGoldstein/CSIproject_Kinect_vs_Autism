using UnityEngine;
using System.Collections;

public class Erasera : MonoBehaviour {
	public int playernum;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		//Destroy (other.gameObject);
		if(playernum==other.gameObject.GetComponent<CoverBlocks>().playerDestroyed)
        	other.gameObject.GetComponent<CoverBlocks>().shrinking = true;
    }


}
