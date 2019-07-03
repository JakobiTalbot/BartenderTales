using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    public GameObject m_fireParticlesPrefab;

    public override void ActivateEffect(GameObject target)
    {
        // create fire particles
        GameObject fire = Instantiate(m_fireParticlesPrefab, target.transform);
    }
}
