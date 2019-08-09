using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixieDust : PotionEffect
{
    public float m_hoverHeight = 1.5f;
    public float m_hoverHeightVariance = 0.5f;
    public float m_bobSpeed = 2f;
    public float m_startHoverUpTime = 1f;
    public float m_timeToHoverBeforeLeaving = 2f;

    private bool m_bReachedHoverHeight = false;
    private Rigidbody m_rb;
    private Vector3 m_v3StartPos;
    private Vector3 m_v3StartHoverPos;
    private float m_fHoverLerpTime = 0f;
    private float m_fTimeReachedHover = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        m_potionName = PotionName.PixieDust;
        if (!GetComponent<Customer>())
            return;

        m_rb = GetComponent<Rigidbody>();
        m_v3StartPos = transform.position;
        m_v3StartHoverPos = m_v3StartPos;
        m_v3StartHoverPos.y += m_hoverHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Customer>())
            return;

        // if haven't reached hover height yet
        if (!m_bReachedHoverHeight)
        {
            gameObject.GetComponent<Customer>().m_Animator.SetBool("HasDrunkFairyDust", true);
            // increase lerp timer
            m_fHoverLerpTime += Time.deltaTime / m_startHoverUpTime;

            // if at end of lerp
            if (m_fHoverLerpTime >= 1f)
            {
                m_bReachedHoverHeight = true;
                transform.position = m_v3StartHoverPos;
                m_fTimeReachedHover = Time.time;
                Invoke("Leave", m_timeToHoverBeforeLeaving);
            }
            else // lerp position to hover height
                transform.position = Vector3.Lerp(m_v3StartPos, m_v3StartHoverPos, m_fHoverLerpTime);
        }
        else // bob up and down
        {
            Vector3 newPos = transform.position;
            newPos.y = m_v3StartHoverPos.y + (Mathf.Sin((Time.time - m_fTimeReachedHover) * m_bobSpeed) * m_hoverHeightVariance);
            transform.position = newPos;
        }
    }

    private void Leave()
    {
        // customer starts leaving bar
        GetComponent<Customer>().ExitBar();
    }
}