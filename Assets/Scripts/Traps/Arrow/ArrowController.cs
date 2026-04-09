using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class ArrowController : MonoBehaviour
{
    private AnimationController _anim;
    private SpriteRenderer _sprite;
    private Collider2D _col;

    [SerializeField] private float _respawnTime;
    [SerializeField] private float _jumpForce;

    private bool _isHit;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
        _sprite = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isHit) return;

        if (collision.CompareTag("Player"))
        {
            OnHit(collision.gameObject);
        }
    }

    private void OnHit(GameObject player)
    {
        _isHit = true;

        player.GetComponent<IPlayerMovement>().ExternalJump(_jumpForce);

        _anim.PlayAnimation("Hit");

        StartCoroutine(RespawnArrow());
    }

    private IEnumerator RespawnArrow()
    {
        _sprite.enabled = false;
        _col.enabled = false;

        yield return new WaitForSeconds(_respawnTime);

        _sprite.enabled = true;
        _col.enabled = true;

        _anim.PlayAnimation("Idle");

        _isHit = false;
    }
}