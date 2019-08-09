using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimator : CustomerAnimator
{
    [SerializeField]
    private Vector2 m_minMaxSecondsForRandomIdleAnimation = new Vector2(4f, 8f);
    [SerializeField]
    private string[] m_randomIdleAnimationTriggerNames;

    override public IEnumerator IdleLoop()
    {
        // while idle
        while (m_animator.GetBool("StoppedMoving"))
        {
            // wait for random time
            yield return new WaitForSeconds(Random.Range(m_minMaxSecondsForRandomIdleAnimation.x, m_minMaxSecondsForRandomIdleAnimation.y));
            // then play random idle animation
            m_animator.SetTrigger(m_randomIdleAnimationTriggerNames[Random.Range(0, m_randomIdleAnimationTriggerNames.Length)]);
        }
    }
}