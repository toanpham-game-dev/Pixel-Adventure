using System.Collections;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class FallingPlatformController : MonoBehaviour
{
    [Header("Idle Motion")]
    [SerializeField] private float idleAmplitude;
    [SerializeField] private float idleSpeed;

    [Header("Squash")]
    [SerializeField] private float squashDistance;
    [SerializeField] private float squashSpeed;

    [Header("Bounce")]
    [SerializeField] private float bounceHeight;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private int bounceCount;

    [Header("Respawn")]
    [SerializeField] private float respawnDelay;

    private Rigidbody2D rb;
    private Collider2D col;
    private AnimationController anim;

    private Vector3 startPos;
    private bool triggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<AnimationController>();

        rb.bodyType = RigidbodyType2D.Kinematic;

        startPos = transform.position;
    }

    private void Update()
    {
        if (triggered) return;

        float y = Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;
        transform.position = startPos + new Vector3(0, y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallRoutine());
        }
    }

    private IEnumerator FallRoutine()
    {
        triggered = true;

        transform.position = startPos;

        // squash
        Vector3 down = startPos + Vector3.down * squashDistance;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * squashSpeed;
            transform.position = Vector3.Lerp(startPos, down, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * squashSpeed;
            transform.position = Vector3.Lerp(down, startPos, t);
            yield return null;
        }

        for (int i = 0; i < bounceCount; i++)
            yield return Bounce();

        anim.PlayAnimation("Off");

        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.gravityScale = 10f;

        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        Respawn();
    }

    private IEnumerator Bounce()
    {
        Vector3 up = startPos + Vector3.up * bounceHeight;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.position = Vector3.Lerp(startPos, up, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.position = Vector3.Lerp(up, startPos, t);
            yield return null;
        }
    }

    private void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;

        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = startPos;

        col.enabled = true;

        anim.PlayAnimation("On");

        triggered = false;
    }
}