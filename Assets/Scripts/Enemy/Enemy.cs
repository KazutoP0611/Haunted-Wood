using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour, IDamagable
{
    public delegate void OnEnemyDestroy();
    private OnEnemyDestroy onEnemyDestroyCallback;

    [SerializeField] private Transform spriteRenTransform;
    [SerializeField] private float distanceFromPlayertoAttack;
    [SerializeField] private float speed;
    [SerializeField] private int enemyHP;
    [SerializeField] private int enemyAtkPow;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip deadSound;

    [Header("Enemy Points")]
    [SerializeField] private int enemyHitGetPoint;
    [SerializeField] private int enemyDeadGetPoint;

    [Header("Targets")]
    public Collider2D[] targetColliders;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask targetLayer;

    [Header("On Hit Details")]
    [SerializeField] private float hitEffectDuration;
    [SerializeField] private Material hitMaterial;

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
    private Renderer rend;
    private Material originalMaterial;
    private Coroutine waitForAfterEnemyDiedCoroutine;
    private Coroutine hitEffectCoroutine;
    private AudioSource audioSource;

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
        rend = GetComponentInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();

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
        originalMaterial = rend.material;
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

    private void HandleFlip()
    {
        Vector3 vectorToPlayer = player.transform.position - transform.position;

        if (attacking)
            return;

        if (vectorToPlayer.x > 0 && !facingRight)
            Flip();
        else if (vectorToPlayer.x < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        spriteRenTransform.Rotate(new Vector3(0, 180, 0));
    }

    public void AnimationTriggerAttack(bool attacking)
    {
        this.attacking = attacking;
    }

    public void PlayAttackSound()
    {
        if (audioSource.clip != attackSound)
            audioSource.clip = attackSound;
        audioSource.Play(0);
    }

    public void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(deadSound, transform.position);
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

    public virtual void OnTakeDamage(int damage)
    {
        if (alive)
        {
            AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);

            if (hitEffectCoroutine != null)
                StopCoroutine(hitEffectCoroutine);

            hitEffectCoroutine = StartCoroutine(OnHitEffect());

            enemyHP -= damage;
            GameController.instance.AddScore(enemyHitGetPoint);

            if (enemyHP <= 0)
            {
                audioSource.Stop();

                col.enabled = false;
                alive = false;
                //isActivating = false;
                GameController.instance.AddScore(enemyDeadGetPoint);
                stateMachine.ChangeState(enemyDeadState);
            }
        }
    }

    IEnumerator OnHitEffect()
    {
        rend.material = hitMaterial;
        yield return new WaitForSeconds(hitEffectDuration);
        rend.material = originalMaterial;
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
