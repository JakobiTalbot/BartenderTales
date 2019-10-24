using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public enum PotionName
{
    PickMeUp = 0,
    SmokeyTeleport,
    PixieDust,
    CosyFire,
    CoughUp,
    CupidsKiss,
    NewYou,
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
    [SerializeField]
    private AudioClip[] m_shakerCapRemoveAudio;
    [SerializeField]
    private AudioClip[] m_shakerCapPlaceAudio;
    [SerializeField]
    private float m_emptyShakerForceThreshold = 1f;
    [SerializeField]
    private Color m_shakenColour = Color.red;
    [SerializeField]
    private float m_shakerCapPopForce = 10f;

    [HideInInspector]
    public Dictionary<PotionName, PotionEffect> m_potionFunc;

    private ParticleSystem m_particleSystem;
    private GameObject m_cap;
    private List<Ingredient> m_contents;
    private Rigidbody m_rb;
    private float m_fCurrentShakeTime = 0f;
    private Vector3 m_v3LastPos;
    private Vector3 m_v3LastDeltaPos;
    private bool m_bCapOn = false;
    private Collider m_collider;
    private IngredientManager m_manager;
    private Transform m_startTransform;
    private AudioSource m_audioSource;
    private Renderer m_renderer;
    private Color m_startColour;
    private ShakerCap m_capClass;

    public AudioSource ingredientsInSound;

    // Start is called before the first frame update
    void Start()
    {
        // get default transform
        m_startTransform = new GameObject().transform;
        m_startTransform.position = transform.position;
        m_startTransform.rotation = transform.rotation;

        m_manager = FindObjectOfType<IngredientManager>();
        m_renderer = GetComponent<Renderer>();
        m_startColour = m_renderer.material.color;
        m_audioSource = GetComponent<AudioSource>();
        m_collider = GetComponent<Collider>();
        m_contents = new List<Ingredient>();
        m_rb = GetComponent<Rigidbody>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_v3LastPos = transform.position;
        m_capClass = FindObjectOfType<ShakerCap>();
        m_cap = m_capClass.gameObject;
        CreatePotionDictionary();
    }

    private void CreatePotionDictionary()
    {
        m_potionFunc = new Dictionary<PotionName, PotionEffect>();
        m_potionFunc.Add(PotionName.CosyFire, m_potionPrefabs[0].GetComponent<CosyFire>());
        m_potionFunc.Add(PotionName.PixieDust, m_potionPrefabs[1].GetComponent<PixieDust>());
        m_potionFunc.Add(PotionName.CoughUp, m_potionPrefabs[2].GetComponent<CoughUp>());
        m_potionFunc.Add(PotionName.SmokeyTeleport, m_potionPrefabs[3].GetComponent<SmokeyTeleport>());
        m_potionFunc.Add(PotionName.PickMeUp, m_potionPrefabs[4].GetComponent<PickMeUp>());
        m_potionFunc.Add(PotionName.CupidsKiss, m_potionPrefabs[5].GetComponent<CupidsKiss>());
        m_potionFunc.Add(PotionName.NewYou, m_potionPrefabs[6].GetComponent<NewYou>());
    }

    // Update is called once per frame
    void Update()
    {
        // get force of shaker movement
        Vector3 force = ((transform.position - m_v3LastPos) - m_v3LastDeltaPos);
            
        // if player is shaking drink
        if (force.magnitude > m_accelShakeThreshold)
        {
            // recipe mixing
            if (m_contents.Count >= 2
                && m_bCapOn)
            {
                m_fCurrentShakeTime += Time.deltaTime;

                float fColourLerp = m_fCurrentShakeTime / m_shakeTime;
                m_renderer.material.color = Color.Lerp(m_startColour, m_shakenColour, fColourLerp);
                m_capClass.LerpShakeColour(m_shakenColour, fColourLerp);

                // if drink shaken
                if (m_fCurrentShakeTime > m_shakeTime)
                {
                    CreatePotions();
                    ResetColours();
                    PopCap();
                }
            }
            // empty ingredients
            else if (transform.up.y < 0f
                     && !m_bCapOn
                     && m_contents.Count > 0
                     && force.magnitude > m_emptyShakerForceThreshold)
            {
                EmptyShaker();
                ResetColours();
            }
        }

        m_v3LastDeltaPos = (transform.position - m_v3LastPos);
        m_v3LastPos = transform.position;
    }

    private void ResetColours()
    {
        m_renderer.material.color = m_startColour;
        m_capClass.ResetColour();
    }

    private void PopCap()
    {
        RemoveCap();
        m_cap.transform.parent = null;
        Rigidbody rb = m_cap.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity += transform.up * m_shakerCapPopForce;
    }

    /// <summary>
    /// Empties all the ingredients from the shaker, setting ingredient positions to outside the shaker
    /// </summary>
    private void EmptyShaker()
    {
        foreach (Ingredient ingredient in m_contents)
        {
            ingredient.gameObject.SetActive(true);
            ingredient.transform.position = m_capPlacedTransform.position;
            // make sure the ingredients don't instantly re-enter the shaker
            StartCoroutine(ingredient.DisallowEnteringShakerForSeconds(1f));
        }

        // clear list of ingredients
        m_contents.Clear();
    }

    private void CreatePotions()
    {
        // find potion type
        GameObject potion = GetPotion();
        // play audio
        //m_audioSource?.PlayOneShot(m_audioClipsOnPotionCreation[Random.Range(0, m_audioClipsOnPotionCreation.Length)]);

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

    /// <summary>
    /// Add an ingredient to the shaker
    /// </summary>
    /// <param name="ingredient"> The ingredient to add </param>
    public void AddIngredient(Ingredient ingredient)
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

        int recipe = (int)m_contents[0].m_ingredientType | (int)m_contents[1].m_ingredientType;
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
        // ignore collision to prevent buggy physics
        Physics.IgnoreCollision(m_collider, cap.GetComponent<Collider>(), true);

        // play audio
        m_audioSource.PlayOneShot(m_shakerCapPlaceAudio[Random.Range(0, m_shakerCapPlaceAudio.Length)]);

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
        // reenable collision
        Physics.IgnoreCollision(m_collider, m_cap.GetComponent<Collider>(), false);

        // play audio
        m_audioSource.PlayOneShot(m_shakerCapRemoveAudio[Random.Range(0, m_shakerCapRemoveAudio.Length)]);
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