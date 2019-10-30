using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class CustomerVoice : MonoBehaviour
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

    protected AudioSource m_audioSource;
    protected Animator m_animator;
    protected Dictionary<PotionName, AudioClip[]> m_potionOrderAudio;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
    }

    private void GenerateDictionary()
    {
        m_potionOrderAudio.Add(PotionName.PickMeUp, m_pickMeUpAudioClips);
        m_potionOrderAudio.Add(PotionName.SmokeyTeleport, m_smokeyTeleportAudioClips);
        m_potionOrderAudio.Add(PotionName.PixieDust, m_pixieDustAudioClips);
        m_potionOrderAudio.Add(PotionName.CosyFire, m_cosyFireClips);
        m_potionOrderAudio.Add(PotionName.CoughUp, m_coughUpClips);
        m_potionOrderAudio.Add(PotionName.CupidsKiss, m_cupidsKissClips);
        m_potionOrderAudio.Add(PotionName.NewYou, m_newYouAudioClips);
    }

    abstract public IEnumerator RandomSoundLoop();
    abstract public void AngrySound();
    abstract public void HappySound();
    abstract public void OrderSound(PotionName potion);
}