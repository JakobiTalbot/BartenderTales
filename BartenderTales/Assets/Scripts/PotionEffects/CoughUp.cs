using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoughUp : PotionEffect
{
    [SerializeField]
    private float m_timeIntoAnimationToSpawnObject = 1.7f;

    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.CoughUp;

        // activate dragon breath if on customer
        if (GetComponent<Customer>())
            DoCoughUp();
    }

    private void DoCoughUp()
    {
        Customer c = GetComponent<Customer>();
        PotionAssets assets = FindObjectOfType<PotionAssets>();

        // play coughup animation
        StartCoroutine(c.CoughUp(assets.m_coughUpRandomObjectPrefabs[Random.Range(0, assets.m_coughUpRandomObjectPrefabs.Length)], m_timeIntoAnimationToSpawnObject));
    }
}