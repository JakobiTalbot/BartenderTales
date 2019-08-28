using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyJar : MonoBehaviour
{
    [SerializeField]
    private int m_maxMoney = 10;
    [SerializeField]
    private TextMeshProUGUI m_moneyCounter;

    [HideInInspector]
    public int m_nCurrentMoney = 0;

    public void AddMoney(int nMoney)
    {
        m_nCurrentMoney += nMoney;
        // set text on money counter
        m_moneyCounter.text = m_nCurrentMoney.ToString();
        if (m_nCurrentMoney >= m_maxMoney)
        {
            // win game
        }
    }
}
