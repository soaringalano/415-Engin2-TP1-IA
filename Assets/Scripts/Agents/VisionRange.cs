using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null )
        {
            return;
        }
        Worker worker = GetComponentInParent<Worker>();
        if(worker != null)
        {
            worker.SetWorkerState(Worker.STATE_BUILDING);
        }
        TeamOrchestrator._Instance.TryAddCollectible(collectible);
    }
}
