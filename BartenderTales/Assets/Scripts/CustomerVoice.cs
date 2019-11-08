using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVoice : MonoBehaviour
{
    [Header("Random Audio")]
    [SerializeField]
    protected AudioClip[] m_randomAudioClips;
    [SerializeField]
    protected Vector2 m_randomRangeBetweenRandomAudioClips = new Vector2(5f, 10f);

    [Header("Reaction Audio")]
    [SerializeField]
    protected AudioClip[] m_angryAudioClips;
    [SerializeField]
    protected AudioClip[] m_happyAudioClips;

    [Header("Potion Ordering Audio")]
    [SerializeField]
    protected AudioClip[] m_cosyFireClips;
    [SerializeField]
    protected AudioClip[] m_coughUpClips;
    [SerializeField]
    protected AudioClip[] m_cupidsKissClips;
    [SerializeField]
    protected AudioClip[] m_newYouAudioClips;
    [SerializeField]
    protected AudioClip[] m_pickMeUpAudioClips;
    [SerializeField]
    protected AudioClip[] m_pixieDustAudioClips;
    [SerializeField]
    protected AudioClip[] m_smokeyTeleportAudioClips;

    [SerializeField]
    protected AudioSource m_voiceAudioSource;

    protected Animator m_animator;
    protected Dictionary<PotionName, AudioClip[]> m_potionOrderAudio;

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
            if (!m_voiceAudioSource.isPlaying
                && CustomerSpawner.m_activeCustomerVoices.Count < CustomerSpawner.MaxAmountOfCustomersToSpeakAtOnce)
            {
                m_voiceAudioSource.clip = m_randomAudioClips[Random.Range(0, m_randomAudioClips.Length)];
                m_voiceAudioSource.Play();
                CustomerSpawner.m_activeCustomerVoices.Add(this);
                Invoke("RemoveFromList", m_voiceAudioSource.clip.length);
            }

            // wait before playing another audio clip
            yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenRandomAudioClips.x, m_randomRangeBetweenRandomAudioClips.y));
        }
    }

    public void AngrySound()
    {
        m_voiceAudioSource.Stop();
        m_voiceAudioSource.clip = m_angryAudioClips[Random.Range(0, m_angryAudioClips.Length)];
        CustomerSpawner.m_activeCustomerVoices.Add(this);
        Invoke("RemoveFromList", m_voiceAudioSource.clip.length);
        m_voiceAudioSource.Play();
    }

    public void HappySound()
    {
        m_voiceAudioSource.Stop();
        m_voiceAudioSource.clip = m_happyAudioClips[Random.Range(0, m_happyAudioClips.Length)];
        CustomerSpawner.m_activeCustomerVoices.Add(this);
        Invoke("RemoveFromList", m_voiceAudioSource.clip.length);
        m_voiceAudioSource.Play();
    }

    public void OrderSound(PotionName potion)
    {
        // get array of order audio clips
        AudioClip[] orderAudio = m_potionOrderAudio[potion];
        m_voiceAudioSource.Stop();
        m_voiceAudioSource.clip = orderAudio[Random.Range(0, orderAudio.Length)];
        Invoke("RemoveFromList", m_voiceAudioSource.clip.length);
        m_voiceAudioSource.Play();
    }

    public void StopTalking()
    {
        m_voiceAudioSource.Stop();
    }

    private void RemoveFromList()
    {
        CustomerSpawner.m_activeCustomerVoices.Remove(this);
    }
}