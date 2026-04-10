using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float speed = 2f;

    private PlayerMovement _playerMovement;
    private Vector3 moveDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>();
        targetPos = pointB.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, pointA.position) < 0.05f)
        {
            targetPos = pointB.position;
            DirectionCalculate();
        }
        if (Vector2.Distance(transform.position, pointB.position) < 0.05f)
        {
            targetPos = pointA.position;
            DirectionCalculate();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerMovement.IsOnPlatform = true;
            _playerMovement.platformRb = rb;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerMovement.IsOnPlatform = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}