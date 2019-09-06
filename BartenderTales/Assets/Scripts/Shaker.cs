﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public enum PotionName
{
    //Skydive = 0,
    QuickEnd,
    //AssassinsSpecial,
    //SmokeyTeleport,
    PixieDust,
    //Beer,
    CosyFire,
    CoughUp,
    //HonestPolicy,
    Count,
    Mundane
}

public class Shaker : MonoBehaviour
{
    public float m_shakeTime = 0.2f;
    public float m_accelShakeThreshold = 1f;
    public List<Transform> m_potionSpawnPoints;
    public GameObject[] m_potionPrefabs;
    public GameObject m_mundanePotion;
    public Transform m_capPlacedTransform;
    public int m_potionsToSpawn = 3;

    [SerializeField]
    private AudioClip[] m_audioClipsOnPotionCreation;

    [HideInInspector]
    public Dictionary<PotionName, PotionEffect> m_potionFunc;

    private ParticleSystem m_particleSystem;
    private GameObject m_cap;
    private List<IngredientType> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastPos;
    private Vector3 m_v3LastDeltaPos;
    private bool m_bCapOn = false;
    private Collider m_collider;
    private IngredientManager m_manager;
    private Transform m_startTransform;
    private AudioSource m_audioSource;

    public AudioSource ingredientsInSound;

    // Start is called before the first frame update
    void Start()
    {
        // get default transform
        m_startTransform = new GameObject().transform;
        m_startTransform.position = transform.position;
        m_startTransform.rotation = transform.rotation;

        m_manager = FindObjectOfType<IngredientManager>();
        m_audioSource = GetComponent<AudioSource>();
        m_collider = GetComponent<Collider>();
        m_contents = new List<IngredientType>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastPos = transform.position;
        m_cap = FindObjectOfType<ShakerCap>().gameObject;
        CreatePotions();
    }

    private void CreatePotions()
    {
        m_potionFunc = new Dictionary<PotionName, PotionEffect>();
        m_potionFunc.Add(PotionName.CosyFire, m_potionPrefabs[0].GetComponent<CosyFire>());
        m_potionFunc.Add(PotionName.PixieDust, m_potionPrefabs[1].GetComponent<PixieDust>());
        m_potionFunc.Add(PotionName.CoughUp, m_potionPrefabs[2].GetComponent<CoughUp>());
    }

    // Update is called once per frame
    void Update()
    {
        // if contains enough contents and the cap is on
        if (m_contents.Count >= 2
            && m_bCapOn)
        {
            // if player is shaking drink
            if (((transform.position - m_v3LastPos) - m_v3LastDeltaPos).magnitude > m_accelShakeThreshold)
            {
                m_fCurrentShakeTime += Time.deltaTime;

                // if drink shaken
                if (m_fCurrentShakeTime > m_shakeTime)
                {
                    // find potion type
                    GameObject potion = GetPotion();
                    // play audio
                    m_audioSource?.PlayOneShot(m_audioClipsOnPotionCreation[Random.Range(0, m_audioClipsOnPotionCreation.Length)]);

                    // get number of potions to create
                    int nPotionsToSpawn;
                    if (potion.GetComponent<Mundane>())
                        nPotionsToSpawn = 1;
                    else
                        nPotionsToSpawn = m_potionsToSpawn;

                    // store taken points beforehand in case not enough points to spawn potions on
                    List<Transform> takenPoints = m_potionSpawnPoints;
                    for (int i = takenPoints.Count - 1; i >= 0; --i)
                    {
                        if (takenPoints[i].GetComponent<PotionPoint>().m_inhabitant)
                            takenPoints.RemoveAt(i);
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

                    // reset stuff
                    m_fCurrentShakeTime = 0f;
                    m_contents.Clear();
                }
            }
        }

        m_v3LastDeltaPos = (transform.position - m_v3LastPos);
        m_v3LastPos = transform.position;
    }

    /// <summary>
    /// Add an ingredient to the shaker
    /// </summary>
    /// <param name="ingredient"> The enum ingredient type to add </param>
    public void AddIngredient(IngredientType ingredient)
    {
        m_contents.Add(ingredient);
        if (ingredientsInSound != null)
        {
            ingredientsInSound.Play();
        }
    }

    /// <summary>
    /// Checks if the ingredients in the shaker make a valid recipe and returns that potion, otherwise returns mundane potion
    /// </summary>
    /// <returns> The potion the recipe in the shaker matches, mundane potion if it doesn't match any </returns>
    private GameObject GetPotion()
    {
        // if potion doesn't contain valid amount of ingredients
        if (m_contents.Count != 2)
            return m_mundanePotion;

        int recipe = (int)m_contents[0] | (int)m_contents[1];
        // if recipe is valid
        if (m_manager.m_potionRecipeDictionary.ContainsKey(recipe))
        {
            return m_manager.m_potionRecipeDictionary[recipe];
        }

        // potion doesn't match any combinations
        return m_mundanePotion;
    }

    /// <summary>
    /// Places the cap onto the shaker
    /// </summary>
    /// <param name="cap"> The shaker cap game object </param>
    public void PlaceCap(GameObject cap)
    {
        Physics.IgnoreCollision(m_collider, cap.GetComponent<Collider>(), true);
        // move to place
        cap.transform.SetPositionAndRotation(m_capPlacedTransform.position, m_capPlacedTransform.rotation);
        cap.transform.parent = transform;
        cap.GetComponent<Rigidbody>().isKinematic = true;
        m_bCapOn = true;
    }

    /// <summary>
    /// Removes the cap from the shaker
    /// </summary>
    public void RemoveCap()
    {
        Physics.IgnoreCollision(m_collider, m_cap.GetComponent<Collider>(), false);
        m_bCapOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShakerCap>()
            && !m_bCapOn)
        {
            Hand hand = m_cap.GetComponentInParent<Hand>();

            // detach from hand if on hand
            if (hand)
            {
                hand.DetachObject(m_cap);
                m_cap.GetComponent<Interactable>().attachedToHand = null;
            }

            PlaceCap(m_cap);
        }
        else if (other.GetComponent<Boundary>())
            BoundaryReset();
    }

    /// <summary>
    /// Resets the position, rotation, velocity and angular velocity of the shaker.
    /// Called upon entering boundary trigger.
    /// </summary>
    public void BoundaryReset()
    {
        transform.position = m_startTransform.position;
        transform.rotation = m_startTransform.rotation;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
    }

    public bool IsCapOn() => m_bCapOn;
}