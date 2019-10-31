using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PixieDust : PotionEffect
{
    public float m_hoverHeight = 1.5f;
    public float m_hoverHeightVariance = 0.5f;
    public float m_bobSpeed = 2f;
    public float m_startHoverUpTime = 1f;
    public float m_timeToHoverBeforeLeaving = 2f;

    [SerializeField]
    AudioClip m_activationSound;

    private Rigidbody m_rb;
    private Vector3 m_v3StartPos;
    private Vector3 m_v3StartHoverPos;
    private float m_fHoverLerpTime = 0f;
    private Customer m_customer;
    
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.PixieDust;

        // get reference and check if not null
        if (!(m_customer = GetComponent<Customer>()))
            return;

        m_rb = GetComponent<Rigidbody>();
        m_v3StartPos = transform.position;
        m_v3StartHoverPos = m_v3StartPos;
        m_v3StartHoverPos.y += m_hoverHeight;

        StartCoroutine(HoverEffect());
    }

    private IEnumerator HoverEffect()
    {
        // play audio
        GetComponent<AudioSource>().PlayOneShot(m_activationSound);

        // stop moving on nav mesh
        m_customer.StopMovement();

        // play animation
        m_customer.m_animator.SetTrigger("Levitate");
        // activate particles
        m_customer.m_sparkleEffect.SetActive(true);
        m_customer.m_speechBubbleCanvas.SetActive(false);

        // hover to starting height
        while (m_fHoverLerpTime < 1f)
        {
            transform.position = Vector3.Lerp(m_v3StartPos, m_v3StartHoverPos, m_fHoverLerpTime);
            // increase lerp timer
            m_fHoverLerpTime += Time.deltaTime / m_startHoverUpTime;

            yield return null;
        }
        
        // get time of reaching hover height so transition is less janky
        float fTimeReachedBaseHoverHeight = Time.time;
        // set position to start hover position, in case lerp went over 1
        transform.position = m_v3StartHoverPos;
        // start leaving after set time
        Invoke("Leave", m_timeToHoverBeforeLeaving);

        // bob up and down for the rest of eternity
        while (true)
        {
            Vector3 newPos = transform.position;
            newPos.y = m_v3StartHoverPos.y + (Mathf.Sin((Time.time - fTimeReachedBaseHoverHeight) * m_bobSpeed) * m_hoverHeightVariance);
            transform.position = newPos;

            yield return null;
        }
    }

    private void Leave()
    {
        // customer starts leaving bar
        m_customer.ExitBar();
    }
}