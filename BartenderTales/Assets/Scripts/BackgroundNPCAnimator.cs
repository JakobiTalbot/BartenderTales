using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPCAnimator : MonoBehaviour
{
    [SerializeField]
    private string[] m_randomTriggerNames;
    [SerializeField]
    private Vector2 m_randomRangeBetweenTriggering;

    private Animator m_animator;

    void Start()
    {
        // get reference to animator
        m_animator = GetComponent<Animator>();
        // start loop
        StartCoroutine(IdleLoop());
    }

    private IEnumerator IdleLoop()
    {
        while (true)
        {
            // wait for random seconds before triggering
            yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenTriggering.x, m_randomRangeBetweenTriggering.y));

            // trigger random animation
            m_animator.SetTrigger(m_randomTriggerNames[Random.Range(0, m_randomTriggerNames.Length)]);
        }
    }
}