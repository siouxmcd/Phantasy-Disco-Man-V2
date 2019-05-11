using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public Image fade;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        fade.canvasRenderer.SetAlpha(1.0f);
        FadeIn();
    }

    public void FadeIn()
    {
        fade.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    public void FadeOut()
    {
        fade.CrossFadeAlpha(1.0f, 0.5f, false);
    }
}
