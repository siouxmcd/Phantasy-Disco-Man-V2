using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DanceBattle : MonoBehaviour {

    //public WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;

    Transform Player;
    Transform Opponent;
    Transform oppTarget;
    public Light[] danceLights = new Light[3];
    Animator animator;
    Animator dkAnim;
    AudioSource tracker;
    PlayerMovement pm;
    //OpponentSkills os;
    GameManager gm;
    public UIManager2 ui;
    public FFTWindow fftWindow;

    Text log;

    public int frequency = 0;

    public float battleScore;
    public float opBattleScore;

    public float[] samples = new float[256];
    public float spawnThreshold = 0.5F;

    AudioSource audio;
    SongSelect sel;

    public AudioMixerSnapshot barSnapshot;

    public AudioClip[] sections;

    private bool isTurn;

    private int lightsCounter;
    private int prevCount;
    private int commentsCounter;

    private int pointLcount;
    private int pointRcount;
    private int shakeLcount;
    private int shakeRcount;
    private int victoryCount;
    private int splitsCount;

    private int secCounter = 0;

    private float secondsCount;

    private string previousTouchState;
    private string[] danceSet = new string[4];

    GameObject plot;
    public static bool isBeat;
    private bool isPreviousBeat;

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

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        sel = GetComponent<SongSelect>();
        prevCount = danceLights.Length;
        lightsCounter = 0;
        danceSet[0] = "isRevR";
        danceSet[1] = "isRevL";
        danceSet[2] = "isPointR";
        danceSet[3] = "isPointL";
        Player = GameObject.Find("Player").transform;
        Opponent = GameObject.Find("Dance King").transform;
        oppTarget = GameObject.Find("opponentPlace").transform;
        danceLights[0] = GameObject.Find("purpLight").GetComponent<Light>();
        danceLights[1] = GameObject.Find("greenLight").GetComponent<Light>();
        danceLights[2] = GameObject.Find("blueLight").GetComponent<Light>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
        dkAnim = GameObject.Find("Dance King").GetComponent<Animator>();
        tracker = GameObject.Find("Player").GetComponent<AudioSource>();
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        ui = GameObject.Find("Manager").GetComponent<UIManager2>();
        log = gm.log;
        plot = gm.plot;
    }
    // Use this for initialization
    void Start () {
        sel.enabled = false;
    }

    public void StartDance()
    {
        if (ui.startDance.activeSelf)
        {
            prevCount = danceLights.Length;
            lightsCounter = 0;
            secCounter = 0;
            battleScore = 0;
            opBattleScore = 0;
            ui.playerDanceScore.text = battleScore.ToString();
            ui.opponentDanceScore.text = opBattleScore.ToString();
            ui.startDance.SetActive(false);
            audio.Play();
            StartCoroutine(trackSections());
        }

    }
	
	// Update is called once per frame
	void Update () {

        //GetSpectrumAudioSource();


        Player.position = Vector3.Lerp(Player.position, new Vector3(-0.4591381f, 2.035491f, -0.7840782f), Time.deltaTime);
        Opponent.position = Vector3.Lerp(Opponent.position, new Vector3(-0.137f, 2.035491f, -0.419f), Time.deltaTime);
        Opponent.rotation = Quaternion.Lerp(Opponent.rotation, oppTarget.rotation, Time.deltaTime);
        secondsCount += Time.deltaTime;
        //var axis = WaveVR_Controller.Input(device).GetAxis(WVR_InputId.WVR_InputId_Alias1_Touchpad);

        if (secondsCount >= 2.01)
        {
            for (int i = 0; i <= danceLights.Length - 1; i++)
            {
                if (i == lightsCounter)
                {
                    danceLights[i].enabled = true;
                }
                else
                {
                    danceLights[i].enabled = false;
                }
                
            }

            if (lightsCounter + 1 >= danceLights.Length)
            {
                lightsCounter = 0;
            }
            else
            {
                lightsCounter++;
            }

            secondsCount = 0;

        }

        switch (secCounter)
        {
            case 0:
                animator.SetBool("isDanceStart", true);
                dkAnim.SetBool("isDanceStart", true);
                break;
            case 1:
                //dkAnim.SetBool("Turn1", true);
                StartCoroutine(turnComments(ui.opponentComments, "My Turn!", "Show me what ya got!"));
                break;
            case 2:
                //dkAnim.SetBool("Turn1", false);
                isTurn = true;
                //ui.danceInstructions.SetActive(true);
                StartCoroutine(turnComments(ui.playerComments, "Here we go!", "Step it up!"));
                break;
            case 3:
                //dkAnim.SetBool("Turn2", true);
                isTurn = false;
                //ui.danceInstructions.SetActive(false);
                StartCoroutine(turnComments(ui.opponentComments, "Not bad!", "Back at ya!"));
                pointLcount = 0;
                pointRcount = 0;
                shakeLcount = 0;
                shakeRcount = 0;
                break;
            case 4:
                //dkAnim.SetBool("Turn2", false);
                isTurn = true;
                //ui.danceInstructions.SetActive(true);
                StartCoroutine(turnComments(ui.playerComments, "Groovy!", "Almost got it!"));
                break;
            case 5:
                isTurn = false;
                //ui.danceInstructions.SetActive(false);
                pointLcount = 0;
                pointRcount = 0;
                shakeLcount = 0;
                shakeRcount = 0;
                break;
            default:
                break;
        }

        if (isTurn)
        {
            /*if (WaveVR_Controller.Input(device).GetTouchDown(WVR_InputId.WVR_InputId_Alias1_Touchpad))
            {
                if (axis.y >= 0.7 && axis.y <= 1.0)
                {
                    log.text += " top";
                    previousTouchState = "rShake";
                }
                if (axis.y <= -0.7 && axis.y >= -1.0)
                {
                    previousTouchState = "lShake";
                }
                if (axis.x >= 0.7 && axis.x <= 1.0)
                {
                    previousTouchState = "lPoint";
                }
                if (axis.x <= -0.7 && axis.x >= -1.0)
                {
                    previousTouchState = "rPoint";
                }
                if (WaveVR_Controller.Input(device).GetPress(WVR_InputId.WVR_InputId_Alias1_Digital_Trigger))
                {
                    if (axis.y >= 0.7 && axis.y <= 1.0)
                    {
                        log.text += " top";
                        previousTouchState = "splits";
                    }
                    if (axis.y <= -0.7 && axis.y >= -1.0)
                    {
                        previousTouchState = "victory";
                    }
                }
            }

            if (WaveVR_Controller.Input(device).GetTouchUp(WVR_InputId.WVR_InputId_Alias1_Touchpad))
            {
                log.text = "Up";
                if (axis.y <= -0.7 && axis.y >= -1.0)
                {
                    if (previousTouchState == "splits")
                    {
                        animator.SetBool("isSplits", true);
                        splitsCount++;
                        battleScore += gm.MoveScore("finishMove", splitsCount);
                        StartCoroutine(setAnimToFalse("isSplits"));
                    }
                    if (previousTouchState == "rShake")
                    {
                        animator.SetBool("isShakeR", true);
                        shakeRcount++;
                        battleScore += gm.MoveScore("basicMove", shakeRcount);
                        StartCoroutine(setAnimToFalse("isShakeR"));
                    }
                }
                if (axis.y >= 0.7 && axis.y <= 1.0)
                {
                    if (previousTouchState == "victory")
                    {
                        animator.SetBool("isVictory", true);
                        victoryCount++;
                        battleScore += gm.MoveScore("finishMove", victoryCount);
                        StartCoroutine(setAnimToFalse("isVictory"));
                    }
                    if (previousTouchState == "lShake")
                    {
                        animator.SetBool("isShakeL", true);
                        shakeLcount++;
                        battleScore += gm.MoveScore("basicMove", shakeLcount);
                        StartCoroutine(setAnimToFalse("isShakeL"));
                    }
                }
                if (axis.x <= -0.7 && axis.x >= -1.0)
                {
                    if (previousTouchState == "lPoint")
                    {
                        animator.SetBool("isPointL", true);
                        pointLcount++;
                        battleScore += gm.MoveScore("basicMove", pointLcount);
                        StartCoroutine(setAnimToFalse("isPointL"));
                    }
                }
                if (axis.x >= 0.7 && axis.x <= 1.0)
                {
                    if (previousTouchState == "rPoint")
                    {
                        animator.SetBool("isPointR", true);
                        pointRcount++;
                        battleScore += gm.MoveScore("basicMove", pointRcount);
                        StartCoroutine(setAnimToFalse("isPointR"));
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                pointLcount++;
                battleScore += gm.MoveScore("basicMove", pointLcount);
                animator.SetBool("isPointL", true);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                animator.SetBool("isPointL", false);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                pointRcount++;
                battleScore += gm.MoveScore("basicMove", pointRcount);
                animator.SetBool("isPointR", true);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                animator.SetBool("isPointR", false);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                shakeLcount++;
                battleScore += gm.MoveScore("basicMove", shakeLcount);
                animator.SetBool("isShakeL", true);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                animator.SetBool("isShakeL", false);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                shakeRcount++;
                battleScore += gm.MoveScore("basicMove", shakeRcount);
                animator.SetBool("isShakeR", true);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                animator.SetBool("isShakeR", false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                victoryCount++;
                battleScore += gm.MoveScore("finishMove", victoryCount);
                animator.SetBool("isVictory", true);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                animator.SetBool("isVictory", false);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                splitsCount++;
                battleScore += gm.MoveScore("finishMove", splitsCount);
                animator.SetBool("isSplits", true);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                animator.SetBool("isSplits", false);
            }*/
            
        }
        Debug.Log(isBeat);
        if (isBeat)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    splitsCount++;
                    battleScore += gm.MoveScore("finishMove", splitsCount);
                    animator.SetBool("isSplits", true);
                    StartCoroutine(setAnimToFalse("isSplits"));
                }
                else if (OVRInput.GetDown(OVRInput.Button.Two))
                {
                    victoryCount++;
                    battleScore += gm.MoveScore("finishMove", victoryCount);
                    animator.SetBool("isVictory", true);
                    StartCoroutine(setAnimToFalse("isVictory"));
                }
            }

            else if (OVRInput.GetDown(OVRInput.Button.One))
            {
                pointRcount++;
                battleScore += gm.MoveScore("basicMove", pointRcount);
                SetAnim("isPointR", "isShakeL", "isShakeR", "isPointL", animator);
            }
            else if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                pointLcount++;
                battleScore += gm.MoveScore("basicMove", pointLcount);
                SetAnim("isPointL", "isShakeL", "isPointR", "isShakeR", animator);
            }
            else if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                shakeLcount++;
                battleScore += gm.MoveScore("basicMove", shakeLcount);
                SetAnim("isShakeL", "isShakeR", "isPointR", "isPointL", animator);
            }
            else if (OVRInput.GetDown(OVRInput.Button.Four))
            {
                shakeRcount++;
                battleScore += gm.MoveScore("basicMove", shakeRcount);
                SetAnim("isShakeR", "isShakeL", "isPointR", "isPointL", animator);
            }

            if(isBeat != isPreviousBeat)
            {
                Debug.Log("enemyBeat");
                bool success = Random.value > 0.5f;
                if (success)
                {
                    int move = Mathf.RoundToInt(Random.Range(0, 3));
                    if (move == 0)
                    {
                        SetAnim(danceSet[move], danceSet[move + 1], danceSet[move + 2], danceSet[move + 3], dkAnim);
                    }
                    else if (move == 1)
                    {
                        SetAnim(danceSet[move], danceSet[move - 1], danceSet[move + 1], danceSet[move + 2], dkAnim);
                    }
                    else if (move == 2)
                    {
                        SetAnim(danceSet[move], danceSet[move + 1], danceSet[move - 2], danceSet[move - 1], dkAnim);
                    }
                    else if (move == 3)
                    {
                        SetAnim(danceSet[move], danceSet[move - 1], danceSet[move - 2], danceSet[move - 3], dkAnim);
                    }
                    opBattleScore += MoveScore();
                }
            }
        }


        ui.playerDanceScore.text = battleScore.ToString();

        ui.opponentDanceScore.text = opBattleScore.ToString(); ;


        if (!audio.isPlaying)
        {
            foreach(Light light in danceLights)
            {
                light.enabled = false;
            }
            pm.enabled = true;

            barSnapshot.TransitionTo(1.0f);
            animator.SetBool("isDanceStart", false);
            dkAnim.SetBool("isDanceStart", false);
            ui.playerDanceScore.text = string.Empty;
            ui.opponentDanceScore.text = string.Empty;

            //battleScore += gm.theThreadz;

            /*if (battleScore > opBattleScore)
            {
                ui.win.enabled = true;
                ui.winText.text = "You beat the Dance King!";
                gm.StartCoroutine(gm.winTimer());
            }
            else if (battleScore < opBattleScore)
            {
                ui.win.enabled = true;
                ui.winText.text = "Better luck next time!";
                gm.StartCoroutine(gm.winTimer());
            }*/
            plot.SetActive(false);
            gameObject.AddComponent<DanceBattle>().enabled = false;
            ui.startDance.SetActive(true);
            ui.songCanvas.SetActive(true);
            Destroy(this);
        }

        isPreviousBeat = isBeat;

    }

    IEnumerator trackSections()
    {
        if (secCounter < sections.Length)
        {
            tracker.clip = sections[secCounter];
            tracker.Play();
            yield return new WaitForSeconds(tracker.clip.length);
        }
        
        if(secCounter < sections.Length)
        {
            secCounter++;
            if(secCounter == 1 || secCounter == 3)
            {
                commentsCounter = 0;
                //opBattleScore += MoveScore();
                //ui.opponentDanceScore.text = opBattleScore.ToString();
            }
            else if(secCounter == 2 || secCounter == 4)
            {
                commentsCounter = 0;
            }
            StartCoroutine(trackSections());
        }
    }

    public float MoveScore()
    {
        float moveScore = 0;

        float fluidityScore;
        float balanceScore;

        float fluidRange = Random.Range(0, 10);
        float balanceRange = Random.Range(0, 10);

        /*if (fluidRange <= os.fluidity)
        {
            fluidityScore = 10;
        }
        else
        {
            fluidityScore = fluidRange;
        }

        if (balanceRange <= os.balance)
        {
            balanceScore = 10;
        }
        else
        {
            balanceScore = balanceRange;
        }

        moveScore += fluidityScore * 2;
        moveScore += balanceScore * 2;*/

        moveScore = 5;

        StartCoroutine(moveScoreTimer());
        ui.opponentMoveScore.text = moveScore.ToString();
        return moveScore;
    }

    void SetAnim(string animtrue, string animf1, string animf2, string animf3, Animator anim)
    {
        anim.SetBool(animtrue, true);
        anim.SetBool(animf1, false);
        anim.SetBool(animf2, false);
        anim.SetBool(animf3, false);
    }

    IEnumerator moveScoreTimer(){
        yield return new WaitForSeconds(3);
        ui.opponentMoveScore.text = string.Empty;
    }

    IEnumerator setAnimToFalse(string animation)
    {
        yield return new WaitForSeconds(1);
        animator.SetBool(animation, false);
    }

    IEnumerator turnComments(Text textBox, string comment1, string comment2)
    {
        if (commentsCounter == 0)
        {
            commentsCounter++;
            textBox.text = comment1;
            yield return new WaitForSeconds(2f);
            textBox.text = string.Empty;
            yield return new WaitForSeconds(6f);
            textBox.text = comment2;
            yield return new WaitForSeconds(2f);
            textBox.text = string.Empty;
        }
    }

    /*void GetSpectrumAudioSource()
    {
        audio.GetSpectrumData(samples, 0, fftWindow);

        if (samples[frequency] > spawnThreshold)
        {
            Debug.Log("badum");
        }
    }*/
}
