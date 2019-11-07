using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;
using System.Text;

public struct Score
{
    public string name;
    public int value;

    public Score(Score score)
    {
        name = score.name;
        value = score.value;
    }
    public Score (string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}

public class HighScoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_highScoreCanvas;
    [SerializeField]
    private GameObject m_newHighScoreCanvas;
    [SerializeField]
    private TextMeshPro[] m_highScoreTexts;
    [SerializeField]
    private int m_scoresCount = 7;

    private Score[] m_scores;
    private int m_currentGameScore;

    private void Awake()
    {
        m_scores = new Score[m_scoresCount];
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        // if first time playing
        if (PlayerPrefs.GetString("ScoreName0", "null") == "null")
        {
            for (int i = 0; i < m_scoresCount; ++i)
            {
                m_scores[i].name = "AAA";
                m_scores[i].value = (m_scoresCount - i) * 10;
            }
            return;
        }

        for (int i = 0; i < m_scoresCount; ++i)
        {
            m_scores[i].name = PlayerPrefs.GetString("ScoreName" + i.ToString());
            m_scores[i].value = PlayerPrefs.GetInt("ScoreValue" + i.ToString());
        }
    }

    public void DisplayHighScores()
    {
        m_highScoreCanvas.SetActive(true);

        for (int i = 0; i < m_scoresCount; ++i)
        {
            // set high scores
            m_highScoreTexts[i].text = (i + 1).ToString() + ".   " + m_scores[i].name + "   " + m_scores[i].value.ToString();
        }
    }

    public void CheckHighScore(int score)
    {
        if (score > m_scores[m_scoresCount - 1].value)
        {
            m_currentGameScore = score;
            // get player's initials
            SteamVR.instance.overlay.ShowKeyboard(0, 0, "Enter your initials", 3, "", true, 0);

            // keyboard close listener
            SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);

            // display new high score text
            m_newHighScoreCanvas.SetActive(true);
        }
        else
        {
            DisplayHighScores();
        }
    }

    private void OnKeyboardClosed(VREvent_t args)
    {
        StringBuilder stringBuilder = new StringBuilder(3);
        SteamVR.instance.overlay.GetKeyboardText(stringBuilder, 3);

        // add to highscores
        m_scores[m_scores.Length - 1] = new Score(stringBuilder.ToString(), m_currentGameScore);
        SortHighScores();
        DisplayHighScores();
        SaveHighScores();
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