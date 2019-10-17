using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyJar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_moneyCounter;
    [SerializeField]
    private AudioClip[] m_coinDropSounds;

    [HideInInspector]
    public int m_nCurrentMoney = 0;

    private AudioSource m_audioSource;
    private Rigidbody m_rb;
    private Vector3 m_v3OriginalPos;
    private Quaternion m_originalRot;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
        m_v3OriginalPos = transform.position;
        m_originalRot = transform.rotation;
    }

    /// <summary>
    /// Adds specified amount of money to the money jar
    /// </summary>
    /// <param name="nMoney"> The amount of money to add to the coin jar </param>
    public void AddMoney(int nMoney)
    {
        m_nCurrentMoney += nMoney;
        // set text on money counter
        m_moneyCounter.text = m_nCurrentMoney.ToString();
        // play random audio clip
        m_audioSource.PlayOneShot(m_coinDropSounds[Random.Range(0, m_coinDropSounds.Length)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Boundary>())
        {
            transform.position = m_v3OriginalPos;
            transform.rotation = m_originalRot;
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
}
