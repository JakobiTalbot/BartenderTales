using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    // possible refactor: create array of points and sort by distance to bar, spawn customer at lowest distance

    public GameObject[] m_customerPrefabs;
    public Transform m_spawnPoint;
    public List<Transform> m_servingPoints;
    public List<Transform> m_waitingPoints;
    public Transform m_coinDropPoint;

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

    [HideInInspector]
    public List<GameObject> m_customers;
    [HideInInspector]
    public bool m_spawnCustomers = true;
    private float m_fCustomerSpawnTimer = 0f;
    private bool m_bHappyHour = false;

    // Start is called before the first frame update
    void Start()
    {
        m_customers = new List<GameObject>();
        // spawn customer at start
        m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation));
        int i = Random.Range(0, m_servingPoints.Count);
        Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();
        cust.SetDestination(m_servingPoints[i], false);
        cust.SetCoinDropPos(m_coinDropPoint.position);
        m_servingPoints.RemoveAt(i);
        StartCoroutine(CustomerSpawnLoop());
    }

    private IEnumerator CustomerSpawnLoop()
    {
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

        // spawn customer
        m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation));
        Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();
        cust.SetDestination(destPoint, bWait);
        cust.SetCoinDropPos(m_coinDropPoint.position);

        return true;
    }
}