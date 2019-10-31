using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurVoice : CustomerVoice
{
    public override IEnumerator RandomSoundLoop()
    {
        // wait before playing first random audio
        yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenRandomAudioClips.x, m_randomRangeBetweenRandomAudioClips.y));

        // end loop when customer exits idle state
        while (m_animator.GetBool("StoppedMoving"))
        {
            if (!m_voiceAudioSource.isPlaying)
                m_voiceAudioSource.PlayOneShot(m_randomAudioClips[Random.Range(0, m_randomAudioClips.Length)]);

            // wait before playing another audio clip
            yield return new WaitForSeconds(Random.Range(m_randomRangeBetweenRandomAudioClips.x, m_randomRangeBetweenRandomAudioClips.y));
        }
    }

    public override void AngrySound()
    {
        m_voiceAudioSource.PlayOneShot(m_angryAudioClips[Random.Range(0, m_angryAudioClips.Length)]);
    }

    public override void HappySound()
    {
        m_voiceAudioSource.PlayOneShot(m_happyAudioClips[Random.Range(0, m_happyAudioClips.Length)]);
    }

    public override void OrderSound(PotionName potion)
    {
        // get array of order audio clips
        AudioClip[] orderAudio = m_potionOrderAudio[potion];
        // play random clip from array
        m_voiceAudioSource.PlayOneShot(orderAudio[Random.Range(0, orderAudio.Length)]);
    }
}