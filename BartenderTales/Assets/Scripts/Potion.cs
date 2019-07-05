using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Potion : MonoBehaviour
{
    public List<IngredientType> m_ingredients;
    public PotionName m_potionName;

    private Transform m_point;

    public void SetPoint(Transform point)
    {
        m_point = point;
    }
}