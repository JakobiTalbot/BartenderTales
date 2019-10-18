using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct Score
{
    public string name;
    public int value;

    public Score(Score score)
    {
        name = score.name;
        value = score.value;
    }
}

public class HighScoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_highScoreCanvas;
    [SerializeField]
    private TextMeshPro[] m_highScoreTexts;
    [SerializeField]
    private int m_scoresCount = 7;

    private Score[] m_scores;

    private void Awake()
    {
        m_scores = new Score[m_scoresCount];
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        for (int i = 0; i < m_scoresCount; ++i)
        {
            m_scores[i].name = PlayerPrefs.GetString("ScoreName" + i.ToString());
            m_scores[i].value = PlayerPrefs.GetInt("ScoreValue" + i.ToString());
        }
    }

    public void DisplayHighScores()
    {

    }

    public void CheckHighScore(int score)
    {
        if (score > m_scores[m_scoresCount - 1].value)
        {
            // get players initials
        }
    }

    public void SaveHighScores()
    {
        for (int i = 0; i < m_scoresCount; ++i)
        {
            PlayerPrefs.SetString("ScoreName" + i.ToString(), m_scores[i].name);
            PlayerPrefs.SetInt("ScoreValue" + i.ToString(), m_scores[i].value);
        }
        PlayerPrefs.Save();
    }

    private void SortHighScores()
    {
        // bubble sort descending
        for (int i = 0; i < m_scores.Length; ++i)
        {
            for (int j = i + 1; j < m_scores.Length; ++j)
            {
                if (m_scores[i].value < m_scores[j].value)
                {
                    // switch
                    Score temp = new Score(m_scores[i]);
                    m_scores[i] = m_scores[j];
                    m_scores[j] = temp;
                }
            }
        }
    }
}