using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class FireController : MonoBehaviour
{
    private AnimationController _anim;
    private bool _isFiring;

    [SerializeField] private Collider2D _fireTrigger;
    [SerializeField] private float _stopFiringTime;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
        _isFiring = false;
    }

    private void Start()
    {
        _fireTrigger.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFiring) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            _anim.PlayThenTransition("Hit", "On");
            StartCoroutine(StopFiring());
        }
    }

    public void OpenFire()
    {
        _fireTrigger.GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator StopFiring()
    {
        yield return new WaitForSeconds(_stopFiringTime);

        _fireTrigger.enabled = false;

        _isFiring = false;

        _anim.PlayAnimation("Off");
    }
}
