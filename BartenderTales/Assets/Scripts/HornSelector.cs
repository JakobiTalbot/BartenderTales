using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_horns;

    private void Awake()
    {
        // enable random horn on creation
        int i = Random.Range(0, m_horns.Length);
        m_horns[i].SetActive(true);
    }
}