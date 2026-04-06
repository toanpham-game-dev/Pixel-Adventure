using UnityEngine;

public class BlueBirdGizmos : MonoBehaviour
{
    public Vector2 wallCheckSize = new Vector2(0.6f, 0.6f);
    public float wallCheckDistance = 0.5f;

    private void OnDrawGizmos()
    {
        int dir = transform.localScale.x > 0 ? 1 : -1;

        Vector3 origin = transform.position;
        Vector3 castPos = origin + new Vector3(dir * wallCheckDistance, 0, 0);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(castPos, wallCheckSize);

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(origin, castPos);
    }
}