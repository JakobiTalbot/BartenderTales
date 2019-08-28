using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyJar : MonoBehaviour
{
    [SerializeField]
    private int m_maxMoney = 10;
    [SerializeField]
    private TextMeshProUGUI m_moneyCounter;
    [SerializeField]
    private GameObject m_winCanvas;
    [SerializeField]
    private TextMeshPro m_moneyText;
    [SerializeField]
    private float m_timeToWaitAfterWinning = 5f;

    [HideInInspector]
    public int m_nCurrentMoney = 0;

    private bool m_bAlreadyWon = false;

    public void AddMoney(int nMoney)
    {
        m_nCurrentMoney += nMoney;
        // set text on money counter
        m_moneyCounter.text = m_nCurrentMoney.ToString();
        if (m_nCurrentMoney >= m_maxMoney
            && !m_bAlreadyWon)
        {
            StartCoroutine(WinGame());
            m_bAlreadyWon = true;
        }
    }

    private IEnumerator WinGame()
    {
        // activate win stuff
        m_winCanvas.SetActive(true);
        m_moneyText.text += m_nCurrentMoney;

        // wait before resetting scene
        yield return new WaitForSeconds(m_timeToWaitAfterWinning);

        // reset scene
        SceneManager.LoadSceneAsync(0);
    }
}
