using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Page : MonoBehaviour
{
    private Collider m_collider;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out var hit, 1f)
            && hit.transform.GetComponent<Page>())
        {
            m_collider.enabled = false;
        }
        else
        {
            m_collider.enabled = true;
        }
    }
}
