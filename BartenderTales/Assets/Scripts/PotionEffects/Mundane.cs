using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundane : PotionEffect
{
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.Mundane;
        GetComponent<Customer>()?.ExitBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}