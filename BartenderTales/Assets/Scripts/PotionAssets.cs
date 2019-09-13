using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PotionAssets : MonoBehaviour
{
    [Header("Cosy Fire")]
    public GameObject m_cosyFireParticles;
    [Header("Mundane")]
    public GameObject m_mundaneExplosionPrefab;
    [Header("Cough Up")]
    public GameObject[] m_coughUpRandomObjectPrefabs;
    [Header("Smokey Teleport")]
    public Vector2 m_randomRangeBetweenTeleports;
    public float m_teleportAreaRadius = 8f;
    public Vector2 m_randomTimeToDoTeleporting;
    public GameObject m_smokeyTeleportParticlePrefab;
}