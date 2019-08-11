using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

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
    public int m_coinsGivenOnCorrectOrder = 1;
    public Vector2 m_speechBubbleBuffer = new Vector2(1, 1);
    public GameObject m_moneyPrefab;

    private PotionName m_order;
    private CustomerSpawner m_spawner;
    private NavMeshAgent m_agent;
    private Transform m_point;
    private bool m_bWaiting = true;
    private bool m_bBadPerson = false;
    private bool m_bExitingBar = false;
    private Vector3 m_v3CoinDropPos;
    private Rigidbody m_rigidbody;
    private CustomerAnimator m_customerAnimator;

    public Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_customerAnimator = GetComponent<CustomerAnimator>();
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
            // if reached the exit point of the bar
            if (m_bExitingBar)
            {
                m_spawner.m_customers.Remove(gameObject);
                Destroy(gameObject);
                return;
            }

            // if waiting and there is free spots at the front of bar
            if (m_bWaiting && m_spawner.m_servingPoints.Count > 0)
            {
                int i = Random.Range(0, m_spawner.m_servingPoints.Count);
                m_spawner.m_waitingPoints.Add(m_point);
                SetDestination(m_spawner.m_servingPoints[i], false);
                m_spawner.m_servingPoints.RemoveAt(i);
                m_bWaiting = false;
                //m_Animator.SetBool("WalkAgain", true);
            }

            // display speech bubble order
            else if (!m_agent.isStopped
                && !m_bWaiting)
                Speak(Regex.Replace(m_order.ToString(), "([A-Z])", " $1").Trim() + " please!");

            // face player
            Vector3 v3Pos = Camera.main.transform.position;
            v3Pos.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3Pos - transform.position), m_rotationSpeed);

            // stop moving
            m_agent.isStopped = true;
            m_rigidbody.isKinematic = true;
            m_Animator.SetBool("StoppedMoving", true);
            m_customerAnimator.StartCoroutine("IdleLoop");
        }
        else if (m_agent.isStopped)
            SetDestination(m_point, m_bWaiting);
    }

    public void SetDestination(Transform dest, bool bWait)
    {
        if (!m_agent)
            m_agent = GetComponent<NavMeshAgent>();
        m_bWaiting = bWait;
        m_agent.isStopped = false;
        m_point = dest;
        m_agent.SetDestination(dest.position);
        //m_Animator.SetBool("WalkAgain", true);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Potion>())
        {
            // add point to spawner points
            if (m_bWaiting)
                m_spawner.m_waitingPoints.Add(m_point);
            else
                m_spawner.m_servingPoints.Add(m_point);

            PotionEffect p = collision.gameObject.GetComponent<PotionEffect>();
            // TODO: reactions to potions
            // if correct potion given
            if (m_order == p.m_potionName)
            {
                FindObjectOfType<ReputationManager>().AddToReputation(m_reputationOnCorrectOrder);
                // give money
                Instantiate(m_moneyPrefab, m_v3CoinDropPos, Quaternion.Euler(Vector3.zero));
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

            if (p.m_potionName == PotionName.Mundane)
                gameObject.AddComponent<Mundane>();
            else
            {
                System.Type type = FindObjectOfType<Shaker>().m_potionFunc[p.m_potionName].GetType();
                // drink potion
                gameObject.AddComponent(type);
            }

            // dirty way to fix errors from object being destroyed when still on hand
            collision.transform.parent = null;
            collision.gameObject.SetActive(false);

            Destroy(collision.gameObject);
        }
    }

    public void ExitBar()
    {
        m_bExitingBar = true;
        m_agent.isStopped = false;
        m_point = FindObjectOfType<CustomerSpawner>().m_spawnPoint;
        m_agent.SetDestination(m_point.position);
    }

    public void SetCoinDropPos(Vector3 v3DropPos)
    {
        m_v3CoinDropPos = v3DropPos;
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