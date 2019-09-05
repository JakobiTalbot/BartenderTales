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
    [SerializeField]
    private AudioClip[] m_audioClipsOnLeverPull;

    private Rigidbody m_rb;
    private Vector3 m_originalPos;
    private bool m_bLeverAlreadyPulled = false;
    private AudioSource m_audioSource;

    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
        m_originalPos = m_rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if lever is pulled all the way
        if (transform.localRotation.eulerAngles.x > 85f
            && transform.localRotation.eulerAngles.x < 95f
            && !m_bLeverAlreadyPulled)
        {
            // invoke callbacks
            m_onLeverPulled.Invoke();
            // ensure the lever is not considered pulled until after it is reset
            m_bLeverAlreadyPulled = true;
            // play audio
            m_audioSource?.PlayOneShot(m_audioClipsOnLeverPull[Random.Range(0, m_audioClipsOnLeverPull.Length)]);
            // start moving lever back
            StartCoroutine(ResetLever());
        }
    }

    /// <summary>
    /// Resets the lever's X Euler rotation back to 0
    /// </summary>
    private IEnumerator ResetLever()
    {
        Vector3 moveDir = (m_originalPos - m_rb.position).normalized;
        // while lever is not at start position
        while (transform.localRotation.eulerAngles.x > 5f)
        {
            // move towards start position
            m_rb.velocity = moveDir * m_leverReturnSpeed;
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