using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    private void Start()
    {
        m_potionName = PotionName.CosyFire;
        if (!GetComponent<Potion>())
            Instantiate(FindObjectOfType<PotionAssets>().m_cosyFireParticles, transform);
    }
}