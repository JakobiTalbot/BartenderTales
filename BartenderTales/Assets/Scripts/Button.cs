using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Button : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onButtonPressed;

    private Vector3 m_lastHandPos;
    private GameObject m_activeHand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hand>())
        {
            StartCoroutine(UpdateButtonPress());
            m_activeHand = other.gameObject;
            m_lastHandPos = m_activeHand.transform.position;
        }
        else if (other.GetComponent<ButtonBottom>())
        {
            m_onButtonPressed.Invoke();
            StopCoroutine(UpdateButtonPress());
        }
    }

    private IEnumerator UpdateButtonPress()
    {
        while (true)
        {
            transform.position += -transform.up * (m_lastHandPos.y - m_activeHand.transform.position.y);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hand>())
        {
            StopCoroutine(UpdateButtonPress());
            m_activeHand = null;
        }
    }
}