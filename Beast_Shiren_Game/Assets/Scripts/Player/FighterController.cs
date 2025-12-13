using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody))]
public class FighterController : MonoBehaviour
{
    // MOVIMENTO 
    [SerializeField] private float runSpeed = 5f;
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }

    [SerializeField] private Rigidbody rb;

    // ESTADOS 
    private enum FighterState { Idle, Walk, Jump, Attack }
    [SerializeField] private FighterState state = FighterState.Idle;

    //  ANIMAÇÃO 
    [SerializeField] private SpriteAnimator animator;

    // ATAQUES 
    [SerializeField] private AttackDataSO lightAttack;
    [SerializeField] private Hitbox hitbox;
    private int attackFrame;
    private AttackDataSO currentAttack;

    // VIDA 
    [SerializeField] private int maxHealth = 100;
    private int health;
    public int Health { get { return health; } private set { health = value; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Health = maxHealth;
        hitbox.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleState();
        AnimateState();
    }

    private void FixedUpdate()
    {
        if (state == FighterState.Walk)
        {
            Move();
        }
        else if (state != FighterState.Attack)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    //ESTADOS
    private void HandleState()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (state == FighterState.Idle || state == FighterState.Walk)
        {
            if (input != 0f)
                state = FighterState.Walk;
            else
                state = FighterState.Idle;
        }

        if (Input.GetKeyDown(KeyCode.J) && state != FighterState.Attack)
        {
            state = FighterState.Attack;
            currentAttack = lightAttack;
            attackFrame = 0;
        }

        if (state == FighterState.Attack)
        {
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
                state = FighterState.Idle;
            }
        }
    }

    //MOVIMENTO 
    private void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector3(input * runSpeed, rb.linearVelocity.y, 0f);
    }

    //ANIMAÇÃO
    private void AnimateState()
    {
        switch (state)
        {
            case FighterState.Idle:
                animator.PlayIdle();
                break;
            case FighterState.Walk:
                animator.PlayWalk();
                break;
            case FighterState.Attack:
                animator.PlayAttack();
                break;
        }
    }

    // DANO 
    public void TakeHit(int damage)
    {
        Health -= damage;
        Debug.Log("HP: " + Health);
    }
}
