using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WaveVR_Log;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    //public WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;

    public Animator animator;
    public Text log;

    public float x;
    public float z;

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
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            animator.SetBool("isBack", true);
            z = 1;
        }
        else
        {
            animator.SetBool("isBack", false);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            animator.SetBool("isRight", true);
            x = 1;
        }
        else
        {
            animator.SetBool("isRight", false);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            animator.SetBool("isForward", true);
            z = -1;
        }
        else
        {
            animator.SetBool("isForward", false);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            animator.SetBool("isLeft", true);
            x = -1;
        }
        else
        {
            animator.SetBool("isLeft", false);
        }
        var dirX = x * Time.deltaTime * 1f;
        var dirZ = z * Time.deltaTime * 1f;
        transform.Translate(dirX, 0, dirZ);
        x = 0;
        z = 0;

        /*if (WaveVR_Controller.Input(device).GetTouch(WVR_InputId.WVR_InputId_Alias1_Touchpad))
        {
            var axis = WaveVR_Controller.Input(device).GetAxis(WVR_InputId.WVR_InputId_Alias1_Touchpad);

            log.text = "Axis: " + axis;

            if (WaveVR_Controller.Input(device).GetPress(WVR_InputId.WVR_InputId_Alias1_Touchpad))
            {

                if (axis.x >= 0.3f)
                {
                    animator.SetBool("isRight", true);
                }
                else
                {
                    animator.SetBool("isRight", false);
                }

                if (axis.x <= -0.3f)
                {
                    animator.SetBool("isLeft", true);
                }
                else
                {
                    animator.SetBool("isLeft", false);
                }
                if (axis.y >= 0.3f)
                {
                    animator.SetBool("isBack", true);
                }
                else
                {
                    animator.SetBool("isBack", false);
                }
                if (axis.y <= -0.3f)
                {
                    animator.SetBool("isForward", true);
                }
                else
                {
                    animator.SetBool("isForward", false);
                }

                var x = axis.x * Time.deltaTime * 1f;
                var z = axis.y * Time.deltaTime * 1f;
                transform.Translate(x, 0, z);
            }
        }

        if (WaveVR_Controller.Input(device).GetPressUp(WVR_InputId.WVR_InputId_Alias1_Touchpad))
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isForward", false);
            animator.SetBool("isRight", false);
        }*/

        /*var a = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var b = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, a, 0);
        transform.Translate(0, 0, b);
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isBack", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isBack", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("isLeft", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isLeft", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isForward", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("isForward", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isRight", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isRight", false);
        }*/

    }

}
