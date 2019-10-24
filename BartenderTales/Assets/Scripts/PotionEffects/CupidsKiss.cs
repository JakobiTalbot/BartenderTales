using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupidsKiss : PotionEffect
{
    private Customer m_cust;
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.CupidsKiss;

        if (m_cust = GetComponent<Customer>())
            ActivateEffect();
    }

    private void ActivateEffect()
    {
        // enable ragdoll
        m_cust.SetRagdoll(true);
        // particles
        GameObject p = Instantiate(FindObjectOfType<PotionAssets>().m_cupidsKissParticlePrefab, transform);
        p.transform.position = transform.position;
        p.transform.rotation = transform.rotation;
        Destroy(p, 5f);

        StartCoroutine(m_cust.Dissolve());
    }
}