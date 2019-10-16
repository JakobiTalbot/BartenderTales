using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField]
    private float m_dissolveTime = 2f;
    [SerializeField]
    private float m_timeToWaitBeforeDissolving = 5f;

    private Renderer m_renderer;
    
    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        StartCoroutine(DissolveOverTime());
    }

    public IEnumerator DissolveOverTime()
    {
        float fLerp = 0f;

        yield return new WaitForSeconds(m_timeToWaitBeforeDissolving);

        while (fLerp < 1f)
        {
            m_renderer.material.SetFloat("_Amount", fLerp);
            fLerp += (Time.deltaTime / m_dissolveTime);

            yield return null;
        }

        m_renderer.material.SetFloat("_Amount", 1);
        m_renderer.enabled = false;

        Destroy(gameObject, 1f);
    }
}