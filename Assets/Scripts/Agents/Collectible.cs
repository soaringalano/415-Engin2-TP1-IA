using MBT;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /**
     * added by Mao
     * global collectible id
     */
    private static int s_globalCollectibleId = 0;

    private const float COOLDOWN = 5.0f;
    private float m_currentCooldown = 0.0f;

    /**
     * add by Mao
     * identical collectible id
     */
    public int m_collectibleId { get; private set; }

    private void Start()
    {
        // assign an id and increase global id
        lock (this)
        {
            m_collectibleId = s_globalCollectibleId;
            s_globalCollectibleId++;
        }
    }

    private void OnDestroy()
    {
        
    }

    public ECollectibleType Extract()
    {
        if (m_currentCooldown < 0.0f)
        {
            Debug.Log("Collectible extracted. Last extraction was: " + (COOLDOWN - m_currentCooldown).ToString() + " seconds ago");
            m_currentCooldown = COOLDOWN;
            return ECollectibleType.Regular;
        }

        //We have been extracted twice under 5 seconds
        DispatchCenter._Instance.ReleaseCollectible(m_collectibleId);
        TeamOrchestrator._Instance.KnownCollectibles.Remove(this.m_collectibleId);
        Destroy(gameObject);
        return ECollectibleType.Special;
    }

    private void FixedUpdate()
    {
        m_currentCooldown -= Time.fixedDeltaTime;
    }
}

public enum ECollectibleType
{
    Regular,
    Special,
    None
}