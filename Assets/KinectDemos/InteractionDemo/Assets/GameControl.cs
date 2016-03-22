using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public GameObject overLay;
    public GameObject picture;
    public GameObject eraser;
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Overlay"))
            overLay = GameObject.Find("Overlay");
        else Debug.Log("no OverLay");
        if (GameObject.Find("Cube"))
            picture = GameObject.Find("Cube");
        else Debug.Log("no cube");

        if (GameObject.Find("Eraser"))
            picture = GameObject.Find("Eraser");
        else Debug.Log("no eraser");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Instantiate(overLay, new Vector3(overLay.transform.position.x + j*2, overLay.transform.position.y + i*2, overLay.transform.position.z), Quaternion.identity);
            }
        }
        Destroy(overLay);
    }


// Update is called once per frame
void Update()
{
    if (!GameObject.Find("Overlay(Clone)"))
        Debug.Log("asdf");




}
}