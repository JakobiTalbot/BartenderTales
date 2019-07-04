using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public float m_rotationSpeed = 0.02f;
    public float m_timeUntilExitingBarAfterDrinking = 2f;

    private PotionName m_order;
    private CustomerSpawner m_spawner;
    private NavMeshAgent m_agent;
    private Transform m_point;
    private bool m_bWaiting = true;
    private bool m_bBadPerson = false;
    private bool m_bExitingBar = false;
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        // order random potion
        // TODO: only order good potions
        m_order = (PotionName)Random.Range(0, (int)PotionName.Count);
        m_spawner = FindObjectOfType<CustomerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_agent.remainingDistance < 0.6f)
        {
            if (m_bExitingBar)
            {
                Destroy(gameObject);
                return;
            }
            // face player
            Vector3 v3Pos = Camera.main.transform.position;
            v3Pos.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3Pos - transform.position), m_rotationSpeed);
            // stop moving
            m_agent.isStopped = true;
            if (m_bWaiting && m_spawner.m_servingPoints.Count > 0)
            {
                SetDestination(m_spawner.m_servingPoints[Random.Range(0, m_spawner.m_servingPoints.Count)]);
                m_bWaiting = false;
            }
        }
    }

    public void SetDestination(Transform dest)
    {
        if (!m_agent)
            m_agent = GetComponent<NavMeshAgent>();
        m_agent.isStopped = false;
        m_point = dest;
        m_agent.SetDestination(dest.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Potion>())
        {
            PotionEffect p = collision.gameObject.GetComponent<PotionEffect>();
            // TODO: reactions to potions
            // if correct potion given
            if (m_order == p.m_potionName)
            {
                // happy reaction
            }
            else // if wrong potion given
            {
                if (m_bBadPerson)
                {

                }
                else
                {
                    // sad reaction
                }
            }
            System.Type type = FindObjectOfType<Shaker>().m_potionFunc[p.m_potionName].GetType();
            // drink potion
            gameObject.AddComponent(type);
            Destroy(collision.gameObject);
            Invoke("ExitBar", m_timeUntilExitingBarAfterDrinking);
        }
    }

    private void ExitBar()
    {
        m_bExitingBar = true;
        m_agent.isStopped = false;
        m_point = FindObjectOfType<CustomerSpawner>().m_spawnPoint;
        m_agent.SetDestination(m_point.position);
    }
}