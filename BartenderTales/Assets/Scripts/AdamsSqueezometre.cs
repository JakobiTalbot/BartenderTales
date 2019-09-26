using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AdamsSqueezometre : MonoBehaviour
{
    public SteamVR_Input_Sources LeftInputSource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;

    private void Update()
    {
        Debug.Log("Left Trigger value:" + LeftInputSource.ToString() + SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource).ToString());
        Debug.Log("Right Trigger value:" + RightInputSource.ToString() + SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource).ToString());
    }

}

