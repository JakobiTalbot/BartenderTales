using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyJar : MonoBehaviour
{
    [SerializeField]
    private int m_maxMoney = 10;

    private Renderer m_renderer;
    [HideInInspector]
    public int m_nCurrentMoney = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int nMoney)
    {
        m_nCurrentMoney += nMoney;
        m_renderer.material.SetFloat("_MoneyAmount", (float)m_nCurrentMoney / m_maxMoney);
        if (m_nCurrentMoney >= m_maxMoney)
        {
            // win game
        }
    }
}
