using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public int m_maxReputation = 10;
    public int m_startingReputation = 5;
    [Tooltip("The colour for the lantern to be when at maximum reputation")]
    public Color m_maxRepColour = Color.green;
    [Tooltip("The colour for the lantern to be when at minimum reputation")]
    public Color m_minRepColour = Color.red;

    private Renderer m_renderer;
    private int m_nCurrentRep;
    // Start is called before the first frame update
    void Start()
    {
        m_nCurrentRep = m_startingReputation;
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation));
    }

    public void AddToReputation(int nReputationToAdd)
    {
        m_nCurrentRep -= nReputationToAdd;
        m_renderer.material.color = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation));
        if (m_nCurrentRep <= 0)
        {
            // end game
            Application.Quit();
        }
    }
}
