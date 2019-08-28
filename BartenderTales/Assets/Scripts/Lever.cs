﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onLeverPulled;
    [SerializeField]
    private float m_leverReturnSpeed = 0.5f;

    private Rigidbody m_rb;
    private Vector3 m_originalPos;
    private bool m_bLeverAlreadyPulled = false;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_originalPos = m_rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.localRotation.eulerAngles.x);
        // if lever is pulled all the way
        if (transform.localRotation.eulerAngles.x > 85f
            && transform.localRotation.eulerAngles.x < 95f
            && !m_bLeverAlreadyPulled)
        {
            // invoke callbacks
            m_onLeverPulled.Invoke();
            // ensure the lever is not considered pulled until after it is reset
            m_bLeverAlreadyPulled = true;
            // start moving lever back
            StartCoroutine(ResetLever());
        }
    }

    /// <summary>
    /// Resets the lever's X Euler rotation back to 0
    /// </summary>
    private IEnumerator ResetLever()
    {
        Vector3 startPos = m_rb.position;
        // while lever is not at start position
        while (transform.localRotation.eulerAngles.x > 5f)
        {
            // move towards start position
            m_rb.velocity = (m_originalPos - startPos) * m_leverReturnSpeed;
            yield return null;
        }
        // lever has returned to start position
        m_rb.isKinematic = true;
        m_bLeverAlreadyPulled = false;
    }

    public void DisableKinematic()
    {
        m_rb.isKinematic = false;
    }
}