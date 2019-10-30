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
        PotionAssets pa = FindObjectOfType<PotionAssets>();
        // enable ragdoll
        m_cust.SetRagdoll(true);
        // start dissolve before enabling particles to prevent particle colour changing
        StartCoroutine(m_cust.Dissolve());
        // instantiate particles childed to central rigidbody of customer
        GameObject p = Instantiate(pa.m_cupidsKissParticlePrefab, m_cust.m_pickMeUpRigidbody.transform);
        p.transform.SetPositionAndRotation(m_cust.m_pickMeUpRigidbody.transform.position, Quaternion.LookRotation(-m_cust.m_pickMeUpRigidbody.transform.right));
        // play audio clip
        GetComponent<AudioSource>().PlayOneShot(pa.m_cupidsKissActivationAudio);
        Destroy(p, 5f);
    }
}