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
        // wait for random time before first animation
        yield return new WaitForSeconds(Random.Range(m_minMaxSecondsForRandomIdleAnimation.x, m_minMaxSecondsForRandomIdleAnimation.y));
        // while idle
        while (m_animator.GetBool("StoppedMoving"))
        {
            // play random idle animation
            m_animator.SetTrigger(m_randomIdleAnimationTriggerNames[Random.Range(0, m_randomIdleAnimationTriggerNames.Length)]);
            // then wait for random time until next random animation
            yield return new WaitForSeconds(Random.Range(m_minMaxSecondsForRandomIdleAnimation.x, m_minMaxSecondsForRandomIdleAnimation.y));
        }
    }
}