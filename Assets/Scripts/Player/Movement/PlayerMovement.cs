/*
    Created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
    (Dash removed cleanly)
*/
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController _playerController;

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsSliding { get; private set; }

    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    // Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    // Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    // Air Jump
    private int _airJumpsLeft;
    private int _jumpsUsed;
    #endregion

    #region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    #endregion

    private void Start()
    {
        SetGravityScale(_playerController.Data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        if (_playerController.Input.Move != 0)
            CheckDirectionToFace(_playerController.Input.Move > 0);

        if (_playerController.Input.Move == 0 && !IsSliding && LastOnGroundTime > 0)
        {
            _playerController.Anim.PlayAnimation("Idle");
        }
        else if (_playerController.Input.Move != 0 && !IsSliding && LastOnGroundTime > 0)
        {
            _playerController.Anim.PlayAnimation("Run");
        }

        if (_playerController.RB.linearVelocity.y < 0 && LastOnGroundTime < 0 && !IsSliding && _airJumpsLeft == _playerController.Data._maxAirJumps)
        {
            _playerController.Anim.PlayAnimation("Fall");
        }

        if (_playerController.Input.JumpDown)
        {
            OnJumpInput();
        }

        if (_playerController.Input.JumpUp)
        {
            OnJumpUpInput();
        }

        #endregion

        #region COLLISION CHECKS
        if (!IsJumping)
        {
            // Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            {
                LastOnGroundTime = _playerController.Data.coyoteTime;
            }

            // Right wall
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _wallLayer) && IsFacingRight)
                 || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _wallLayer) && !IsFacingRight)) && !IsWallJumping)
                LastOnWallRightTime = _playerController.Data.coyoteTime;

            // Left wall
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _wallLayer) && !IsFacingRight)
                 || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _wallLayer) && IsFacingRight)) && !IsWallJumping)
                LastOnWallLeftTime = _playerController.Data.coyoteTime;

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region JUMP / WALL JUMP CHECKS
        if (IsJumping && _playerController.RB.linearVelocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > _playerController.Data.wallJumpTime)
            IsWallJumping = false;

        // If we're on the ground, reset jumps
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            _isJumpFalling = false;
            _airJumpsLeft = _playerController.Data._maxAirJumps;
            _jumpsUsed = 0;
        }

        // Jump
        if (CanJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            IsWallJumping = false;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }
        // Wall Jump
        else if (CanWallJump() && LastPressedJumpTime > 0)
        {
            IsWallJumping = true;
            IsJumping = false;
            _isJumpCut = false;
            _isJumpFalling = false;

            _wallJumpStartTime = Time.time;
            _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

            WallJump(_lastWallJumpDir);
        }
        // Air Jump
        else if (CanAirJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            AirJump();
        }
        #endregion

        #region SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _playerController.Input.Move < 0) || (LastOnWallRightTime > 0 && _playerController.Input.Move > 0)))
        {
            IsSliding = true;
            _airJumpsLeft = _playerController.Data._maxAirJumps;
            _jumpsUsed = 0;
        }
        else
        {
            IsSliding = false;
        }
        #endregion

        #region GRAVITY
        if (IsSliding)
        {
            SetGravityScale(0);
        }
        else if (_isJumpCut)
        {
            SetGravityScale(_playerController.Data.gravityScale * _playerController.Data.jumpCutGravityMult);
            _playerController.RB.linearVelocity = new Vector2(_playerController.RB.linearVelocity.x, Mathf.Max(_playerController.RB.linearVelocity.y, -_playerController.Data.maxFallSpeed));
        }
        else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(_playerController.RB.linearVelocity.y) < _playerController.Data.jumpHangTimeThreshold)
        {
            SetGravityScale(_playerController.Data.gravityScale * _playerController.Data.jumpHangGravityMult);
        }
        else if (_playerController.RB.linearVelocity.y < 0)
        {
            SetGravityScale(_playerController.Data.gravityScale * _playerController.Data.fallGravityMult);
            _playerController.RB.linearVelocity = new Vector2(_playerController.RB.linearVelocity.x, Mathf.Max(_playerController.RB.linearVelocity.y, -_playerController.Data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(_playerController.Data.gravityScale);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (IsWallJumping)
            Run(_playerController.Data.wallJumpRunLerp);
        else
            Run(1f);

        if (IsSliding)
            Slide();
    }

    #region INPUT CALLBACKS
    public void OnJumpInput()
    {
        LastPressedJumpTime = _playerController.Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale) => _playerController.RB.gravityScale = scale;
    #endregion

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        float targetSpeed = _playerController.Input.Move * _playerController.Data.runMaxSpeed;

        float accelRate = (LastOnGroundTime > 0)
            ? ((Mathf.Abs(targetSpeed) > 0.01f) ? _playerController.Data.runAccelAmount : _playerController.Data.runDeccelAmount)
            : ((Mathf.Abs(targetSpeed) > 0.01f) ? _playerController.Data.runAccelAmount * _playerController.Data.accelInAir : _playerController.Data.runDeccelAmount * _playerController.Data.deccelInAir);

        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(_playerController.RB.linearVelocity.y) < _playerController.Data.jumpHangTimeThreshold)
        {
            accelRate *= _playerController.Data.jumpHangAccelerationMult;
            targetSpeed *= _playerController.Data.jumpHangMaxSpeedMult;
        }

        if (_playerController.Data.doConserveMomentum && Mathf.Abs(_playerController.RB.linearVelocity.x) > Mathf.Abs(targetSpeed)
            && Mathf.Sign(_playerController.RB.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }

        float speedDif = targetSpeed - _playerController.RB.linearVelocity.x;
        float movement = speedDif * accelRate;
        _playerController.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);  
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = _playerController.Data.jumpForce;
        if (_playerController.RB.linearVelocity.y < 0)
            force -= _playerController.RB.linearVelocity.y;

        _playerController.RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        _jumpsUsed++;
        _playerController.Anim.PlayAnimation("Jump");
    }

    private void WallJump(int dir)
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        Vector2 force = new Vector2(_playerController.Data.wallJumpForce.x, _playerController.Data.wallJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(_playerController.RB.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= _playerController.RB.linearVelocity.x;

        if (_playerController.RB.linearVelocity.y < 0)
            force.y -= _playerController.RB.linearVelocity.y;

        _playerController.RB.AddForce(force, ForceMode2D.Impulse);

        _playerController.Anim.PlayAnimation("Jump");
        _jumpsUsed++;
    }

    private void AirJump()
    {
        LastPressedJumpTime = 0;
        _airJumpsLeft--;

        float baseForce = _playerController.Data.jumpForce * _playerController.Data._airJumpForceMult;

        // Cancel downward momentum
        Vector2 vel = _playerController.RB.linearVelocity;
        if (vel.y < 0) vel.y = 0;
        _playerController.RB.linearVelocity = vel;

        _playerController.RB.AddForce(Vector2.up * baseForce, ForceMode2D.Impulse);

        _playerController.Anim.PlayThenTransition("Double_Jump", "Fall");
    }
    #endregion

    #region OTHER MOVEMENT METHODS
    private void Slide()
    {
        if (_playerController.RB.linearVelocity.y > 0)
            _playerController.RB.AddForce(-_playerController.RB.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);

        float speedDif = _playerController.Data.slideSpeed - _playerController.RB.linearVelocity.y;
        float movement = speedDif * _playerController.Data.slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        _playerController.RB.AddForce(movement * Vector2.up);

        _playerController.Anim.PlayAnimation("Slide");
    }
    #endregion

    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
               (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanAirJump()
    {
        if (!_playerController.Data._allowDoubleJump) return false;

        // Air jump if we have jumps left and we're not on the ground or wall jumping
        return LastOnGroundTime <= 0 && !IsWallJumping && _airJumpsLeft > 0 && _jumpsUsed >= 1;
    }

    private bool CanJumpCut() => IsJumping && _playerController.RB.linearVelocity.y > 0;
    private bool CanWallJumpCut() => IsWallJumping && _playerController.RB.linearVelocity.y > 0;

    public bool CanSlide()
    {
        return LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0;
    }
    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}
