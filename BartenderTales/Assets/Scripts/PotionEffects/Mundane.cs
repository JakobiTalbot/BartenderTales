using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundane : PotionEffect
{
    [SerializeField]
    private float m_timeToWaitBeforeExploding = 2f;
    [SerializeField]
    private float m_reactionRadius = 5f;
    [SerializeField]
    private int m_reputationOnGoodCustomerExplosion = -1;
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.Mundane;
        if (GetComponent<Customer>())
        {
            Invoke("Explode", m_timeToWaitBeforeExploding);
        }
    }

    private void Explode()
    {
        // get all customers within range
        Collider[] affectedCustomers = Physics.OverlapSphere(transform.position, m_reactionRadius);

        // cheer if evil customer exploded
        if (GetComponent<Customer>().IsEvil())
        {
            foreach (Collider c in affectedCustomers)
            {
                c.GetComponent<Customer>()?.Cheer();
            }
            FindObjectOfType<ReputationManager>().AddToReputation(-m_reputationOnGoodCustomerExplosion);
        }
        // be shocked if good customer exploded
        else
        {
            foreach (Collider c in affectedCustomers)
            {
                c.GetComponent<Customer>()?.Shocked();
            }
            FindObjectOfType<ReputationManager>().AddToReputation(m_reputationOnGoodCustomerExplosion);
        }

        Destroy(Instantiate(FindObjectOfType<PotionAssets>().m_mundaneExplosionPrefab, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
    }
}