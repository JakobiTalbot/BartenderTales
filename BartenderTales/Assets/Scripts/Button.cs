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


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ButtonBottom>())
        {
            m_onButtonPressed.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Hand>())
        {
            Vector3 newPos = m_rb.position;
            newPos.y = other.transform.position.y;
            m_rb.MovePosition(newPos);
        }
    }
}