using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyJar : MonoBehaviour
{
    public AudioSource coinSound;

    [SerializeField]
    private TextMeshProUGUI m_moneyCounter;

    [HideInInspector]
    public int m_nCurrentMoney = 0;

    public void AddMoney(int nMoney)
    {
        m_nCurrentMoney += nMoney;
        // set text on money counter
        m_moneyCounter.text = m_nCurrentMoney.ToString();

        if (coinSound != null)
        {
            coinSound.Play();
        }
    }
}
