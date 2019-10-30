using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundOnAwake : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_audioClips;
    
    void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();

        // play random sound
        source.PlayOneShot(m_audioClips[Random.Range(0, m_audioClips.Length)]);
    }
}