using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using wvr;
using WaveVR_Log;

public class PlayerInteractions : MonoBehaviour {

    //public WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;

    public DanceBattle db;
    public SongController sc;
    public PlayerMovement pm;
    public GameManager gm;
    public UIManager2 ui;

    public GameObject cam;
    public GameObject[] cameraPos;
    public GameObject plot;

    public AudioMixerSnapshot[] snapshots;

    public FadeManager fm;

    public SongSelect ss;

    public float timeTakenDuringLerp = 1f;

    private float timeStartedLerping;

    private bool isDefault;
    private bool isBar;
    private bool isLaundry;
    private bool isLerping;
    private bool isIntro = true;

    private Vector3 startPos;
    private Vector3 endPos;

    /*WVR_InputId[] buttonIds = new WVR_InputId[]
    {
        WVR_InputId.WVR_InputId_Alias1_Menu,
        WVR_InputId.WVR_InputId_Alias1_Grip,
        WVR_InputId.WVR_InputId_Alias1_DPad_Left,
        WVR_InputId.WVR_InputId_Alias1_DPad_Up,
        WVR_InputId.WVR_InputId_Alias1_DPad_Right,
        WVR_InputId.WVR_InputId_Alias1_DPad_Down,
        WVR_InputId.WVR_InputId_Alias1_Volume_Up,
        WVR_InputId.WVR_InputId_Alias1_Volume_Down,
        WVR_InputId.WVR_InputId_Alias1_Touchpad,
        WVR_InputId.WVR_InputId_Alias1_Trigger,
        WVR_InputId.WVR_InputId_Alias1_Digital_Trigger,
        WVR_InputId.WVR_InputId_Alias1_System
    };

    WVR_InputId[] axisIds = new WVR_InputId[] {
        WVR_InputId.WVR_InputId_Alias1_Touchpad,
        WVR_InputId.WVR_InputId_Alias1_Trigger
    };*/

    // Use this for initialization
    void Start () {
        pm = GetComponent<PlayerMovement>();
        cam.transform.position = cameraPos[1].transform.position;

        snapshots[4].TransitionTo(0.01f);

        StartCoroutine(ChangeSnapshotIntro(5, 17.0f, 2.5f));
        StartCoroutine(ChangeSnapshotIntro(0, 38.675f, 0.01f));
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter(Collider other)
    {
        isIntro = false;
        int i;
        if (other.tag == "DoorClub")
        {
            if (other.transform.GetChild(0).gameObject.activeSelf && other.transform.GetChild(1).gameObject.activeSelf)
            {
                i = 1;
                StartCoroutine(ChangeCamPos(i, other));
            }
            else
            {
                i = 0;

                StartCoroutine(ChangeCamPos(i, other));
            }

        }
        if (other.tag == "DoorLaundry")
        {
            if (other.transform.GetChild(0).gameObject.activeSelf)
            {
                i = 2;
                StartCoroutine(ChangeCamPos(i, other));
            }
            else
            {
                i = 0;
                StartCoroutine(ChangeCamPos(i, other));
            }

        }
        if (other.tag == "danceFloor")
        {
            ui.startDance.SetActive(true);
        }
        if (other.tag == "bar")
        {
            ui.drinkInstructions.SetActive(true);
        }
        if (other.tag == "machine")
        {
            ui.laundryInstructions.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "danceFloor")
        {
            ui.startDance.SetActive(false);
        }
        if (other.tag == "bar")
        {
            ui.drinkInstructions.SetActive(false);
        }
        if (other.tag == "machine")
        {
            ui.laundryInstructions.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "danceFloor")
        {

            /*if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("dancestart");
                db.enabled = true;
                pm.enabled = false;
            }
            if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Digital_Trigger))
            {
                Debug.Log("dancestart");
                db.enabled = true;
                db.StartDance();
                pm.enabled = false;
            }*/
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (ui.songCanvas.activeSelf)
                {
                    snapshots[5].TransitionTo(0.2f);
                    plot.SetActive(true);
                    sc.StartPlot();
                    if (db == null)
                        db = FindObjectOfType<DanceBattle>();
                    db.enabled = true;
                    db.StartDance();
                    enabled = false;
                }
                else
                {

                    pm.enabled = false;
                    ui.songCanvas.SetActive(true);
                    ss.enabled = true;
                }
                Debug.Log("dancestart");
                GameObject.Find("SongCanvas").SetActive(true);
                
            }
        }

        if (other.tag == "bar")
        {

            /*if (Input.GetKeyDown(KeyCode.E))
            {
                ui.barCanvas.enabled = true;
            }
            if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Digital_Trigger))
            {
                ui.barCanvas.enabled = true;
            }*/
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                ui.barCanvas.enabled = true;
            }
        }

        if (other.tag == "machine")
        {

            /*if (Input.GetKeyDown(KeyCode.E))
            {
                gm.Laundry();
            }
            if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Digital_Trigger))
            {
                gm.Laundry();
            }*/
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                gm.Laundry();
            }
        }
    }

    IEnumerator ChangeCamPos(int i, Collider other)
    {
        snapshots[i].TransitionTo(1.0f);
        fm.FadeOut();
        yield return new WaitForSeconds(0.5f);
        if (i == 0)
        {
            if (!other.transform.GetChild(0).gameObject.activeSelf)
            {
                other.transform.GetChild(0).gameObject.SetActive(true);
                try
                {
                    other.transform.GetChild(1).gameObject.SetActive(true);
                }
                catch
                {

                }
            }
        }
        else if(i == 1)
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
        }

        else if (i == 2)
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
        cam.transform.position = cameraPos[i].transform.position;
        fm.FadeIn();
    }

    IEnumerator ChangeSnapshotIntro(int index, float timeTo, float transTime)
    {
        yield return new WaitForSeconds(timeTo);
        if (isIntro)
        {
            snapshots[index].TransitionTo(transTime);
        }

    }

}
