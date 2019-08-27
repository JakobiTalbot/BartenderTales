using System.Collections;
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
    private bool m_bLeverAlreadyPulled = false;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // if lever is pulled all the way
        if (transform.localRotation.eulerAngles.x > 89f
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
        while (transform.localRotation.eulerAngles.x > 1f)
        {
            m_rb.angularVelocity = new Vector3(-m_leverReturnSpeed, 0, 0);
            yield return null;
        }
        m_rb.isKinematic = true;
        m_bLeverAlreadyPulled = false;
    }

    public void DisableKinematic()
    {
        m_rb.isKinematic = false;
    }
}