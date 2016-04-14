using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent (typeof(AudioSource))]

public class PlayVideo : MonoBehaviour {

    private AudioSource audio;
    public GameObject video;
    public GameObject button;
    public MovieTexture[] movies;

    

    int n;

    // Use this for initialization
    void Start () {
		movies = Resources.LoadAll<MovieTexture>("Videos");

    }
	
	// Update is called once per frame
	void Update () {

        //Hitting Space will stop the video
        if (Input.GetKeyDown(KeyCode.Space))
        {
			movies[n].Stop();

            Disable_Video();
            Enable_Button();
        }
        
        //If the video ends, then hide the video and show the play button
        if (!movies[n].isPlaying)
        {
            Disable_Video();
            Enable_Button();
        }
    }

	//Button Function to Play Videos
	public void playVideo()
	{
		Button_Click();
		Enable_Video();
		Disable_Button();
	
	}

    //To hide the video
    public void Disable_Video()
    {
        video.SetActive(false);
    }

    //To reveal the video
    public void Enable_Video()
    {
        video.SetActive(true);
    }

    //To hide the UI button
    public void Disable_Button()
    {
        button.SetActive(false);
    }

    //To reveal the UI button
    public void Enable_Button()
    {
        button.SetActive(true);
    }

    //When clicked on the UI button do this:
    public void Button_Click()
    {
        //Assign n to a random number from 0-2
        n = Random.Range(0, movies.Length);

        //Display that number on the console (so we know what video gets played)
        print(n);
        
        GetComponent<RawImage>().texture = movies[n] as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movies[n].audioClip;

        movies[n].Play();
        audio.Play();
    }


}
