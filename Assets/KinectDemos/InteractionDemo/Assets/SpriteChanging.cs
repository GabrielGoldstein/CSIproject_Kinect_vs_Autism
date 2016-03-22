using UnityEngine;
using System.Collections;

public class SpriteChanging : MonoBehaviour {

    public SpriteRenderer sr;
    public Sprite[] textures;
    // Use this for initialization
    void Start () {

        sr = GetComponent<SpriteRenderer>();
        textures = Resources.LoadAll<Sprite>("images");
        Debug.Log(textures.Length);
        int choice = Random.Range(0, textures.Length);

        Debug.Log(choice.ToString());
        sr = GetComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[choice] as Sprite;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
