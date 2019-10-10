using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandBlending : MonoBehaviour
{
    [SerializeField]
    private SteamVR_Input_Sources m_inputSource;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_animator.SetFloat("BlendHandFloat", SteamVR_Actions._default.Squeeze.GetAxis(m_inputSource));
    }
}