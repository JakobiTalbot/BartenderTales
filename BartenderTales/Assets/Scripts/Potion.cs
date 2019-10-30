using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Potion : MonoBehaviour
{
    public PotionName m_potionName;
    [SerializeField]
    private float m_impulseThresholdToPlayCollisionAudio = 0.5f;
    [SerializeField]
    private AudioClip[] m_collisionAudioClips;
    [SerializeField]
    private float m_impulseThresholdToShatterPotion = 2f;
    [SerializeField]
    private GameObject m_shatterPrefab;
    

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
            // create shatter effect
            Destroy(Instantiate(m_shatterPrefab, transform.position, Quaternion.identity), 5f);
            m_bDrank = true;
            cust.DrinkPotion(this);
        }
        else
        {
            if (collision.impulse.magnitude > m_impulseThresholdToPlayCollisionAudio)
            {
                m_audioSource.volume = collision.impulse.magnitude / 5f;
                m_audioSource.PlayOneShot(m_collisionAudioClips[Random.Range(0, m_collisionAudioClips.Length)]);
            }
        }
    }
}