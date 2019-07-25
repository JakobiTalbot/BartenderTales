using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickEnd : PotionEffect
{
    public float m_timeToWaitBeforeExploding = 2f;
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
        Destroy(Instantiate(FindObjectOfType<PotionAssets>().m_quickEndExplosionPrefab, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
        // TODO: reactions from other customers
    }
}