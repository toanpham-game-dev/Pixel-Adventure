using UnityEngine;
using static UnityEditor.Tilemaps.RuleTileTemplate;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private PlayerController _player;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    [SerializeField] private LayerMask _groundLayer;

    public bool IsFacingRight { get; private set; } = true;
    public float LastOnGroundTime { get; private set; }

    private void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        #endregion

        #region INPUT CHECKS
        CheckDirectionToFace(_player.Input.Move > 0, _player.Input.Move != 0);
        #endregion

        #region COLLISION CHECKS
        //Ground Check
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
            LastOnGroundTime = 0.1f;
        #endregion
    }

    public void Run()
    {
        float targetSpeed = _player.Input.Move * _player.Data.runMaxSpeed;

        float accelRate;
        if (LastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _player.Data.runAccelAmount : _player.Data.runDeccelAmount;
        }
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _player.Data.runAccelAmount * _player.Data.accelInAir : _player.Data.runDeccelAmount * _player.Data.deccelInAir;

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (_player.Data.doConserveMomentum &&
            Mathf.Abs(_player.RB.linearVelocity.x) > Mathf.Abs(targetSpeed) &&
            Mathf.Sign(_player.RB.linearVelocity.x) == Mathf.Sign(targetSpeed) &&
            Mathf.Abs(targetSpeed) > 0.01f &&
            LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        float speedDif = targetSpeed - _player.RB.linearVelocity.x;
        float movement = speedDif * accelRate;
        _player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }


    private void Jump()
    {

    }

    private void Slide()
    {

    }

    private void DoubleJump()
    {

    }

    private void WallJump()
    {

    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight, bool isMoving)
    {
        if (isMovingRight != IsFacingRight && isMoving)
            Turn();
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
}
