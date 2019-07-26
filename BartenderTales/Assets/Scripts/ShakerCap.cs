using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerCap : MonoBehaviour
{
    private bool m_bHeld = false;
    private bool m_bOnShaker = false;
    private Shaker m_shaker;
    private void OnTriggerEnter(Collider other)
    {
        if (!m_bOnShaker
            && other.GetComponent<Shaker>()
            && !m_bHeld)
        {
            if (!m_shaker)
                m_shaker = other.GetComponent<Shaker>();
            m_bOnShaker = true;
            m_shaker.PlaceCap(gameObject);
        }
    }

    public void EnableHeld()
    {
        m_bHeld = true;
        if (m_bOnShaker)
        {
            m_bOnShaker = false;
            m_shaker.RemoveCap();
        }
    }
    public void DisableHeld()
    {
        m_bHeld = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}