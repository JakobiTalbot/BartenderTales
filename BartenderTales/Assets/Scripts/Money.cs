using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoneyJar>())
        {
            // put money in jar
            other.GetComponent<MoneyJar>().AddMoney(1);
            transform.parent = null;
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}
