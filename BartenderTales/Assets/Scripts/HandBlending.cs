using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandBlending : MonoBehaviour
{
    [SerializeField]
    private string m_axisName;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand));
        //m_animator.SetFloat("BlendHandFloat", SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand));
        m_animator.SetFloat("BlendHandFloat", Input.GetAxis(m_axisName));
    }
}