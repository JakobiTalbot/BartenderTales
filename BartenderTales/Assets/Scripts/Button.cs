using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onButtonPressed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ButtonBottom>())
        {
            m_onButtonPressed.Invoke();
        }
    }
}
