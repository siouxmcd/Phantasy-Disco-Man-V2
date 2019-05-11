using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public int availSkills;
    public RpgSkills skills;

    public GameObject button;

    public Image[] totalSkills;
    public Image[] improvSkills;
    public Image[] fluidSkills;
    public Image[] balanceSkills;

    public int improv;
    public int fluidity;
    public int balance;

    public FadeManager fm;

    // Use this for initialization
    void Start () {
        availSkills = 18;
        improv = -1;
        fluidity = -1;
        balance = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if (availSkills == 0)
        {
            button.SetActive(true);
        }
        else
        {
            if (button.activeSelf)
            {
                button.SetActive(false);
            }
        }
	}

    public void increaseImprov()
    {
        if(availSkills > 0)
        {
            improv++;
            improvSkills[improv].enabled = true;
            DecreaseAvailSkill();
        }
    }
    public void decreaseImprov()
    {
        improvSkills[improv].enabled = false;
        improv--;
        IncreaseAvailSkill();
    }
    public void increaseFluid()
    {
        if(availSkills > 0)
        {
            fluidity++;
            fluidSkills[fluidity].enabled = true;
            DecreaseAvailSkill();
        }
    }
    public void decreaseFluid()
    {
        fluidSkills[fluidity].enabled = false;
        fluidity--;
        IncreaseAvailSkill();
    }
    public void increaseBal()
    {
        if (availSkills > 0)
        {
            balance++;
            balanceSkills[balance].enabled = true;
            DecreaseAvailSkill();
        }
    }
    public void decreaseBal()
    {
        balanceSkills[balance].enabled = false;
        balance--;
        IncreaseAvailSkill();
    }

    public void IncreaseAvailSkill()
    {
        if (availSkills >= 0 && availSkills <= 18)
        {
            totalSkills[availSkills].enabled = true;
            availSkills++;
        }
        else if (availSkills >= 18)
        {
            availSkills = 18;
        }
        else if (availSkills <= 0)
        {
            availSkills = 0;
        }
    }

    public void DecreaseAvailSkill()
    {
        if (availSkills >= 0 && availSkills <= 18)
        {
            availSkills--;
            totalSkills[availSkills].enabled = false;
        }
        else if (availSkills >= 18)
        {
            availSkills = 18;
        }
        else if (availSkills <= 0)
        {
            availSkills = 0;
        }
    }

    public void LoadMainScene()
    {
        skills.improv = improv + 1;
        skills.fluidity = fluidity + 1;
        skills.balance = balance + 1;
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        fm.FadeOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main");
    }
}
