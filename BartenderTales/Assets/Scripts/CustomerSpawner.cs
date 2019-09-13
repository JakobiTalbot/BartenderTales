using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

struct WantedCustomer
{
    public GameObject customer;
    public Texture2D mugshot;

    public WantedCustomer(GameObject customer, Texture2D mugshot)
    {
        this.customer = customer;
        this.mugshot = mugshot;
    }
}

struct WantedPoster
{
    public Renderer poster;
    public bool hasCustomer;

    public WantedPoster(Renderer poster)
    {
        this.poster = poster;
        hasCustomer = false;
    }

    public void SetTexture(WantedCustomer customer)
    {
        poster.material.mainTexture = customer.mugshot;
        poster.transform.parent.gameObject.SetActive(true);
        hasCustomer = true;
    }

    public void Disable()
    {
        poster.transform.parent.gameObject.SetActive(false);
        hasCustomer = false;
    }
}

public class CustomerSpawner : MonoBehaviour
{
    // possible refactor: create array of points and sort by distance to bar, spawn customer at lowest distance

    public GameObject[] m_customerPrefabs;
    public Transform m_spawnPoint;
    public List<Transform> m_servingPoints;
    public List<Transform> m_waitingPoints;
    public Transform m_coinDropPoint;

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
    private Transform m_clockHand;
    [SerializeField]
    private GameObject m_timeOverCanvas;
    [SerializeField]
    private TextMeshPro m_finalMoneyText;
    [SerializeField]
    private Renderer[] m_wantedPosterImages;
    [SerializeField]
    private Vector2Int m_randomRangeSpawnsBetweenBadSpawns;

    [HideInInspector]
    public List<GameObject> m_customers;
    [HideInInspector]
    public bool m_spawnCustomers = true;

    private List<WantedPoster> m_posters;
    private List<WantedCustomer> m_wantedCustomers;
    private float m_fCustomerSpawnTimer = 0f;
    private bool m_bHappyHour = false;
    private float m_fGameTimer = 0f;
    private int m_spawnsUntilNextBadCustomer;

    public AudioSource mainMenuMusic;
    public AudioSource calmHourMusic;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        m_customers = new List<GameObject>();
        m_posters = new List<WantedPoster>();
        m_wantedCustomers = new List<WantedCustomer>();
        m_spawnsUntilNextBadCustomer = Random.Range(m_randomRangeSpawnsBetweenBadSpawns.x, m_randomRangeSpawnsBetweenBadSpawns.y);
        StartCoroutine(SpawnWantedCustomersOnStart());
    }

    private IEnumerator SpawnWantedCustomersOnStart()
    {
        // create wanted customers
        for (int i = 0; i < m_wantedPosterImages.Length; ++i)
        {
            m_posters.Add(new WantedPoster(m_wantedPosterImages[i]));
            StartCoroutine(CreateWantedCustomer(m_posters[i]));
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Fade()
    {
        StartCoroutine(AudioController.FadeOut(mainMenuMusic, fadeTime));
        StartCoroutine(AudioController.FadeIn(calmHourMusic, fadeTime));
    }

    private IEnumerator CreateWantedCustomer(WantedPoster poster)
    {
        GameObject wantedCust = Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation);
        m_customers.Add(wantedCust);
        Customer cust = wantedCust.GetComponent<Customer>();

        // decide whether to wear hat or not
        if (Random.Range(0, m_hatPrefabs.Length + 1) > 0)
        {
            // pick hat
            cust.SetHat(m_hatPrefabs[Random.Range(0, m_hatPrefabs.Length)]);
        }

        yield return new WaitForEndOfFrame();

        // create image for customer
        RenderTexture.active = cust.m_mugshotCamera.targetTexture;
        Texture2D mugshot = new Texture2D(cust.m_mugshotCamera.targetTexture.width, cust.m_mugshotCamera.targetTexture.height);
        cust.m_mugshotCamera.Render();
        mugshot.ReadPixels(new Rect(0, 0, cust.m_mugshotCamera.targetTexture.width, cust.m_mugshotCamera.targetTexture.height), 0, 0);
        mugshot.Apply();

        WantedCustomer wc = new WantedCustomer(wantedCust, mugshot);
        m_wantedCustomers.Add(wc);
        poster.SetTexture(wc);
        wantedCust.SetActive(false);
    }

    private IEnumerator CustomerSpawnLoop()
    {
        SpawnCustomer();
        while (true)
        {
            if (m_bHappyHour)
                yield return new WaitForSeconds(Random.Range(m_randomRangeCustomerSpawnHappyHour.x, m_randomRangeCustomerSpawnHappyHour.y));
            else
                yield return new WaitForSeconds(Random.Range(m_randomRangeCustomerSpawnNotHappyHour.x, m_randomRangeCustomerSpawnNotHappyHour.y));

            // spawn customer when available
            yield return new WaitUntil(SpawnCustomer);
        }
    }

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

    private bool SpawnCustomer()
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
            destPoint = m_waitingPoints[m_waitingPoints.Count - 1];
            m_waitingPoints.RemoveAt(m_waitingPoints.Count - 1);
            bWait = true;
        }
        else // if there are free serving points
        {
            destPoint = m_servingPoints[m_servingPoints.Count - 1];
            m_servingPoints.RemoveAt(m_servingPoints.Count - 1);
        }

        // if it's time to spawn a bad customer
        if (m_spawnsUntilNextBadCustomer <= 0)
        {
            // get number of inactivate bad customers
            List<WantedCustomer> inactiveWantedCustomers = new List<WantedCustomer>();
            foreach (WantedCustomer wc in m_wantedCustomers)
            {
                if (!wc.customer.activeSelf)
                    inactiveWantedCustomers.Add(wc);
            }

            // spawn bad customer if there are any to spawn
            if (inactiveWantedCustomers.Count > 0)
            {
                GameObject customer = inactiveWantedCustomers[Random.Range(0, inactiveWantedCustomers.Count)].customer;
                customer.SetActive(true);
                Customer c = customer.GetComponent<Customer>();
                c.SetIsBad(true);
                c.SetDestination(destPoint, bWait);
                c.SetCoinDropPos(m_coinDropPoint.position);
                // set new wait until next bad customer spawns
                m_spawnsUntilNextBadCustomer = Random.Range(m_randomRangeSpawnsBetweenBadSpawns.x, m_randomRangeSpawnsBetweenBadSpawns.y);
            }
            else
            {
                // spawn normal customer
                m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation));

                Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();

                // decide whether to wear hat or not
                if (Random.Range(0, m_hatPrefabs.Length + 1) > 0)
                {
                    // pick hat
                    cust.SetHat(m_hatPrefabs[Random.Range(0, m_hatPrefabs.Length)]);
                }

                cust.SetDestination(destPoint, bWait);
                cust.SetCoinDropPos(m_coinDropPoint.position);
            }
        }
        else
        {
            // spawn customer
            m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation));

            Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();

            // decide whether to wear hat or not
            if (Random.Range(0, m_hatPrefabs.Length + 1) > 0)
            {
                // pick hat
                cust.SetHat(m_hatPrefabs[Random.Range(0, m_hatPrefabs.Length)]);
            }

            cust.SetDestination(destPoint, bWait);
            cust.SetCoinDropPos(m_coinDropPoint.position);

            // decrement spawn count until next bad customer
            --m_spawnsUntilNextBadCustomer;
        }

        return true;
    }

    /// <summary>
    /// Start the coroutines for the game loop
    /// </summary>
    public void StartSpawning()
    {
        StartCoroutine(CustomerSpawnLoop());
        StartCoroutine(HappyHourTimer());
        StartCoroutine(GameTimer());
        Destroy(m_startLever);
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitUntil(CountDown);

        // timer runs out
        m_timeOverCanvas.SetActive(true);
        m_finalMoneyText.text += FindObjectOfType<MoneyJar>().m_nCurrentMoney.ToString();

        yield return new WaitForSeconds(10f);

        SceneManager.LoadSceneAsync(0);
    }

    private bool CountDown()
    {
        m_fGameTimer += Time.deltaTime;
        // rotate clock hand
        m_clockHand.localRotation = Quaternion.Euler(m_clockHand.localRotation.x, m_clockHand.localRotation.y, (m_fGameTimer / m_gameTimeSeconds) * 360f);
        return m_fGameTimer >= m_gameTimeSeconds;
    }
}