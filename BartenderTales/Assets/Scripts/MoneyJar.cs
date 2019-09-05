using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyJar : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_audioClipsOnLeverPull;
    [SerializeField]
    private TextMeshProUGUI m_moneyCounter;

    [HideInInspector]
    public int m_nCurrentMoney = 0;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
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
        m_audioSource?.PlayOneShot(m_audioClipsOnLeverPull[Random.Range(0, m_audioClipsOnLeverPull.Length)]);
    }
}
