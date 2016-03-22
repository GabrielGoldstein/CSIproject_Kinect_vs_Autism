using UnityEngine;
using System.Collections;

public class CoverBlocks : MonoBehaviour {
    public bool shrinking = false;
    // Use this for initialization
    void Start () {

        
        GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, .5f);

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
