using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShakerCap : MonoBehaviour
{
    private Shaker m_shaker;

    private void Awake()
    {
        m_shaker = FindObjectOfType<Shaker>();
    }

    public void EnableHeld()
    {
        m_shaker.RemoveCap();
    }
    public void DisableHeld()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}