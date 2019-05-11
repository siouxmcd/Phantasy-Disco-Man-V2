using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public RpgSkills rpg;

    public int improvSkill;
    public int fluidSkill;
    public int balanceSkill;
    public int movesetSkill;

    public Animator bartender;

    private float theDo;
    private float theSneaks;
    public float theThreadz;
    private float theBling;

    private float dirtyScale = 1f;
    private float secondsCount;

    private int beerCount = 0;

    private IEnumerator Bco;
    private IEnumerator Cco;

    public UIManager2 ui;
    public Text log;
    public GameObject plot;


    // Use this for initialization
    void Start () {
        rpg = GameObject.Find("Skills").GetComponent<RpgSkills>();
        theDo = 1f;
        theThreadz = 1f;
        improvSkill = rpg.improv;
        fluidSkill = rpg.fluidity;
        balanceSkill = rpg.balance;
    }
	
	// Update is called once per frame
	void Update () {
        secondsCount += Time.deltaTime;
        if(secondsCount >= 60)
        {
            theDo -= dirtyScale;
            theThreadz -= dirtyScale;
            secondsCount = 0;
            Debug.Log("gettin dirty " + theThreadz);
        }
        if(theDo <= 1f)
        {
            theDo = 1f;
        }
        if (theThreadz <= 1f)
        {
            theThreadz = 1f;
        }
    }

    public float MoveScore(string move, int counter)
    {
        float moveScore = 0;

        float fluidityScore;
        float balanceScore;

        float fluidRange = Random.Range(0, 10);
        float balanceRange = Random.Range(0, 10);

        if (fluidRange <= fluidSkill)
        {
            fluidityScore = 10;
        } else
        {
            fluidityScore = fluidRange / 2;
        }

        if (balanceRange <= balanceSkill)
        {
            balanceScore = 10;
        }
        else
        {
            balanceScore = balanceRange / 2;
        }

        moveScore += fluidityScore;
        moveScore += balanceScore;

        if (move == "basicMove")
        {
            moveScore = 5;
        }
        else if (move == "finishMove")
        {
            moveScore = 10;
        }

        StartCoroutine(moveScoreTimer());
        ui.playerMoveScore.text = moveScore.ToString();
        return moveScore;
    }

    public void Laundry()
    {
        theThreadz = 5f;
        ui.laundry.SetActive(true);
        StartCoroutine(laundryTimer());
    }

    public void Shower()
    {
        theDo = 5f;
    }

    public void Drink (string drink)
    {
        bartender.SetBool("isSold", true);
        StartCoroutine(bartenAnim());
        ui.barCanvas.enabled = false;
        
        if(drink == "Cactus")
        {
            ui.cactus.gameObject.SetActive(true);
            fluidSkill = 0;
            improvSkill = 10;
            balanceSkill = 3;
            Cco = CactusTime(30.0f);
            StartCoroutine(Cco);
        }
        if(drink == "Beer")
        {
            ui.beer.gameObject.SetActive(true);
            beerCount++;
            if(beerCount == 1)
            {
                fluidSkill += 2;
                Bco = BeerTime(10.0f);
                StartCoroutine(Bco);
            }
            if(beerCount == 2)
            {
                balanceSkill -= 2;
                Bco = BeerTime(10.0f);
                StartCoroutine(Bco);
            }
            if (beerCount >= 3)
            {
                balanceSkill -= 3;
                Bco = BeerTime(10.0f);
                StartCoroutine(Bco);
            }
        }
    }

    IEnumerator CactusTime(float time)
    {
        yield return new WaitForSeconds(time);
        fluidSkill = rpg.fluidity;
        improvSkill = rpg.improv;
        balanceSkill = rpg.balance;
    }

    IEnumerator BeerTime(float time)
    {
        yield return new WaitForSeconds(time);
        if(beerCount == 1)
        {
            fluidSkill -= 2;
        } else if (beerCount == 2)
        {
            balanceSkill += 2;
        } else if (beerCount >= 3)
        {
            balanceSkill += 3;
        }
        beerCount--;
    }

    IEnumerator bartenAnim()
    {
        yield return new WaitForSeconds(3);
        bartender.SetBool("isSold", false);
        ui.cactus.gameObject.SetActive(false);
        ui.beer.gameObject.SetActive(false);
    }

    IEnumerator moveScoreTimer()
    {
        yield return new WaitForSeconds(3);
        ui.playerMoveScore.text = string.Empty;
    }

    IEnumerator laundryTimer()
    {
        yield return new WaitForSeconds(3);
        ui.laundry.SetActive(false);
    }

    public IEnumerator winTimer()
    {
        yield return new WaitForSeconds(4);
        ui.winText.text = string.Empty;
        ui.win.enabled = false;
    }
}
