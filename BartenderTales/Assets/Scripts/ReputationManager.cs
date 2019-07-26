using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public int m_maxReputation = 10;
    public int m_startingReputation = 5;
    public float m_colourLerpTime = 1f;
    [Header("Colours")]
    [Tooltip("The colour for the lantern to be when at maximum reputation")]
    public Color m_maxRepColour = Color.green;
    [Tooltip("The colour for the lantern to be when at minimum reputation")]
    public Color m_minRepColour = Color.red;

    private Renderer m_renderer;
    private int m_nCurrentRep;
    private float m_fLerpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_nCurrentRep = m_startingReputation;
        m_renderer = GetComponent<Renderer>();
        // set colour (multiply by 2 to colourise)
        m_renderer.material.color = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
    }

    public void AddToReputation(int nReputationToAdd)
    {
        m_nCurrentRep += nReputationToAdd;
        // set colour (multiply by 2 to colourise)
        Color targetColour = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
        StartCoroutine(LerpColour(m_renderer.material.color, targetColour));
        if (m_nCurrentRep <= 0)
        {
            // end game
            Application.Quit();
        }
    }

    IEnumerator LerpColour(Color startColour, Color targetColour)
    {
        while (m_fLerpTimer < 1f)
        {
            m_fLerpTimer += Time.deltaTime / m_colourLerpTime;
            m_renderer.material.color = Color.Lerp(startColour, targetColour, m_fLerpTimer);
            yield return new WaitForEndOfFrame();
        }

        m_renderer.material.color = targetColour;
        m_fLerpTimer = 0f;
    }
}