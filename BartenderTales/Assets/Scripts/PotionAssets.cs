﻿using System.Collections;
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
    public AudioClip[] m_teleportAudioClips;
    [Header("New You")]
    public GameObject m_newYouParticlePrefab;
    public GameObject[] m_customerPrefabs;
    [Header("Cupid's Kiss")]
    public GameObject m_cupidsKissParticlePrefab;
    public AudioClip m_cupidsKissActivationAudio;
    [Header("Pick Me Up")]
    public AudioClip[] m_bonesBreakingAudioClips;
}