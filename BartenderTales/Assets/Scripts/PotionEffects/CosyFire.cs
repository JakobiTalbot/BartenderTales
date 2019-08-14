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
            c.m_animator.SetTrigger("CosyFireReaction");
            c.m_animator.SetBool("StoppedMoving", false);
            Instantiate(FindObjectOfType<PotionAssets>().m_cosyFireParticles, transform);
            c.Invoke("ExitBar", m_secondsToWaitBeforeLeavingBar);
        }
    }
}