using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandsAnim : MonoBehaviour
{
   // public SteamVR_Input_Sources hand;

    public Animator anim;
    public SteamVR_TrackedObject mTrackedObject = null;
    public SteamVR mDevice;



    private void Awake()    
    {
        mTrackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //SteamVR_Actions._default.GrabGrip.GetLastStateDown(SteamVR_Input_Sources.LeftHand);

        Debug.Log("Left Trigger value:" + SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand));// SteamVR_Input_Sources.LeftHand));
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Predded left");
            Debug.Log("Left Trigger value:" + SteamVR_Actions._default.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand).ToString());
            GetComponent<Animator>().SetTrigger("Grab1");
        }

        ////SteamVR_Actions._default.GrabGrip.GetLastStateDown(SteamVR_Input_Sources.RightHand);
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Predded right");
            GetComponent<Animator>().SetTrigger("Grab2");
        }
    }
}
