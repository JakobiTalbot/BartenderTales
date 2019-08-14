using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Button : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onButtonPressed;

    private Rigidbody m_rb;
    private GameObject m_activeHand;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hand>())
        {
            m_activeHand = other.gameObject;
        }
        else if (other.GetComponent<ButtonBottom>())
        {
            m_onButtonPressed.Invoke();
            m_activeHand = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hand>())
        {
            m_activeHand = null;
        }
        
    }

    private IEnumerator UpdateButton()
    {
        while (m_activeHand)
        {
            Vector3 newPos = m_rb.position;
            newPos.y = m_activeHand.transform.position.y;
            m_rb.MovePosition(newPos);
            yield return new WaitForEndOfFrame();
        }
    }
}