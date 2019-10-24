using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShakerCap : MonoBehaviour
{
    private Shaker m_shaker;
    private Transform m_startTransform;
    private Rigidbody m_rb;
    private Renderer m_renderer;
    private Color m_startColor;

    private void Awake()
    {
        // get default transform
        m_startTransform = new GameObject().transform;
        m_startTransform.position = transform.position;
        m_startTransform.rotation = transform.rotation;

        m_renderer = GetComponent<Renderer>();
        m_startColor = m_renderer.material.color;
        m_shaker = FindObjectOfType<Shaker>();
        m_rb = GetComponent<Rigidbody>();
    }

    public void EnableHeld()
    {
        m_shaker.RemoveCap();
    }
    public void DisableHeld()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Boundary>())
            BoundaryReset();
    }

    public void ResetColour()
    {
        m_renderer.material.color = m_startColor;
    }

    public void LerpShakeColour(Color color, float fLerp)
    {
        m_renderer.material.color = Color.Lerp(m_startColor, color, fLerp);
    }

    public void BoundaryReset()
    {
        transform.position = m_startTransform.position;
        transform.rotation = m_startTransform.rotation;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        transform.parent = null;
        m_shaker.RemoveCap();
    }
}