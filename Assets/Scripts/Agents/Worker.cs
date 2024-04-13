using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{

    public const int STATE_WAITING = 2;

    public const int STATE_WORKING = 3;

    public const int STATE_SCOUTING = 4;

    public const int STATE_BUILDING = 5;

    /**
     * added by Mao
     * global worker id
     */
    private static int s_globalWorkerId = 0;

    /**
     * added by Mao
     * identical worker id
     */
    public int m_workerId { get; private set; }

    /**
     * added by Mao
     * state of the worker, default is scouting
     * accumulating means worker is assigned to a resource and have to accumulate at the specified camp
     * in position means worker is at the specified camp
     * waiting means all worker that assigned to the same resource are in position and waiting for their turn
     * scouting means worker is not assigned to any resource and scouting to find more resources
     */
    private int m_workerState = STATE_SCOUTING;

    public void SetWorkerState(int workerState)
    {
        m_workerState = workerState;
    }

    public int GetWorkerState()
    {
        return m_workerState;
    }    

    public bool NeedWait()
    {
        return m_workerState == STATE_WAITING && m_myWaitTime > 0;
    }

    public float m_myWaitTime = 0.0f;

    private const float EXTRACTION_DURATION = 1.0f;

    private const float DEPOSIT_DURATION = 1.0f;

    [SerializeField]
    private float m_radius = 5.0f;

    [SerializeField]
    private Transform m_radiusDebugTransform;

    [SerializeField]
    private ECollectibleType m_collectibleInInventory = ECollectibleType.None;

    [SerializeField]
    private Collectible m_currentExtractingCollectible;

    private bool m_isInDepot = false;

    private bool m_isInExtraction = false;

    // added by Mao, if I am scouting or not
    public bool m_isInScouting = true;

    private float m_currentActionDuration = 0.0f;

    private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius);
    }

    private void Start()
    {
        lock (this)
        {
            m_workerId = s_globalWorkerId;
            s_globalWorkerId++;
        }
        TeamOrchestrator._Instance.WorkersList.Add(this.m_workerId, this);
        Debug.Log("New worker added : " + m_workerId);
    }

    private void FixedUpdate()
    {
        if (m_workerState == STATE_WAITING && m_myWaitTime > 0.0f)
        {
            m_myWaitTime -= Time.fixedDeltaTime;

            return;
        }

        if (m_isInDepot || m_isInExtraction)
        {
            m_currentActionDuration -= Time.fixedDeltaTime;
            if (m_currentActionDuration < 0.0f)
            {
                if (m_isInDepot)
                {
                    DepositResource();
                }
                else
                {
                    GainCollectible();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            m_currentExtractingCollectible = collectible;
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = true;
            //Start countdown to collect it
        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_currentActionDuration = DEPOSIT_DURATION;
            m_isInDepot = true;
            //Start countdown to deposit my current collectible (if it exists)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            if (m_currentExtractingCollectible == collectible)
            {
                m_currentExtractingCollectible = null;
            }
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = false;
        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_isInDepot = false;
            m_currentActionDuration = DEPOSIT_DURATION;
        }
    }

    private void GainCollectible()
    {
        m_collectibleInInventory = m_currentExtractingCollectible.Extract();
        m_isInExtraction = false;
        m_currentExtractingCollectible = null;
    }

    private void DepositResource()
    {
        TeamOrchestrator._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
    }
}