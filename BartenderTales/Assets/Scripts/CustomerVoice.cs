using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVoice : MonoBehaviour
{
    [Header("Random Audio")]
    [SerializeField]
    private AudioClip[] m_randomAudioClips;
    [SerializeField]
    private Vector2 m_randomRangeBetweenRandomAudioClips = new Vector2(5f, 10f);

    [Header("Reaction Audio")]
    [SerializeField]
    private AudioClip[] m_angryAudioClips;
    [SerializeField]
    private AudioClip[] m_happyAudioClips;

    [Header("Potion Ordering Audio")]
    [SerializeField]
    private AudioClip[] m_cosyFireClips;
    [SerializeField]
    private AudioClip[] m_coughUpClips;
    [SerializeField]
    private AudioClip[] m_cupidsKissClips;
    [SerializeField]
    private AudioClip[] m_newYouAudioClips;
    [SerializeField]
    private AudioClip[] m_pickMeUpAudioClips;
    [SerializeField]
    private AudioClip[] m_pixieDustAudioClips;
    [SerializeField]
    private AudioClip[] m_smokeyTeleportAudioClips;

    private AudioSource m_audioSource;
    private Animator m_animator;
    private Dictionary<PotionName, AudioClip[]> m_potionOrderAudio;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        GenerateDictionary();
    }

    private void GenerateDictionary()
    {
        m_potionOrderAudio = new Dictionary<PotionName, AudioClip[]>();

        m_potionOrderAudio.Add(PotionName.PickMeUp, m_pickMeUpAudioClips);
        m_potionOrderAudio.Add(PotionName.SmokeyTeleport, m_smokeyTeleportAudioClips);
        m_potionOrderAudio.Add(PotionName.PixieDust, m_pixieDustAudioClips);
        m_potionOrderAudio.Add(PotionName.CosyFire, m_cosyFireClips);
        m_potionOrderAudio.Add(PotionName.CoughUp, m_coughUpClips);
        m_potionOrderAudio.Add(PotionName.CupidsKiss, m_cupidsKissClips);
        m_potionOrderAudio.Add(PotionName.NewYou, m_newYouAudioClips);
    }

    public IEnumerator RandomSoundLoop()
    {
        // wait before playing first random audio
        yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenRandomAudioClips.x, m_randomRangeBetweenRandomAudioClips.y));

        // end loop when customer exits idle state
        while (m_animator.GetBool("StoppedMoving"))
        {
            if (!m_audioSource.isPlaying
                && CustomerSpawner.m_activeCustomerVoices.Count < CustomerSpawner.MaxAmountOfCustomersToSpeakAtOnce)
            {
                m_audioSource.clip = m_randomAudioClips[Random.Range(0, m_randomAudioClips.Length)];
                m_audioSource.Play();
                CustomerSpawner.m_activeCustomerVoices.Add(this);
                Invoke("RemoveFromList", m_audioSource.clip.length);
            }

            // wait before playing another audio clip
            yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenRandomAudioClips.x, m_randomRangeBetweenRandomAudioClips.y));
        }
    }

    public void AngrySound()
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_angryAudioClips[Random.Range(0, m_angryAudioClips.Length)];
        CustomerSpawner.m_activeCustomerVoices.Add(this);
        Invoke("RemoveFromList", m_audioSource.clip.length);
        m_audioSource.Play();
    }

    public void HappySound()
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_happyAudioClips[Random.Range(0, m_happyAudioClips.Length)];
        CustomerSpawner.m_activeCustomerVoices.Add(this);
        Invoke("RemoveFromList", m_audioSource.clip.length);
        m_audioSource.Play();
    }

    public void OrderSound(PotionName potion)
    {
        // get array of order audio clips
        AudioClip[] orderAudio = m_potionOrderAudio[potion];
        m_audioSource.Stop();
        m_audioSource.clip = orderAudio[Random.Range(0, orderAudio.Length)];
        Invoke("RemoveFromList", m_audioSource.clip.length);
        m_audioSource.Play();
    }

    private void RemoveFromList()
    {
        CustomerSpawner.m_activeCustomerVoices.Remove(this);
    }
}