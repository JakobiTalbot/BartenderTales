using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    private void Start()
    {
        m_potionName = PotionName.CosyFire;
        Customer c = GetComponent<Customer>();
        if (c)
        {
            gameObject.GetComponent<Customer>().m_Animator.SetBool("HasDrunkCosyFire", true);
            Instantiate(FindObjectOfType<PotionAssets>().m_cosyFireParticles, transform);
            c.ExitBar();
        }
    }
}