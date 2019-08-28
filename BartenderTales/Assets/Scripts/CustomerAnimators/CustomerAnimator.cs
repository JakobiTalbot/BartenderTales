﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class CustomerAnimator : MonoBehaviour
{
    protected Animator m_animator;
    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    abstract public IEnumerator IdleLoop();
    abstract public void Shocked();
    abstract public void Cheer();
    abstract public void Order();
}