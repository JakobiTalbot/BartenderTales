﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionName
{
    FantasticTeleport = 0,
    QuickEnd,
    AssassinsSpecial,
    SmokeyTeleport,
    PixieDust,
    Beer,
    CosyFire,
    BelchFire,
    HonestPolicy,
    Count,
    Mundane
}

public class Shaker : MonoBehaviour
{
    public float m_shakeTime = 0.2f;
    public float m_accelShakeThreshold = 1f;
    public List<Transform> m_potionSpawnPoints;
    public GameObject[] m_potionPrefabs;
    public int m_potionsToSpawn = 3;

    [HideInInspector]
    public Dictionary<PotionName, PotionEffect> m_potionFunc;

    private ParticleSystem m_particleSystem;
    private List<IngredientType> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastPos;
    private Vector3 m_v3LastDeltaPos;

    // Start is called before the first frame update
    void Start()
    {
        m_contents = new List<IngredientType>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastPos = transform.position;
        CreatePotions();
    }

    private void CreatePotions()
    {
        m_potionFunc = new Dictionary<PotionName, PotionEffect>();
        m_potionFunc.Add(PotionName.CosyFire, m_potionPrefabs[0].GetComponent<CosyFire>());
        m_potionFunc.Add(PotionName.QuickEnd, m_potionPrefabs[1].GetComponent<QuickEnd>());
        m_potionFunc.Add(PotionName.PixieDust, m_potionPrefabs[2].GetComponent<PixieDust>());
    }

    // Update is called once per frame
    void Update()
    {

        if (m_contents.Count >= 2)
        {
            // if player is shaking drink
            if (((transform.position - m_v3LastPos) - m_v3LastDeltaPos).magnitude > m_accelShakeThreshold)
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

        m_v3LastDeltaPos = (transform.position - m_v3LastPos);
        m_v3LastPos = transform.position;
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

            // dirty way to fix errors from object being destroyed when still on hand
            collision.transform.parent = null;
            collision.gameObject.SetActive(false);

            Destroy(collision.gameObject);
        }
    }
}