using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPoint : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_inhabitant;
    public float m_distanceToForgetInhabitant = 0.5f;

    private Shaker m_shaker;

    private void Start()
    {
        m_shaker = FindObjectOfType<Shaker>();
    }

    private void Update()
    {
        if (m_inhabitant
            && Vector3.Distance(transform.position, m_inhabitant.transform.position) > m_distanceToForgetInhabitant)
        {
            m_inhabitant = null;
        }
    }

    public void SetInhabitant(GameObject inhabitant)
    {
        if (m_inhabitant)
            Destroy(m_inhabitant);
        m_inhabitant = Instantiate(inhabitant, transform.position, transform.rotation);
    }
}