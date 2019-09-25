using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandsAnim : MonoBehaviour
{
   // public SteamVR_Input_Sources hand;

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        //SteamVR_Actions._default.GrabGrip.GetLastStateDown(SteamVR_Input_Sources.LeftHand);

        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Predded left6");
            GetComponent<Animator>().SetTrigger("Grab");
        }

        //SteamVR_Actions._default.GrabGrip.GetLastStateDown(SteamVR_Input_Sources.RightHand);
        if (SteamVR_Actions._default.GrabPinch.GetLastStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Predded right");
            GetComponent<Animator>().SetTrigger("Grab");
        }
    }
}
