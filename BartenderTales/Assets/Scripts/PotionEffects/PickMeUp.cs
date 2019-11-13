using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeUp : PotionEffect
{
    [SerializeField]
    private float m_timeBeforeChangingDirection = 1f;
    [SerializeField]
    private float m_movementForce = 400f;
    [SerializeField]
    private float m_timeForEffectToBeActive = 10f;

    private Rigidbody m_rb;
    private Customer m_cust;
    private Vector3 m_v3ForceDirection = Vector3.up;
    private AudioSource m_audioSource;
    private Vector3 m_v3LastVelocity;
    private PotionAssets m_pa;
    private bool m_bDoEffect = false;

    void Start()
    {
        // set potion enum name
        m_potionName = PotionName.PickMeUp;

        // do effect if 
        if (m_cust = GetComponent<Customer>())
            StartCoroutine(EffectLoop());
    }

    private IEnumerator EffectLoop()
    {
        // ragdoll
        m_cust.SetRagdoll(true);

        m_pa = FindObjectOfType<PotionAssets>();
        m_audioSource = GetComponent<AudioSource>();

        foreach (GameObject trails in m_cust.GetComponent<Customer>().m_trailEffects)
        {
            trails.SetActive(true);
        }
        m_rb = m_cust.m_pickMeUpRigidbody;

        // stop loop after set amount of seconds
        Invoke("StopLoop", m_timeForEffectToBeActive);

        // enable effect
        m_bDoEffect = true;
        m_v3LastVelocity = m_rb.velocity;

        // loop
        while (m_bDoEffect)
        {
            yield return new WaitForSeconds(m_timeBeforeChangingDirection);

            // flip force direction
            m_v3ForceDirection *= -1;
        }
    }

    private void FixedUpdate()
    {
        // only update if doing effect
        if (!m_bDoEffect)
            return;

        // add force
        m_rb.velocity += (m_v3ForceDirection * m_movementForce * Time.fixedDeltaTime);

        if (Vector3.Distance(m_rb.velocity, m_v3LastVelocity) > 5)
            m_audioSource.PlayOneShot(m_pa.m_bonesBreakingAudioClips[Random.Range(0, m_pa.m_bonesBreakingAudioClips.Length)]);

        m_v3LastVelocity = m_rb.velocity;
    }

    private void StopLoop()
    {
        m_bDoEffect = false;
        StartCoroutine(m_cust.Dissolve());
    }
}