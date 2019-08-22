using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    [SerializeField]
    private int m_maximumIngredientsPerType = 2;
    [SerializeField]
    [Tooltip("Ensure there is at least one point for each ingredient prefab, otherwise no ingredients will spawn")]
    private Transform[] m_ingredientSpawnPoints;
    [SerializeField]
    private GameObject[] m_potionPrefabs;
    [SerializeField]
    private GameObject[] m_ingredientPrefabs;

    [HideInInspector]
    public int[] m_recipes;
    [HideInInspector]
    public Dictionary<int, GameObject> m_potionRecipeDictionary;

    private GameObject[] m_spawnedIngredients;
    private int m_nPointsPerIngredientType;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: only generate recipes on new game
        GenerateRecipes();
        CreateIngredients();
    }

    private void GenerateRecipes()
    {
        // create new array
        m_recipes = new int[(int)PotionName.Count];
        m_potionRecipeDictionary = new Dictionary<int, GameObject>();

        List<int> existingRecipes = new List<int>();

        for (int i = 0; i < m_recipes.Length; ++i)
        {
            int ingredient1 = 1 << Random.Range(0, (int)IngredientType.Count);
            int ingredient2 = 1 << Random.Range(0, (int)IngredientType.Count);

            while ((ingredient1 & ingredient2) > 0)
            {
                // ingredient double up... get new ingredient2
                ingredient2 = 1 << Random.Range(0, (int)IngredientType.Count);
            }

            int recipe = ingredient1 | ingredient2;

            // recreate recipe if already exists
            if (existingRecipes.Contains(recipe))
                i -= 1;
            else
            {
                m_recipes[i] = recipe;
                existingRecipes.Add(m_recipes[i]);
                // add recipe to dictionary
                m_potionRecipeDictionary.Add(recipe, m_potionPrefabs[i]);
            }
        }
    }

    /// <summary>
    /// Creates instances of ingredient objects in array of points
    /// </summary>
    private void CreateIngredients()
    {
        List<Transform> availablePoints = new List<Transform>(m_ingredientSpawnPoints);
        m_spawnedIngredients = new GameObject[m_ingredientSpawnPoints.Length];
        m_nPointsPerIngredientType = m_ingredientSpawnPoints.Length / m_ingredientPrefabs.Length;
        // for each ingredient
        for (int i = 0; i < m_ingredientPrefabs.Length; ++i)
        {
            // spawn on number of points
            for (int j = 0; j < m_nPointsPerIngredientType; ++j)
            {
                int randPoint = Random.Range(0, availablePoints.Count);
                m_spawnedIngredients[i + j] = Instantiate(m_ingredientPrefabs[i], availablePoints[randPoint].transform.position, availablePoints[randPoint].transform.rotation);
                availablePoints.RemoveAt(randPoint);
            }
        }
    }

    /// <summary>
    /// Respawn ingredients randomly
    /// </summary>
    public void RefillIngredients()
    {
        if (!m_spawnedIngredients[0])
            return;
        List<Transform> availablePoints = new List<Transform>(m_ingredientSpawnPoints);

        for (int i = 0; i < m_spawnedIngredients.Length; ++i)
        {
            // move ingredient to random point
            int randPoint = Random.Range(0, availablePoints.Count);
            m_spawnedIngredients[i].transform.SetPositionAndRotation(availablePoints[randPoint].transform.position, availablePoints[randPoint].transform.rotation);
            availablePoints.RemoveAt(randPoint);

            // activate ingredient
            m_spawnedIngredients[i].SetActive(true);
        }
    }
}
