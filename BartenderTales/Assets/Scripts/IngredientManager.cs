using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IngredientPoint
{
    public GameObject m_ingredientTypePrefab;
    public Transform m_ingredientTypeSpawnPoint;
    [HideInInspector]
    public List<GameObject> m_spawnedIngredients;
}

public class IngredientManager : MonoBehaviour
{
    [SerializeField]
    private int m_maximumIngredientsPerType = 2;

    public IngredientPoint[] m_ingredientPoints;

    // Start is called before the first frame update
    void Start()
    {
        CreateIngredients();
    }

    /// <summary>
    /// Creates instances of ingredient objects in array of points
    /// </summary>
    private void CreateIngredients()
    {
        foreach (IngredientPoint point in m_ingredientPoints)
        {
            // spawn ingredient on point
            for (int i = 0; i < m_maximumIngredientsPerType; ++i)
                point.m_spawnedIngredients.Add(Instantiate(point.m_ingredientTypePrefab, point.m_ingredientTypeSpawnPoint.position, point.m_ingredientTypeSpawnPoint.rotation));
        }
    }

    /// <summary>
    /// Resets all used ingredients to their spawn points and sets them to active
    /// </summary>
    public void RefillIngredients()
    {
        foreach (IngredientPoint point in m_ingredientPoints)
        {
            for (int i = 0; i < m_maximumIngredientsPerType; ++i)
            {
                // reset ingredient if its not active
                if (!point.m_spawnedIngredients[i].activeSelf)
                {
                    point.m_spawnedIngredients[i].SetActive(true); // activate ingredient
                    point.m_spawnedIngredients[i].transform.SetPositionAndRotation(point.m_ingredientTypeSpawnPoint.position, point.m_ingredientTypeSpawnPoint.rotation);
                }
            }
        }
    }
}
