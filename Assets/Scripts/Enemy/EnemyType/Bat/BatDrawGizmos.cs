using UnityEngine;

public class BatDrawGizmos : MonoBehaviour
{
    [SerializeField] private float _viewRange;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewRange);
    }
}
