using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    public GameObject m_fireParticlesPrefab;

    [HideInInspector]
    public bool m_bFireMade = false;
    private void Start()
    {
        m_potionName = PotionName.CosyFire;
    }

    private void Update()
    {
        // do fire if not on potion
        if (!GetComponent<Potion>()
            && m_bFireMade)
        {
            Instantiate(m_fireParticlesPrefab, transform);
            m_bFireMade = true;
        }
    }
}