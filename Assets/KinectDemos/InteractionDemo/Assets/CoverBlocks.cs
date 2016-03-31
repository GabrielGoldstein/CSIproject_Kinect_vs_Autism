using UnityEngine;
using System.Collections;

public class CoverBlocks : MonoBehaviour {
    public bool shrinking = false;
    // Use this for initialization
	public int playerDestroyed;
    void Start () {
		playerDestroyed=(int)(float)(Random.value*10)/5;
        
        if(playerDestroyed==0)
			GetComponent<Renderer>().material.color = new Color(1, 1, 1, .5f);
		else
			GetComponent<Renderer>().material.color = new Color(1, .2f,1, .5f);


    }

    // Update is called once per frame
    void Update () {
        if (shrinking)
        {
            transform.localScale -= new Vector3(0.1F, 0.1f, 0);
            if (transform.localScale.x < .1)
                Destroy(gameObject);
        }

    }
}
