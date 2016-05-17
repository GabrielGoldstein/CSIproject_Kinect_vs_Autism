using UnityEngine;
using System.Collections;

public class log : MonoBehaviour {

	// Use this for initialization

	public System.IO.StreamWriter file ;
	void Start () {
	
		file = new System.IO.StreamWriter("C:\\Users\\user\\Desktop\\GameLog.txt");

		file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  level Loaded: "+ Application.loadedLevelName); 


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
