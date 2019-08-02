using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

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
            // detach from hand
            GetComponentInParent<Hand>()?.DetachObject(gameObject, true);
            GetComponent<Interactable>().attachedToHand = null;
            gameObject.SetActive(false);
        }
    }
}