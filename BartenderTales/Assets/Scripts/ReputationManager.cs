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
        m_nCurrentRep = m_startingReputation;
        m_renderer = GetComponent<Renderer>();
        // set colour (multiply by 2 to colourise)
        m_renderer.material.color = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            AddToReputation(-1);
    }

    public void AddToReputation(int nReputationToAdd)
    {
        // game already over, exit function
        if (m_gameOverCanvas.activeSelf)
            return;

        m_nCurrentRep += nReputationToAdd;
        // set colour (multiply by 2 to colourise)
        Color targetColour = Color.Lerp(m_minRepColour, m_maxRepColour, ((float)m_nCurrentRep / m_maxReputation)) * 2;
        StartCoroutine(LerpColour(m_renderer.material.color, targetColour));
        if (m_nCurrentRep <= 0)
        {
            // end game
            StartCoroutine(GameOver());
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

    private IEnumerator GameOver()
    {
        // stop spawning customers
        FindObjectOfType<CustomerSpawner>().m_spawnCustomers = false;

        // get reference to all customers in scene
        List<GameObject> customers = FindObjectOfType<CustomerSpawner>().m_customers;
        // loop through customers
        foreach (GameObject c in customers)
        {
            // get reference to customer component
            Customer cust = c.GetComponent<Customer>();
            // make all customers leave
            cust.Speak("this bar sucks!");
            cust.Invoke("ExitBar", Random.Range(0.5f, 3f));
        }
        // activate game over canvas
        m_gameOverCanvas.SetActive(true);
        m_finalMoneyText.text += FindObjectOfType<MoneyJar>().m_nCurrentMoney;
        // wait then load scene
        yield return new WaitForSeconds(m_timeToWaitAfterGameOverUntilRestarting);
        SceneManager.LoadSceneAsync(0);
    }
}