using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Transform> m_potionSpawnPoints;
    public GameObject[] m_potionPrefabs;
    public int m_potionsToSpawn = 3;

    private ParticleSystem m_particleSystem;
    private List<IngredientType> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_contents = new List<IngredientType>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastVelocity = m_rb.velocity;
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
                Debug.Log(m_fCurrentShakeTime);
                // if drink shaken
                if (m_fCurrentShakeTime > m_shakeTime)
                {
                    Debug.Log("done");
                    GameObject potion = GetPotion();
                    int nPotionsToSpawn = m_potionsToSpawn;

                    // store taken points beforehand in case not enough points to spawn potions on
                    List<Transform> takenPoints = m_potionSpawnPoints;
                    foreach(Transform t in takenPoints)
                    {
                        if (t.GetComponent<PotionPoint>().m_inhabitant)
                            takenPoints.Remove(t);
                    }

                    // create potions at spawn points
                    for (int i = 0; i < m_potionsToSpawn; ++i)
                    {
                        // if spawn point is empty
                        if (!m_potionSpawnPoints[i].GetComponent<PotionPoint>().m_inhabitant)
                        {
                            m_potionSpawnPoints[i].GetComponent<PotionPoint>().SetInhabitant(potion);
                            nPotionsToSpawn--;

                            if (nPotionsToSpawn == 0)
                                break;
                        }
                    }

                    // still have potions to spawn, spawn them in pre-inhabited spots
                    while (nPotionsToSpawn > 0)
                    {
                        int iRand = Random.Range(0, takenPoints.Count - 1);
                        m_potionSpawnPoints[iRand].GetComponent<PotionPoint>().SetInhabitant(potion);
                        nPotionsToSpawn--;
                    }

                    m_fCurrentShakeTime = 0f;
                    m_contents.Clear();
                }
            }
        }
        m_v3LastVelocity = m_rb.velocity;
    }

    public void AddIngredient(IngredientType ingredient)
    {
        m_contents.Add(ingredient);
    }

    private GameObject GetPotion()
    {
        // if potion doesn't contain valid amount of ingredients
        if (m_contents.Count != 2)
            return null;

        // loop through each possible combination
        foreach (GameObject potion in m_potionPrefabs)
        {
            Potion p = potion.GetComponent<Potion>();
            // if combination is valid
            if (p.m_ingredients.Contains(m_contents[0])
                && p.m_ingredients.Contains(m_contents[1]))
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
        if (collision.gameObject.GetComponent<Ingredient>())
        {
            // TODO: feedback for putting ingredient in potion
            m_contents.Add(collision.gameObject.GetComponent<Ingredient>().m_ingredientType);
            Destroy(collision.gameObject);
        }
    }
}