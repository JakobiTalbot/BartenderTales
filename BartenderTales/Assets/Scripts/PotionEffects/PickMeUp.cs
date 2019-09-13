using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeUp : PotionEffect
{
    [SerializeField]
    private float m_timeBeforeChangingDirection = 1f;
    [SerializeField]
    private float m_movementForce = 30000f;
    [SerializeField]
    private float m_timeForEffectToBeActive = 10f;

    private Rigidbody m_rb;
    private Customer m_cust;
    private Vector3 m_v3ForceDirection = Vector3.up;
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

        m_rb = m_cust.m_pickMeUpRigidbody;

        // stop loop after set amount of seconds
        Invoke("StopLoop", m_timeForEffectToBeActive);

        // enable effect
        m_bDoEffect = true;

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
        m_rb.AddForce(m_v3ForceDirection * m_movementForce * Time.fixedDeltaTime);
    }

    private void StopLoop()
    {
        m_bDoEffect = false;
    }
}