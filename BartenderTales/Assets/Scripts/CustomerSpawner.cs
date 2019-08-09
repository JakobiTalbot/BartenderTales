using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] m_customerPrefabs;
    public Transform m_spawnPoint;
    public List<Transform> m_servingPoints;
    public List<Transform> m_waitingPoints;
    public float m_timeBetweenCustomers = 60f;
    public Transform m_coinDropPoint;

    [HideInInspector]
    public List<GameObject> m_customers;
    [HideInInspector]
    public bool m_spawnCustomers = true;
    private float m_fCustomerSpawnTimer = 0f;


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

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_spawnCustomers)
            return;
        // increment timer
        m_fCustomerSpawnTimer += Time.deltaTime;

        // if time to spawn customer and spawn points available
        while (m_fCustomerSpawnTimer >= m_timeBetweenCustomers
            && (m_servingPoints.Count > 0 || m_waitingPoints.Count > 0))
        {
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

            m_fCustomerSpawnTimer -= m_timeBetweenCustomers;
            // spawn customer
            m_customers.Add(Instantiate(m_customerPrefabs[Random.Range(0, m_customerPrefabs.Length)], m_spawnPoint.position, m_spawnPoint.rotation));
            Customer cust = m_customers[m_customers.Count - 1].GetComponent<Customer>();
            cust.SetDestination(destPoint, bWait);
            cust.SetCoinDropPos(m_coinDropPoint.position);
        }
    }
}
