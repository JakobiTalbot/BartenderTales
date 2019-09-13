using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmokeyTeleport : PotionEffect
{
    private NavMeshAgent m_navAgent;
    private Customer m_customer;
    private bool m_bDoTeleport = true;

    void Start()
    {
        // set potion name
        m_potionName = PotionName.SmokeyTeleport;

        // activate teleport loop if on customer
        if (m_customer = GetComponent<Customer>())
            StartCoroutine(TeleportLoop());
    }

    private IEnumerator TeleportLoop()
    {
        PotionAssets assets = FindObjectOfType<PotionAssets>();
        // get references
        m_navAgent = GetComponent<NavMeshAgent>();

        // break out of loop after random time
        Invoke("StopTeleporting", Random.Range(assets.m_randomTimeToDoTeleporting.x, assets.m_randomTimeToDoTeleporting.y));

        // go idle
        m_customer.m_animator.SetBool("StoppedMoving", true);
        
        // stop navigating
        m_navAgent.isStopped = true;
        m_customer.m_speechBubbleCanvas.SetActive(false);

        // teleport loop
        while (m_bDoTeleport)
        {
            // wait until next teleport
            yield return new WaitForSeconds(Random.Range(assets.m_randomRangeBetweenTeleports.x, assets.m_randomRangeBetweenTeleports.y));

            // get random point in sphere
            Vector3 tpPoint = Random.insideUnitSphere * Random.Range(0f, assets.m_teleportSphereRadius);
            NavMeshHit hit;

            // if point on nav mesh found
            if (NavMesh.SamplePosition(tpPoint, out hit, assets.m_teleportSphereRadius, NavMesh.AllAreas))
            {
                // create particles
                Destroy(Instantiate(assets.m_smokeyTeleportParticlePrefab, transform.position, Quaternion.identity), 5f);
                // teleport
                transform.position = hit.position;
            }
        }

        // make customer disappear
        m_customer.End();
    }

    /// <summary>
    /// Causes the teleporting coroutine to stop running
    /// </summary>
    private void StopTeleporting()
    {
        m_bDoTeleport = false;
    }
}