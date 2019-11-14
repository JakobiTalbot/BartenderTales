using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum CustomerType
{
    Goblin = 0,
    Minotaur,
    CatMan
}

public class CustomerSpawner : MonoBehaviour
{
    // possible refactor: create array of points and sort by distance to bar, spawn customer at lowest distance

    public GameObject[] m_customerPrefabs;
    public List<Transform> m_servingPoints;
    public List<Transform> m_waitingPoints;
    public Transform m_coinDropPoint;
    
    public Transform[] m_spawnPoints;

    [HideInInspector]
    public List<CustomerVoice> m_activeCustomerVoices;
    public const int MaxAmountOfCustomersToSpeakAtOnce = 1;

    [SerializeField]
    private int m_numberOfTutorialCustomers = 3;
    [SerializeField]
    private GameObject[] m_hatPrefabs;
    [SerializeField]
    private Vector2 m_randomRangeBetweenHappyHours;
    [SerializeField]
    private Vector2 m_randomLengthForHappyHour;
    [SerializeField]
    [Tooltip("The random range of time between a customer spawning during happy hour")]
    private Vector2 m_randomRangeCustomerSpawnHappyHour;
    [SerializeField]
    [Tooltip("The random range of time between a customer spawning not during happy hour")]
    private Vector2 m_randomRangeCustomerSpawnNotHappyHour;
    [SerializeField]
    private GameObject m_startLever;
    [SerializeField]
    private float m_gameTimeSeconds = 300f;
    [SerializeField]
    private Transform[] m_clockHands;

    [Header("Time Over Stuff")]
    [SerializeField]
    private GameObject m_timeOverCanvas;
    [SerializeField]
    private TextMeshPro m_totalScoreText;
    [SerializeField]
    private TextMeshPro m_correctOrdersText;
    [SerializeField]
    private TextMeshPro m_incorrectOrdersText;

    [HideInInspector]
    public List<GameObject> m_customers;
    [HideInInspector]
    public bool m_spawnCustomers = true;

    private ScoreManager m_scoreManager;
    private float m_fCustomerSpawnTimer = 0f;
    private bool m_bHappyHour = false;
    private float m_fGameTimer = 0f;

    [Header("Music")]
    public AudioSource mainMenuMusic;
    public AudioSource calmHourMusic;
    public float fadeTime;
    public float m_gameMusicVolume = 0.2f;

    public static CustomerSpawner instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        m_customers = new List<GameObject>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void Fade()
    {
        StartCoroutine(AudioController.FadeOut(mainMenuMusic, fadeTime));
        StartCoroutine(AudioController.FadeIn(calmHourMusic, fadeTime, m_gameMusicVolume));
    }

    /// <summary>
    /// Coroutine to spawn customers with specified time intervals
    /// </summary>
    private IEnumerator CustomerSpawnLoop()
    {
        SpawnCustomer(false);

        while (true)
        {
            if (m_bHappyHour)
                yield return new WaitForSeconds(Random.Range(m_randomRangeCustomerSpawnHappyHour.x, m_randomRangeCustomerSpawnHappyHour.y));
            else
                yield return new WaitForSeconds(Random.Range(m_randomRangeCustomerSpawnNotHappyHour.x, m_randomRangeCustomerSpawnNotHappyHour.y));

            // spawn customer when available
            yield return new WaitUntil(() => SpawnCustomer(false));
        }
    }

    public void SpawnTutorialCustomer()
    {
        // if still spawning tutorial customers
        if (m_numberOfTutorialCustomers-- > 0)
            SpawnCustomer(true);
        else
            StartCoroutine(CustomerSpawnLoop());
    }

    /// <summary>
    /// Coroutine to switch states between happy hour and not happy hour
    /// </summary>
    private IEnumerator HappyHourTimer()
    {
        while (true)
        {
            if (!m_bHappyHour)
            {
                yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenHappyHours.x, m_randomRangeBetweenHappyHours.y));
                // happy hour
                m_bHappyHour = true;
                // restart customer spawn coroutine
                StopCoroutine(CustomerSpawnLoop());
                StartCoroutine(CustomerSpawnLoop());
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(m_randomLengthForHappyHour.x, m_randomLengthForHappyHour.y));
                m_bHappyHour = false;
            }
        }
    }

    /// <summary>
    /// Attempt to spawn a customer
    /// </summary>
    /// <returns> Whether a customer was spawned or not </returns>
    private bool SpawnCustomer(bool bTutorialCustomer)
    {
        // dont spawn if there is no spaces
        if (!(m_servingPoints.Count > 0 || m_waitingPoints.Count > 0))
            return false;

        // create variables
        Transform destPoint;
        bool bWait = false;

        // if no free serving points
        if (m_servingPoints.Count <= 0)
        {
            int point = Random.Range(0, m_waitingPoints.Count);
            destPoint = m_waitingPoints[point];
            m_waitingPoints.RemoveAt(point);
            bWait = true;
        }
        else // if there are free serving points
        {
            int point = Random.Range(0, m_servingPoints.Count);
            destPoint = m_servingPoints[point];
            m_servingPoints.RemoveAt(point);
        }

        // get random spawn point
        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];

        // spawn customer
        m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], spawnPoint.position, spawnPoint.rotation));

        Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();
        cust.GetReferences();

        cust.SetDestination(destPoint, bWait);
        if (cust.GetCustomerType() != CustomerType.CatMan)
            cust.SetHat(GetRandomHat());
        cust.SetCoinDropPos(m_coinDropPoint.position);
        cust.SetTutorialCustomer(bTutorialCustomer);

        return true;
    }

    /// <summary>
    /// Start the coroutines for the game loop
    /// </summary>
    public void StartSpawning()
    {
        SpawnTutorialCustomer();
        m_customers[0].GetComponent<Customer>().SetOrder(PotionName.CosyFire);
        StartCoroutine(HappyHourTimer());
        StartCoroutine(GameTimer());
        Destroy(m_startLever);
    }

    /// <summary>
    /// The coroutine to track game state
    /// </summary>
    private IEnumerator GameTimer()
    {
        yield return new WaitUntil(CountDown);

        // stop spawning customers and make all current customers leave the bar
        StopCoroutine(CustomerSpawnLoop());
        foreach (GameObject g in m_customers)
            g.GetComponent<Customer>().ExitBar();

        // timer runs out
        m_timeOverCanvas.SetActive(true);

        ScoreManager s = FindObjectOfType<ScoreManager>();
        m_totalScoreText.text += s.GetTotalScore().ToString();
        m_correctOrdersText.text += s.GetCorrectOrderCount().ToString();
        m_incorrectOrdersText.text += s.GetIncorrectOrderCount().ToString();

        yield return new WaitForSeconds(5f);

        m_timeOverCanvas.SetActive(false);

        // display high scores
        HighScoreManager highScoreManager = FindObjectOfType<HighScoreManager>();
        highScoreManager.CheckHighScore(s.GetTotalScore());
    }

    /// <summary>
    /// Counts the remaining game time and rotates the clock hand to match how much time is left
    /// </summary>
    /// <returns> Returns whether the game timer has ran out or not </returns>
    private bool CountDown()
    {
        m_fGameTimer += Time.deltaTime;
        float fRot = (m_fGameTimer / m_gameTimeSeconds) * 360f;
        // rotate clock hand
        foreach (Transform t in m_clockHands)
            t.localRotation = Quaternion.Euler(t.localRotation.x, t.localRotation.y, fRot);
        return m_fGameTimer >= m_gameTimeSeconds;
    }

    public GameObject GetRandomHat()
    {
        // decide whether to wear hat or not
        if (Random.Range(0, m_hatPrefabs.Length + 1) > 0)
        {
            // pick hat
            return m_hatPrefabs[Random.Range(0, m_hatPrefabs.Length)];
        }

        return null;
    }

    public ScoreManager GetScoreManager() => m_scoreManager;
}