using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredient
{
    WhiskeyWatermelon = 0,
    EmuElderberry,
    PixiePear,
    ElvenMagicBox,
    DragonChilli,
    UnicornFeathers,
    Count
}

public struct Combination
{
    public List<Ingredient> m_ingredients;
    public PotionEffect m_potionEffect;

    public Combination(Ingredient a, Ingredient b, PotionEffect potionEffect)
    {
        m_ingredients = new List<Ingredient>();
        m_ingredients.Add(a);
        m_ingredients.Add(b);
        m_potionEffect = potionEffect;
    }
}

public class Shaker : MonoBehaviour
{
    public float m_shakeTime = 0.2f;
    public float m_radiansShakeThreshold = 2f;
    public float m_velocityShakeThreshold = 1.5f;

    private ParticleSystem m_particleSystem;
    private List<Combination> m_combinations;
    private List<Ingredient> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_contents = new List<Ingredient>();
        m_combinations = new List<Combination>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastVelocity = m_rb.velocity;
        CreateCombinations();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_contents.Count >= 2)
        {
            // if player is shaking drink
            if (m_rb.angularVelocity.magnitude > m_radiansShakeThreshold
                || (m_rb.velocity - m_v3LastVelocity).magnitude > m_velocityShakeThreshold)
            {
                m_fCurrentShakeTime += Time.deltaTime;
                Debug.Log("shaking...");
                // if drink shaken
                if (m_fCurrentShakeTime > m_shakeTime)
                {
                    Debug.Log("done");
                }
            }
        }
        m_v3LastVelocity = m_rb.velocity;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        m_contents.Add(ingredient);
    }

    private void CreateCombinations()
    {
        //// quick end
        //m_combinations.Add(new Combination(Ingredient.WhiskeyWatermelon, Ingredient.ElvenMagicBox));
        //// assassin's special
        //m_combinations.Add(new Combination(Ingredient.EmuElderberry, Ingredient.ElvenMagicBox));
        //// smokey teleport
        //m_combinations.Add(new Combination(Ingredient.PixiePear, Ingredient.DragonChilli));
        //// pixie dust
        //m_combinations.Add(new Combination(Ingredient.WhiskeyWatermelon, Ingredient.UnicornFeathers));
        //// cosy fire
        //m_combinations.Add(new Combination(Ingredient.EmuElderberry, Ingredient.DragonChilli));
        //// honest policy
        //m_combinations.Add(new Combination(Ingredient.PixiePear, Ingredient.UnicornFeathers));
    }

    private void CheckIngredients()
    {
        if (m_contents.Count != 2)
            return;

        // loop through each possible combination
        foreach (Combination combo in m_combinations)
        {
            // if combination matches
            if (combo.m_ingredients.Contains(m_contents[0])
                && combo.m_ingredients.Contains(m_contents[1]))
            {
                // create bottles with potion effect
            }
        }
    }
}