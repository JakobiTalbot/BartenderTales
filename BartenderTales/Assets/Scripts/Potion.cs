using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private PotionEffect m_potionEffect;
    [HideInInspector]
    public List<Ingredient> m_ingredients;
    [HideInInspector]
    public PotionName m_potionName;

    public Potion(Ingredient a, Ingredient b, PotionEffect potionEffect, PotionName potionName)
    {
        m_ingredients.Add(a);
        m_ingredients.Add(b);
        m_potionEffect = potionEffect;
        m_potionName = potionName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}