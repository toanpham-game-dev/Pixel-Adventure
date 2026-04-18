using UnityEngine;

public class ChickenGizmos : MonoBehaviour
{
    [SerializeField] private float viewRange;
    [SerializeField] private Color visionColor = Color.yellow;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = visionColor;

        // Draw detection radius
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
