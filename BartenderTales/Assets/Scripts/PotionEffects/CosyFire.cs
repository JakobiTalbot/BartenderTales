using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosyFire : PotionEffect
{
    public GameObject m_fireParticlesPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateEffect()
    {
        // create fire particles
        GameObject fire = Instantiate(m_fireParticlesPrefab, transform);
    }
}
