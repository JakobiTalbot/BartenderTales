using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public enum IngredientType
{
    Mushroom =          1,
    EmuElderberry =     1 << 1,
    PixiePear =         1 << 2,
    ElvenMagicBox =     1 << 3,
    DragonChilli =      1 << 4,
    UnicornFeathers =   1 << 5,
    Count =             6
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