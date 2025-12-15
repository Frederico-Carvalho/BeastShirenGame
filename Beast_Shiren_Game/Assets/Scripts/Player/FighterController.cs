using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FighterController : MonoBehaviour
{
    //MOVEMENT
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private Rigidbody rb;

    //GROUND CHECK 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    //ANIMATION
    [SerializeField] private SpriteAnimator animator;


    //ATTACKS
    [SerializeField] private AttackDataSO lightAttack;
    [SerializeField] private AttackDataSO heavyAttack;
    [SerializeField] private AttackDataSO jumpAttack;
    [SerializeField] private Hitbox hitbox;

    private AttackDataSO currentAttack;
    private int attackFrame;

    //OPPONENT DIRECTION
    [SerializeField] private Transform opponent;

    //STATE
    private enum State { Idle, Walk, Jump, AirDash, Attack }
    private State state = State.Idle;

    //HP
    [SerializeField] private int maxHealth = 100;
    private int health;
    public int Health { get { return health; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        hitbox.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckGround();
        HandleInput();
        HandleAirDash();
        HandleAirDashInput();
        HandleAttack();
        Animate();
    }

    private void FixedUpdate()
    {
        if (state == State.Walk)
        {
            float input = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector3(input * runSpeed, rb.linearVelocity.y, 0f);
        }
    }

    //  INPUT
    private void HandleInput()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (state == State.Idle || state == State.Walk)
        {
            if (isGrounded)
                state = input != 0f ? State.Walk : State.Idle;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isGrounded)
                StartAttack(lightAttack);
            else
                StartAttack(jumpAttack);
        }

        if (Input.GetKeyDown(KeyCode.K) && isGrounded)
        {
            StartAttack(heavyAttack);
        }
    }

    //Walk toward/away from opponent
    private bool IsMovingForward(float input)
    {
        float directionToOpponent = opponent.position.x - transform.position.x;
        return Mathf.Sign(directionToOpponent) == Mathf.Sign(input);
    }


    // AIR DASH
    [SerializeField] private float airDashSpeed = 8f;
    [SerializeField] private float airDashDuration = 0.2f;
    [SerializeField] private float doubleTapTime = 0.25f;

    private bool airDashUsed;
    private float airDashTimer;
    private int airDashDirection;
    private float lastLeftTap;
    private float lastRightTap;

    private void HandleAirDashInput()
    {
        if (isGrounded) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastLeftTap <= doubleTapTime)
                TryStartAirDash(-1);
            lastLeftTap = Time.time;
            UnityEngine.Debug.Log("Key A/D pressed in air");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastRightTap <= doubleTapTime)
                TryStartAirDash(1);
            lastRightTap = Time.time;
            UnityEngine.Debug.Log("Key A/D pressed in air");
        }
        
    }
    // AIR DASH STARTER
    private void TryStartAirDash(int direction)
    {
        if (airDashUsed) return;
        if (state != State.Jump) return;

        airDashUsed = true;
        airDashDirection = direction;
        airDashTimer = airDashDuration;

        rb.linearVelocity = Vector3.zero;
        state = State.AirDash;
    }
    // AIR DASH HANDLER
    private void HandleAirDash()
    {
        if (state != State.AirDash) return;

        airDashTimer -= Time.fixedDeltaTime;

        rb.linearVelocity = new Vector3(
            airDashDirection * airDashSpeed,
            0f,
            0f
        );

        if (airDashTimer <= 0f)
        {
            state = State.Jump;
        }
    }

    // JUMP 
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        state = State.Jump;
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
        {
            airDashUsed = false;
        }

        if (isGrounded && rb.linearVelocity.y <= 0f && state == State.Jump)
        {
            state = State.Idle;
        }
    }

    // ATTACK
    private void StartAttack(AttackDataSO attack)
    {
        if (state == State.Attack) return;

        currentAttack = attack;
        attackFrame = 0;
        state = State.Attack;
    }

    private void HandleAttack()
    {
        if (state != State.Attack) return;

        attackFrame++;

        if (attackFrame == currentAttack.Startup)
        {
            hitbox.Damage = currentAttack.Damage;
            hitbox.gameObject.SetActive(true);
        }

        if (attackFrame == currentAttack.Startup + currentAttack.Active)
        {
            hitbox.gameObject.SetActive(false);
        }

        if (attackFrame >= currentAttack.TotalFrames)
        {
            state = isGrounded ? State.Idle : State.Jump;
        }
    }
    // ANIMATION
    private void Animate()
    {
        if (state == State.Idle) animator.PlayIdle();
        if (state == State.Walk) animator.PlayWalk();
        if (state == State.Jump) animator.PlayJump();

        if (state == State.Attack)
        {
            if (currentAttack == lightAttack) animator.PlayLight();
            if (currentAttack == heavyAttack) animator.PlayHeavy();
            if (currentAttack == jumpAttack) animator.PlayJump();
        }
        if (state == State.Walk)
        {
            float input = Input.GetAxisRaw("Horizontal");

            if (IsMovingForward(input))
                animator.PlayWalkForward();
            else
                animator.PlayWalkBackward();
        }
        if (state == State.AirDash)
        {
            if (IsMovingForward(airDashDirection))
                animator.PlayAirDashForward();
            else
                animator.PlayAirDashBackward();
        }
    }

    //  DAMAGE 
    public void TakeHit(int damage)
    {
        health -= damage;
        UnityEngine.Debug.Log("HP: " + health);
    }
}
