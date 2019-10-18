using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
    public Rigidbody m_pickMeUpRigidbody;

    [SerializeField]
    private Material m_dissolveMaterial;
    [SerializeField]
    private Transform m_coughUpSpawnPoint;
    [SerializeField]
    private float m_impatienceTimer = 30f;
    [SerializeField]
    private Rigidbody[] m_ragdollRigidbodies;
    [SerializeField]
    private Transform m_hatTransform;
    [SerializeField]
    private CustomerType m_customerType;

    private PotionName m_order;
    private CustomerSpawner m_spawner;
    private NavMeshAgent m_agent;
    private Transform m_point;
    private bool m_bWaiting = true;
    private bool m_bExitingBar = false;
    private Vector3 m_v3CoinDropPos;
    private CustomerAnimator m_customerAnimator;
    private ReputationManager m_repManager;
    private ScoreManager m_scoreManager;
    private Renderer m_renderer;
    private bool m_bHadPath = false;
    private bool m_bIsRagdolling = false;
    private bool m_bIsTutorialNPC = false;
    private bool m_bPointReturned = false;
    
    public Animator m_animator;
    public GameObject m_sparkleEffect;
    public List<GameObject> m_trailEffects = new List<GameObject>();

    void Start()
    {
        m_repManager = FindObjectOfType<ReputationManager>();
        m_renderer = GetComponent<Renderer>();
        m_animator = GetComponent<Animator>();
        m_customerAnimator = GetComponent<CustomerAnimator>();
        m_agent = GetComponent<NavMeshAgent>();
        // order random potion
        m_order = (PotionName)Random.Range(0, (int)PotionName.Count);
        m_spawner = FindObjectOfType<CustomerSpawner>();
        SetRagdoll(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetRagdoll(!m_bIsRagdolling);

        if (m_bHadPath
            && m_agent.remainingDistance < 0.6f)
        {
            // if reached the exit point of the bar
            if (m_bExitingBar)
            {
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

            // customer orders
            else if (!m_agent.isStopped
                && !m_bWaiting)
            {
                Speak(Regex.Replace(m_order.ToString(), "([A-Z])", " $1").Trim() + " please!");
                m_customerAnimator.Order();
            }

            // face player
            if (!m_bIsRagdolling)
            {
                Vector3 v3Pos = Camera.main.transform.position;
                v3Pos.y = transform.position.y;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3Pos - transform.position), m_rotationSpeed);
            }

            // stop moving
            if (!m_agent.isStopped
                && m_agent.hasPath)
            {
                m_agent.isStopped = true;
                m_animator.SetBool("StoppedMoving", true);
                m_customerAnimator.StartCoroutine("IdleLoop");
            }
        }
        //else if (m_agent.isStopped)
            //SetDestination(m_point, m_bWaiting);
    }

    public void SetDestination(Transform dest, bool bWait)
    {
        if (!m_agent)
            m_agent = GetComponent<NavMeshAgent>();
        m_bWaiting = bWait;
        m_agent.isStopped = false;
        m_bHadPath = true;
        m_point = dest;

        m_agent.SetDestination(dest.position);
    }

    public void SetTutorialCustomer(bool bTutorialCustomer)
    {
        m_bIsTutorialNPC = bTutorialCustomer;
    }

    public void DrinkPotion(Potion potion)
    {
        // dont drink potion if ragdolled
        if (m_bIsRagdolling)
            return;

        // add point to spawner points
        AddPointToSpawner();

        PotionEffect p = potion.GetComponent<PotionEffect>();
        // TODO: reactions to potions
        // if correct potion given
        if (m_order == p.m_potionName)
        {
            m_repManager.AddToReputation(m_reputationOnCorrectOrder);
            m_scoreManager.AddCorrectOrder();
            // give money
            Instantiate(m_moneyPrefab, m_v3CoinDropPos, Quaternion.Euler(Vector3.zero));
            // happy reaction

            // drink potion
            System.Type type = FindObjectOfType<Shaker>().m_potionFunc[p.m_potionName].GetType();
            gameObject.AddComponent(type);
        }
        else // if wrong potion given
        {
            m_repManager.AddToReputation(-m_reputationOnWrongOrder);
            m_scoreManager.AddIncorrectOrder();
            // sad reaction

            if (p.m_potionName == PotionName.Mundane)
                gameObject.AddComponent<Mundane>();
            else
            {
                // drink potion
                System.Type type = FindObjectOfType<Shaker>().m_potionFunc[p.m_potionName].GetType();
                gameObject.AddComponent(type);
            }
        }

        // spawn new tutorial customer if this customer is tutorial customer
        if (m_bIsTutorialNPC)
            m_spawner.SpawnTutorialCustomer();

        // dirty way to fix errors from object being destroyed when still on hand
        potion.transform.parent = null;
        potion.gameObject.SetActive(false);

        Destroy(potion.gameObject);
    }

    /// <summary>
    /// Makes the customer start exiting the bar
    /// </summary>
    public void ExitBar()
    {
        m_bExitingBar = true;
        AddPointToSpawner();
        m_agent.isStopped = false;
        m_point = m_spawner.m_spawnPoints[Random.Range(0, m_spawner.m_spawnPoints.Length)];
        m_agent.SetDestination(m_point.position);
        m_animator.SetBool("StoppedMoving", false);
    }

    /// <summary>
    /// Adds the customer's waiting/serving point back to the list of them in the customer spawner
    /// </summary>
    public void AddPointToSpawner()
    {
        if (m_bPointReturned)
            return;

        if (m_bWaiting)
            m_spawner.m_waitingPoints.Add(m_point);
        else
            m_spawner.m_servingPoints.Add(m_point);

        m_bPointReturned = true;
    }

    public IEnumerator Dissolve()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            Texture t = r.material.GetTexture("_MainTex");
            Color c = r.material.GetColor("_Color");

            r.material = m_dissolveMaterial;
            if (t)
                r.material.SetTexture("_MainTex", t);
            r.material.SetColor("_Color", c);

            r.gameObject.AddComponent<Dissolve>();
        }

        yield return new WaitForSeconds(8f);

        Destroy(gameObject);
    }

    /// <summary>
    /// Sets the position to spawn a coin when the customer pays
    /// </summary>
    /// <param name="v3DropPos"></param>
    public void SetCoinDropPos(Vector3 v3DropPos)
    {
        m_v3CoinDropPos = v3DropPos;
    }

    public void SetScoreManager(ScoreManager scoreManager) => m_scoreManager = scoreManager;

    /// <summary>
    /// Activates the customer's speech bubble and displays specified text inside it
    /// </summary>
    /// <param name="text"> The string to display inside the speech bubble </param>
    public void Speak(string text)
    {
        m_speechBubbleCanvas.SetActive(true);
        m_text.text = text;
        m_text.rectTransform.sizeDelta = m_text.GetPreferredValues();
        m_bubble.rectTransform.sizeDelta = (m_text.rectTransform.sizeDelta + m_speechBubbleBuffer);
    }

    /// <summary>
    /// Sets the ragdoll state of the customer
    /// </summary>
    /// <param name="bRagdoll"> Whether to set the customer to ragdoll or not </param>
    public void SetRagdoll(bool bRagdoll)
    {
        m_bIsRagdolling = bRagdoll;

        // switch kinematic state to match ragdoll state
        foreach (Rigidbody rb in m_ragdollRigidbodies)
            rb.isKinematic = !m_bIsRagdolling;

        m_animator.enabled = !m_bIsRagdolling;

        if (m_bHadPath)
            m_agent.isStopped = m_bIsRagdolling;

        if (m_bIsRagdolling)
        {
            StopCoroutine(m_customerAnimator.IdleLoop());
        }

        // deactivate speech bubble if starting ragdoll
        if (m_speechBubbleCanvas.activeSelf
            && m_bIsRagdolling)
            m_speechBubbleCanvas.SetActive(false);
    }

    /// <summary>
    /// Plays the customer's shocked animation
    /// </summary>
    public void Shocked() => m_customerAnimator.Shocked();

    /// <summary>
    /// Plays the customer's cheer animation
    /// </summary>
    public void Cheer() => m_customerAnimator.Cheer();

    /// <summary>
    /// The coroutine for cough up animation and item spawning
    /// </summary>
    /// <param name="coughup"> The prefab of the GameObject to cough up </param>
    /// <param name="timeUntilSpawnObject"> Time between starting the cough up animation and spawning the cough up GameObject </param>
    public IEnumerator CoughUp(GameObject coughup, float timeUntilSpawnObject)
    {
        m_customerAnimator.CoughUp();
        yield return new WaitForSeconds(timeUntilSpawnObject);
        // wait before spawning
        GameObject vomit = Instantiate(coughup, m_coughUpSpawnPoint.position, m_coughUpSpawnPoint.rotation);
        DisableCollision(vomit.GetComponent<Collider>());
        yield return new WaitForSeconds(2f);
        ExitBar();
    }

    /// <summary>
    /// Disables collision between all the ragdoll colliders and the specified collider
    /// </summary>
    /// <param name="colliderToDisableAgainst"> The collider to disable collision against </param>
    private void DisableCollision(Collider colliderToDisableAgainst)
    {
        foreach (Rigidbody rb in m_ragdollRigidbodies)
        {
            Physics.IgnoreCollision(rb.GetComponent<Collider>(), colliderToDisableAgainst);
        }
    }

    /// <summary>
    /// Puts the specified hat on the customer's head
    /// </summary>
    /// <param name="hat"> The hat prefab to wear </param>
    public void SetHat(GameObject hat)
    {
        GameObject newHat = Instantiate(hat, m_hatTransform.position, m_hatTransform.rotation);
        newHat.transform.parent = m_hatTransform;
    }

    public void StopMovement()
    {
        m_agent.isStopped = true;
    }

    private void OnDestroy()
    {
        m_spawner.m_customers.Remove(gameObject);
    }

    public CustomerType GetCustomerType() => m_customerType;
}