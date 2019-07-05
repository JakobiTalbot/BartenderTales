using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionEffect : MonoBehaviour
{
    [HideInInspector]
    public PotionName m_potionName;

    public PotionName potionName { get; }
}