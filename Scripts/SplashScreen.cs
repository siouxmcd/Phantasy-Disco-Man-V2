using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using wvr;

public class SplashScreen : MonoBehaviour {

    public WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;

    public Image cuttlefishLogo;
    public Image gameLogo;
    public Camera[] cam;

    FadeManager fm;


    public string loadLevel = "Skills";

    WVR_InputId[] buttonIds = new WVR_InputId[]
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
    };

    IEnumerator Start()
    {
        fm = GetComponent<FadeManager>();
        cuttlefishLogo.canvasRenderer.SetAlpha(0.0f);
        gameLogo.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForSeconds(2f);
        FadeInC();
        yield return new WaitForSeconds(4f);
        FadeOutC();
        yield return new WaitForSeconds(2.5f);
        FadeInL();
        yield return new WaitForSeconds(4f);
        FadeOutL();
        //ChangeBackground();
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(ChangeScene());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ChangeScene());
        }
        if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Digital_Trigger))
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        fm.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(loadLevel);
    }

    void FadeInC()
    {
        cuttlefishLogo.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOutC()
    {
        cuttlefishLogo.CrossFadeAlpha(0, 1.5f, false);
    }

    void FadeInL()
    {
        gameLogo.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOutL()
    {
        gameLogo.CrossFadeAlpha(0, 1.5f, false);
    }

    /*void ChangeBackground()
    {
        float t = Mathf.PingPong(Time.time, 3.0F) / 3.0F;
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("7C400000", out myColor);
        //cam.backgroundColor = Color.Lerp(Color.white, myColor, t);
    }*/

}
