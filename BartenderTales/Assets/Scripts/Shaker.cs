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
    Ingredient[] m_ingredients;
}

public class Shaker : MonoBehaviour
{
    public float m_shakeTime = 0.2f;
    public float m_radiansShakeThreshold = 2f;
    public float m_velocityShakeThreshold = 1.5f;

    private ParticleSystem m_particleSystem;
    private List<Ingredient> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_contents = new List<Ingredient>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastVelocity = m_rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_contents.Count >= 0)
        {
            // if player is shaking drink
            if (/*m_rb.angularVelocity.magnitude > m_radiansShakeThreshold
                || */(m_rb.velocity - m_v3LastVelocity).magnitude > m_velocityShakeThreshold)
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
}