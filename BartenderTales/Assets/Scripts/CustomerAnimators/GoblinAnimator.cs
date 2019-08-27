﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimator : CustomerAnimator
{
    [SerializeField]
    private Vector2 m_minMaxSecondsForRandomIdleAnimation = new Vector2(4f, 8f);\
    [SerializeField]
    private int m_idleAnimationsCount = 3;
    [SerializeField]
    private int m_shockedReactionTriggerCount = 3;

    override public IEnumerator IdleLoop()
    {
        // wait for random time before first animation
        yield return new WaitForSeconds(Random.Range(m_minMaxSecondsForRandomIdleAnimation.x, m_minMaxSecondsForRandomIdleAnimation.y));
        // while idle
        while (m_animator.GetBool("StoppedMoving"))
        {
            string anim = "IdleTrigger" + index.ToString();
            // play random idle animation
            m_animator.SetTrigger(anim);
            // then wait for random time until next random animation
            yield return new WaitForSeconds(Random.Range(m_minMaxSecondsForRandomIdleAnimation.x, m_minMaxSecondsForRandomIdleAnimation.y));
            m_animator.ResetTrigger(anim);
        }
    }

    public override void Shocked()
    {
        m_animator.SetTrigger("Shocked" + Random.Range(1, m_shockedReactionTriggerCount + 1));
    }

    public override void Cheer()
    {
        m_animator.SetTrigger("Cheer");
    }
}