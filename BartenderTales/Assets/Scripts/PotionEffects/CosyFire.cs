using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    [SerializeField]
    private float m_secondsToWaitBeforeLeavingBar = 3.2f;
    private void Start()
    {
        m_potionName = PotionName.CosyFire;
        Customer c = GetComponent<Customer>();
        if (c)
        {
            c.m_Animator.SetTrigger("CosyFireReaction");
            c.m_Animator.SetBool("StoppedMoving", false);
            Instantiate(FindObjectOfType<PotionAssets>().m_cosyFireParticles, transform);
            c.Invoke("ExitBar", m_secondsToWaitBeforeLeavingBar);
        }
    }
}