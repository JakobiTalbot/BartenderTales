using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    WhiskeyWatermelon = 0,
    EmuElderberry,
    PixiePear,
    ElvenMagicBox,
    DragonChilli,
    UnicornFeathers,
    Count
}

public class Ingredient : MonoBehaviour
{
    public IngredientType m_ingredientType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shaker>())
        {
            other.GetComponent<Shaker>().AddIngredient(m_ingredientType);
            transform.parent = null;
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}
