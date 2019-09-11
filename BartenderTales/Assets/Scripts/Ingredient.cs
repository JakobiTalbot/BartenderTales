using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Ingredient : MonoBehaviour
{
    public IngredientType m_ingredientType;

    private Shaker m_shaker;
    private bool m_bCanEnterShaker = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Shaker>()
            && m_bCanEnterShaker)
        {
            // get reference to shaker
            if (!m_shaker)
                m_shaker = other.GetComponent<Shaker>();

            // don't add ingredient if the cap is on
            if (m_shaker.IsCapOn())
                return;

            m_shaker.AddIngredient(this);
            // detach from hand
            GetComponentInParent<Hand>()?.DetachObject(gameObject, true);
            GetComponent<Interactable>().attachedToHand = null;
            gameObject.SetActive(false);
        }
    }

    public IEnumerator DisallowEnteringShakerForSeconds(float time)
    {
        m_bCanEnterShaker = false;
        yield return new WaitForSeconds(time);
        m_bCanEnterShaker = true;
    }
}