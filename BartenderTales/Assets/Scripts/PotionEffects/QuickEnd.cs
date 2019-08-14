using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickEnd : PotionEffect
{
    [SerializeField]
    private float m_timeToWaitBeforeExploding = 2f;
    [SerializeField]
    private float m_emoteRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.QuickEnd;
        if (GetComponent<Customer>())
        {
            Invoke("Explode", m_timeToWaitBeforeExploding);
        }
    }

    private void Explode()
    {
        Collider[] affectedCustomers = Physics.OverlapSphere(transform.position, m_emoteRadius);
        foreach (Collider c in affectedCustomers)
        {
            c.GetComponent<Customer>()?.Shocked();
        }

        Destroy(Instantiate(FindObjectOfType<PotionAssets>().m_quickEndExplosionPrefab, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
        // TODO: reactions from other customers
    }
}