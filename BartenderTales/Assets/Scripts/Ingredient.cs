using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Ingredient : MonoBehaviour
{
    public IngredientType m_ingredientType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shaker>())
        {
            Shaker shaker = other.GetComponent<Shaker>();
            // don't add ingredient if the cap is on
            if (shaker.IsCapOn())
                return;

            shaker.AddIngredient(m_ingredientType);
            // detach from hand
            GetComponentInParent<Hand>()?.DetachObject(gameObject, true);
            GetComponent<Interactable>().attachedToHand = null;
            gameObject.SetActive(false);
        }
    }
}