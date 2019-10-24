using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Potion : MonoBehaviour
{
    public PotionName m_potionName;
    [SerializeField]
    private float m_impulseThresholdToPlayAudio = 0.5f;
    [SerializeField]
    private AudioClip[] m_collisionAudioClips;

    private Transform m_point;
    private AudioSource m_audioSource;
    private bool m_bDrank = false;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void SetPoint(Transform point)
    {
        m_point = point;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_bDrank)
            return;
        Customer cust;
        if (cust = collision.gameObject.GetComponentInParent<Customer>())
        {
            m_bDrank = true;
            cust.DrinkPotion(this);
        }
        else
        {
            Debug.Log(collision.impulse.magnitude);
            if (collision.impulse.magnitude > m_impulseThresholdToPlayAudio)
            {
                m_audioSource.volume = collision.impulse.magnitude / 5f;
                m_audioSource.PlayOneShot(m_collisionAudioClips[Random.Range(0, m_collisionAudioClips.Length)]);
            }
        }
    }
}