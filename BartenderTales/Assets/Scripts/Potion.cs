using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Potion : MonoBehaviour
{
    public PotionName m_potionName;

    private Transform m_point;
    private bool m_bDrank = false;

    public void SetPoint(Transform point)
    {
        m_point = point;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_bDrank)
            return;
        Customer cust;
        if (cust = collision.gameObject.GetComponentInParent<Customer>())
        {
            m_bDrank = true;
            cust.DrinkPotion(this);
        }
    }
}