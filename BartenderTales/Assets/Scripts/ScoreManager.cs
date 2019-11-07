using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int m_nCorrectOrders = 0;
    private int m_nIncorrectOrders = 0;

    private int m_nConsecutiveCorrectOrders = 0;

    private int m_nTotalScore = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_nTotalScore += 100;
        }
    }
    public void AddCorrectOrder()
    {
        ++m_nCorrectOrders;
        m_nTotalScore += ++m_nConsecutiveCorrectOrders;
    }

    public void AddIncorrectOrder()
    {
        ++m_nIncorrectOrders;
        // reset consecutive correct orders
        m_nConsecutiveCorrectOrders = 0;
    }

    public int GetTotalScore() => m_nTotalScore + FindObjectOfType<MoneyJar>().m_nCurrentMoney;
    public int GetCorrectOrderCount() => m_nCorrectOrders;
    public int GetIncorrectOrderCount() => m_nIncorrectOrders;
}