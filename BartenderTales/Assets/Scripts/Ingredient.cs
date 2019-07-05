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
}
