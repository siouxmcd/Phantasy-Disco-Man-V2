using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager2 : MonoBehaviour {

    public GameObject danceInstructions;
    public Canvas barCanvas;
    public Canvas laundryCanvas;
    public Image cactus;
    public Image beer;
    public Image win;
    public GameObject laundry;
    public GameObject laundryInstructions;
    public GameObject drinkInstructions;
    public GameObject startDance;
    public Text winText;
    public Text playerDanceScore;
    public Text opponentDanceScore;
    public Text playerMoveScore;
    public Text opponentMoveScore;
    public Text playerComments;
    public Text opponentComments;

    public Image label;
    public Text curSong;
    public Text curArtist;
    public GameObject songCanvas;

    AudioSource songAud;

    public AudioClip hooked, coming, signed, southern, abc, man, boogie, like;

    // Use this for initialization
    void Start () {
        songAud = GameObject.Find("Bar Interior").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSong(string song)
    {
        if(song == "hooked")
        {
            songAud.clip = hooked;
        }
        if (song == "coming")
        {
            songAud.clip = coming;
        }
        if (song == "signed")
        {
            songAud.clip = signed;
        }
        if (song == "southern")
        {
            songAud.clip = southern;
        }
        if (song == "abc")
        {
            songAud.clip = abc;
        }
        if (song == "man")
        {
            songAud.clip = man;
        }
        if (song == "boogie")
        {
            songAud.clip = boogie;
        }
        if (song == "like")
        {
            songAud.clip = like;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Skills");
    }
}
