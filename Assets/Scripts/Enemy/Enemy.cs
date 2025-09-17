using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void OnEnemyDestroy();
    private OnEnemyDestroy onEnemyDestroyCallback;

    [SerializeField] private Transform spriteRenTransform;
    [SerializeField] private float distanceFromPlayertoAttack;
    [SerializeField] private float speed;
    [SerializeField] private int enemyHP;
    [SerializeField] private int enemyAtkPow;

    [Header("Enemy Points")]
    [SerializeField] private int enemyHitGetPoint;
    [SerializeField] private int enemyDeadGetPoint;

    [Header("Targets")]
    public Collider2D[] targetColliders;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask targetLayer;

    public Enemy_IdleState enemyIdleState { get; private set; }
    public Enemy_WalkState enemyWalkState { get; private set; }
    public Enemy_AttackState enemyAttackState { get; private set; }
    public Enemy_DeadState enemyDeadState { get; private set; }


    public Animator anim { get; private set; }
    private StateMachine stateMachine;
    public bool alive { get; private set; }
    public Player player { get; private set; }


    public float f_DistanceFromPlayerToAttack { get; private set; }
    public float f_Speed { get; private set; }
    public bool attacking { get; private set; }
    //public bool isActivating { get; private set; }

    private bool facingRight = true;
    private Rigidbody2D rb;
    private Collider2D col;
    private float waitForAfterEnemyDied = 0;
    private Coroutine waitForAfterEnemyDiedCoroutine;

    public void InitEnemy(OnEnemyDestroy onEnemyDestroyCallback, float waitForAfterEnemyDied)
    {
        this.onEnemyDestroyCallback = onEnemyDestroyCallback;
        this.waitForAfterEnemyDied = waitForAfterEnemyDied;
    }

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        f_DistanceFromPlayerToAttack = distanceFromPlayertoAttack;
        f_Speed = speed;

        stateMachine = new StateMachine();

        enemyIdleState = new Enemy_IdleState(stateMachine, this, "Idle");
        enemyWalkState = new Enemy_WalkState(stateMachine, this, "Walk");
        enemyAttackState = new Enemy_AttackState(stateMachine, this, "Attack");
        enemyDeadState = new Enemy_DeadState(stateMachine, this, "Dead");
    }

    private void OnEnable()
    {
        alive = true;
        attacking = false;
        //isActivating = false;
    }

    private void Start()
    {
        stateMachine.Initialized(enemyIdleState);
    }

    private void Update()
    {
        if (player != null)
        {
            stateMachine.currentState.Update();

            if (player.alive)
                HandleFlip();
            else
                stateMachine.ChangeState(enemyIdleState);
        }
    }

    public void SetVelocity(float xValue, float yValue)
    {
        rb.velocity = new Vector2(xValue * speed, yValue * speed);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        spriteRenTransform.Rotate(new Vector3(0, 180, 0));
    }

    private void HandleFlip()
    {
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        if (vectorToPlayer.x > 0 && !facingRight && !attacking)
            Flip();
        else if (vectorToPlayer.x < -0 && facingRight && !attacking)
            Flip();
    }

    public void AnimationTriggerAttack(bool attacking)
    {
        this.attacking = attacking;
    }

    public void GetDetectedColliders()
    {
        targetColliders = Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, targetLayer);
        if (targetColliders.Length > 0)
        {
            foreach (var target in targetColliders)
            {
                Player player = target.GetComponent<Player>();
                player?.TakeDamage(enemyAtkPow);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }

    public virtual void TakeDamage(int damage)
    {
        if (alive)
        {
            enemyHP -= damage;
            GameController.instance.AddScore(enemyHitGetPoint);

            if (enemyHP <= 0)
            {
                col.enabled = false;
                alive = false;
                //isActivating = false;
                GameController.instance.AddScore(enemyDeadGetPoint);
                stateMachine.ChangeState(enemyDeadState);
            }
        }
    }

    public void AfterDeathHandle()
    {
        spriteRenTransform.gameObject.SetActive(false);

        if (waitForAfterEnemyDiedCoroutine != null)
            StopCoroutine(waitForAfterEnemyDiedCoroutine);
        waitForAfterEnemyDiedCoroutine = StartCoroutine(EnemyDiedCallbaclCoroutine());
    }

    IEnumerator EnemyDiedCallbaclCoroutine()
    {
        yield return new WaitForSeconds(waitForAfterEnemyDied);
        onEnemyDestroyCallback?.Invoke();
        Destroy(gameObject);
    }
}
