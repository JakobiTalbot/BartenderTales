using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPCAnimator : MonoBehaviour
{
    [SerializeField]
    private int m_idleStatesCount;

    private Animator m_animator;

    void Start()
    {
        // get reference to animator
        m_animator = GetComponent<Animator>();

        // random animation
        int i = Random.Range(0, m_idleStatesCount + 1);
        if (i < m_idleStatesCount)
            m_animator.SetBool("Idle" + i, true);
    }
}