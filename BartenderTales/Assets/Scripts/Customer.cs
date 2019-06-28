using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public float m_rotationSpeed = 0.02f;

    private Potion m_order;
    private NavMeshAgent m_agent;
    private Transform m_point;
    private bool m_bBadPerson = false;
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        Shaker s = FindObjectOfType<Shaker>();
        m_order = s.m_potions[Random.Range(0, s.m_potions.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_agent.remainingDistance < 0.6f)
        {
            // face player
            Vector3 v3Pos = Camera.main.transform.position;
            v3Pos.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3Pos - transform.position), m_rotationSpeed);
            // stop moving
            m_agent.isStopped = true;
        }
    }

    public void SetDestination(Transform dest)
    {
        if (!m_agent)
            m_agent = GetComponent<NavMeshAgent>();
        m_point = dest;
        m_agent.SetDestination(dest.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Potion"))
        {
            Potion p = collision.collider.GetComponent<Potion>();
            // if correct potion given
            if (m_order.m_potionName == p.m_potionName)
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
        }
    }
}