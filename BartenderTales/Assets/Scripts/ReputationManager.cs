using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [SerializeField]
    private GameObject m_gameOverCanvas;
    [SerializeField]
    private TextMeshPro m_finalMoneyText;
    [SerializeField]
    private float m_timeToWaitAfterGameOverUntilRestarting = 10f;

    private Renderer m_renderer;
    private int m_nCurrentRep;
    private float m_fLerpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // set reputation to starting reputation
        m_nCurrentRep = m_startingReputation;
        // get reference to renderer
        m_renderer = GetComponent<Renderer>();
        // set colour (multiply by 2 to colourise)
        m_renderer.material.color = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
    }

    /// <summary>
    /// Add specified integer to reputation then set lantern colour and check for game over
    /// </summary>
    /// <param name="nReputationToAdd"> The amount of reputation to add </param>
    public void AddToReputation(int nReputationToAdd)
    {
        // game already over, exit function
        if (m_gameOverCanvas.activeSelf)
            return;

        // add to reputation
        m_nCurrentRep += nReputationToAdd;

        // set colour (multiply by 2 to colourise)
        Color targetColour = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
        StartCoroutine(LerpColour(m_renderer.material.color, targetColour));
    }

    /// <summary>
    /// Coroutine to lerp the renderer material's colour between two colours
    /// </summary>
    /// <param name="startColour"> The colour to lerp from </param>
    /// <param name="targetColour"> The colour to lerp to </param>
    IEnumerator LerpColour(Color startColour, Color targetColour)
    {
        // while colour hasn't reached target colour
        while (m_fLerpTimer < 1f)
        {
            // lerp to target colour
            m_fLerpTimer += Time.deltaTime / m_colourLerpTime;
            m_renderer.material.color = Color.Lerp(startColour, targetColour, m_fLerpTimer);

            // wait for next frame
            yield return null;
        }

        // set colour to target colour
        m_renderer.material.color = targetColour;
        m_fLerpTimer = 0f;
    }
}