using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewYou : PotionEffect
{
    [SerializeField]
    private float m_secondsToWaitUntilSwitching = 0.2f;
    [SerializeField]
    private float m_secondsForNewCustomerToWaitUntilLeaving = 4f;

    private Customer m_cust;

    void Awake()
    {
        m_potionName = PotionName.NewYou;
        if (m_cust = GetComponent<Customer>())
            StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        PotionAssets pa = FindObjectOfType<PotionAssets>();
        CustomerSpawner cs = FindObjectOfType<CustomerSpawner>();

        // create particles to hide switch
        Instantiate(pa.m_newYouParticlePrefab, transform.position, Quaternion.identity);

        // get new customer prefab
        List<GameObject> customerPrefabs = new List<GameObject>(pa.m_customerPrefabs);
        for (int i = 0; i < customerPrefabs.Count; ++i)
        {
            if (m_cust.GetCustomerType() == customerPrefabs[i].GetComponent<Customer>().GetCustomerType())
            {
                customerPrefabs.RemoveAt(i);
                break;
            }
        }

        GameObject newCustomer = customerPrefabs[Random.Range(0, customerPrefabs.Count)];

        yield return new WaitForSeconds(m_secondsToWaitUntilSwitching);

        // create new customer
        GameObject newCustObj = Instantiate(newCustomer, transform.position, transform.rotation);
        Customer newCust = newCustObj.GetComponent<Customer>();
        newCust.GetReferences();

        // set values for new customer
        if (newCust.GetCustomerType() != CustomerType.CatMan)
        {
            GameObject hat = FindObjectOfType<CustomerSpawner>().GetRandomHat();
            if (hat)
                newCust.SetHat(hat);
        }

        newCust.SetCoinDropPos(cs.m_coinDropPoint.position);        

        newCust.GoIdle();

        // wait until smoke clears before reacting
        newCust.Invoke("CosyFireReact", 1.5f);

        // exit bar after a few seconds
        newCust.Invoke("ExitBar", m_secondsForNewCustomerToWaitUntilLeaving);

        // destroy old customer
        Destroy(gameObject);
    }
}