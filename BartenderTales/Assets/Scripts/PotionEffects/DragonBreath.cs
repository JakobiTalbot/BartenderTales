using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreath : PotionEffect
{
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.DragonBreath;

        // activate dragon breath if on customer
        if (GetComponent<Customer>())
            ActivateDragonBreath();
    }

    private void ActivateDragonBreath()
    {
        Customer c = GetComponent<Customer>();
        // create dragon breath particles at designated point
        Instantiate(FindObjectOfType<PotionAssets>().m_dragonBreathParticlePrefab, c.GetDragonBreathPoint());
    }
}