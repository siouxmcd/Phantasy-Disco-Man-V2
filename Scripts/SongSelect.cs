using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SongSelect : MonoBehaviour {

    public AudioClip[] songs;
    public List<Songs> songList;

    public UIManager2 ui;

    AudioSource aud;

    private int i = 0;

    // Use this for initialization
    void Start () {
        aud = GetComponent<AudioSource>();

        songList = new List<Songs>();
        foreach (AudioClip song in songs){
            Songs songInfo = new Songs();

            byte[] b = new byte[128];
            string sTitle;
            string sSinger;

            string path = Application.dataPath + "/StreamingAssets/" + song.name +".mp3";

            FileStream fs = new FileStream(path, FileMode.Open);
            fs.Seek(-128, SeekOrigin.End);
            fs.Read(b, 0, 128);
            bool isSet = false;
            string sFlag = System.Text.Encoding.Default.GetString(b, 0, 3);
            Debug.Log(sFlag);
            if (sFlag.CompareTo("TAG") == 0)
            {
                Debug.Log("Tag   is   setted! ");
                isSet = true;
            }

            if (isSet)
            {
                //get   title   of   song; 
                sTitle = System.Text.Encoding.Default.GetString(b, 3, 30);
                Debug.Log("Title: " + sTitle);
                //get   singer; 
                sSinger = System.Text.Encoding.Default.GetString(b, 33, 30);
                Debug.Log("Singer: " + sSinger);

                songInfo.songTitle = sTitle.TrimEnd();
                songInfo.artistName = sSinger.TrimEnd();
                songInfo.clip = song;
                songInfo.labelColor = Random.ColorHSV();

                songList.Add(songInfo);
            }
        }

        ui.curSong.text = songList[i].songTitle;
        ui.curArtist.text = songList[i].artistName;
        aud.clip = songList[i].clip;
        ui.label.color = songList[i].labelColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft))
        {
            if(i == 0)
            {
                i = songList.Capacity - 1;
            }
            else
            {
                i--;
            }
            Debug.Log(i);

            ui.curSong.text = songList[i].songTitle;
            ui.curArtist.text = songList[i].artistName;
            aud.clip = songList[i].clip;
            ui.label.color = songList[i].labelColor;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            if(i == songList.Capacity - 1)
            {
                i = 0;
            }
            else
            {
                i++;
            }

            Debug.Log(i);

            ui.curSong.text = songList[i].songTitle;
            ui.curArtist.text = songList[i].artistName;
            aud.clip = songList[i].clip;
            ui.label.color = songList[i].labelColor;
        }
    }
}

public class Songs
{
    public AudioClip clip;
    public string songTitle;
    public string artistName;
    public Color labelColor;
}
