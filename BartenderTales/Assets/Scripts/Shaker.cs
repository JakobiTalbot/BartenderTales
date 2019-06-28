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

public enum PotionName
{
    FantasticTeleport = 0,
    QuickEnd,
    Mundane,
    AssassinsSpecial,
    SmokeyTeleport,
    PixieDust,
    Beer,
    CosyFire,
    Count
}

public class Shaker : MonoBehaviour
{
    public float m_shakeTime = 0.2f;
    public float m_radiansShakeThreshold = 2f;
    public float m_velocityShakeThreshold = 1.5f;

    [HideInInspector]
    public List<Potion> m_potions;

    private ParticleSystem m_particleSystem;
    private List<Ingredient> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_contents = new List<Ingredient>();
        m_potions = new List<Potion>();
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
                    CreatePotion();
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
        //// fantastic teleport
        //m_combinations.Add(new Combination(Ingredient.PixiePear, Ingredient.ElvenMagicBox));
        //// quick end
        //m_combinations.Add(new Combination(Ingredient.WhiskeyWatermelon, Ingredient.ElvenMagicBox));
        //// assassin's special
        //m_combinations.Add(new Combination(Ingredient.EmuElderberry, Ingredient.ElvenMagicBox));
        //// smokey teleport
        //m_combinations.Add(new Combination(Ingredient.PixiePear, Ingredient.DragonChilli));
        //// pixie dust
        //m_combinations.Add(new Combination(Ingredient.WhiskeyWatermelon, Ingredient.UnicornFeathers));
        // cosy fire
        m_potions.Add(new Potion(Ingredient.EmuElderberry, Ingredient.DragonChilli, new CosyFire(), PotionName.CosyFire));
        // belch fire
        //m_combinations.Add(new Combination(Ingredient.WhiskeyWatermelon, Ingredient.DragonChilli));
        //// honest policy
        //m_combinations.Add(new Combination(Ingredient.PixiePear, Ingredient.UnicornFeathers));
    }

    private Potion CreatePotion()
    {
        // if potion doesn't contain valid amount of ingredients
        if (m_contents.Count != 2)
            return null;

        // loop through each possible combination
        foreach (Potion potion in m_potions)
        {
            // if combination is valid
            if (potion.m_ingredients.Contains(m_contents[0])
                && potion.m_ingredients.Contains(m_contents[1]))
            {
                return potion;
            }
        }

        // potion doesn't match any combinations
        // TODO: return mundane potion
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        // ingredient tag should be ingredient name immediately followed by "-Ingredient"
        if (tag.Contains("-Ingredient"))
        {
            m_contents.Add((Ingredient)System.Enum.Parse(typeof(Ingredient), tag.Replace("-Ingredient", ""), true));
            Destroy(collision.gameObject);
        }
    }
}