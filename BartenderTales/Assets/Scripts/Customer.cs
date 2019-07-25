using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    public GameObject m_speechBubbleCanvas;
    public TextMeshProUGUI m_text;
    public Image m_bubble;
    [Tooltip("The speed of the lerp for the customer to rotate to face the player")]
    public float m_rotationSpeed = 0.02f;
    [Tooltip("The time to wait after drinking until the customer leaves the bar")]
    public float m_timeUntilExitingBarAfterDrinking = 2f;
    public int m_reputationOnCorrectOrder = 1;
    public int m_reputationOnWrongOrder = -1;
    public Vector2 m_speechBubbleBuffer = new Vector2(1, 1);

    private PotionName m_order;
    private CustomerSpawner m_spawner;
    private NavMeshAgent m_agent;
    private Transform m_point;
    private bool m_bWaiting = true;
    private bool m_bBadPerson = false;
    private bool m_bExitingBar = false;
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        // order random potion
        // TODO: only order good potions
        m_order = (PotionName)Random.Range(0, (int)PotionName.Count);
        m_spawner = FindObjectOfType<CustomerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_agent.remainingDistance < 0.6f)
        {
            if (m_bExitingBar)
            {
                Destroy(gameObject);
                return;
            }
            else if (!m_agent.isStopped
                && !m_bWaiting)
                Speak("drink plz");
            // face player
            Vector3 v3Pos = Camera.main.transform.position;
            v3Pos.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3Pos - transform.position), m_rotationSpeed);
            // stop moving
            m_agent.isStopped = true;
            if (m_bWaiting && m_spawner.m_servingPoints.Count > 0)
            {
                SetDestination(m_spawner.m_servingPoints[Random.Range(0, m_spawner.m_servingPoints.Count)]);
                m_bWaiting = false;
            }
        }
        else if (m_agent.isStopped)
            SetDestination(m_point);
    }

    public void SetDestination(Transform dest)
    {
        if (!m_agent)
            m_agent = GetComponent<NavMeshAgent>();
        m_agent.isStopped = false;
        m_point = dest;
        m_agent.SetDestination(dest.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Potion>())
        {
            PotionEffect p = collision.gameObject.GetComponent<PotionEffect>();
            // TODO: reactions to potions
            // if correct potion given
            if (m_order == p.m_potionName)
            {
                FindObjectOfType<ReputationManager>().AddToReputation(m_reputationOnCorrectOrder);
                // happy reaction
            }
            else // if wrong potion given
            {
                if (m_bBadPerson)
                {
                    FindObjectOfType<ReputationManager>().AddToReputation(m_reputationOnCorrectOrder);
                }
                else
                {
                    FindObjectOfType<ReputationManager>().AddToReputation(m_reputationOnWrongOrder);
                    // sad reaction
                }
            }
            System.Type type = FindObjectOfType<Shaker>().m_potionFunc[p.m_potionName].GetType();
            // drink potion
            gameObject.AddComponent(type);

            // dirty way to fix errors from object being destroyed when still on hand
            collision.transform.parent = null;
            collision.gameObject.SetActive(false);

            Destroy(collision.gameObject);
            Invoke("ExitBar", m_timeUntilExitingBarAfterDrinking);
        }
    }

    private void ExitBar()
    {
        m_bExitingBar = true;
        m_agent.isStopped = false;
        m_point = FindObjectOfType<CustomerSpawner>().m_spawnPoint;
        m_agent.SetDestination(m_point.position);
    }

    public void Speak(string text)
    {
        m_speechBubbleCanvas.SetActive(true);
        m_text.text = text;
        m_text.rectTransform.sizeDelta = m_text.GetPreferredValues();
        m_bubble.rectTransform.sizeDelta = (m_text.rectTransform.sizeDelta + m_speechBubbleBuffer);
        StartCoroutine(BubbleTimer(5f));
    }

    IEnumerator BubbleTimer(float fTime)
    {
        yield return new WaitForSeconds(fTime);
        m_speechBubbleCanvas.SetActive(false);
    }
}