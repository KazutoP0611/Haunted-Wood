using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.UI.Image;

public class Enemy : MonoBehaviour, IDamagable
{
    public delegate void OnEnemyDestroy();
    private OnEnemyDestroy onEnemyDestroyCallback;

    [SerializeField] private Transform spriteRenTransform;
    public float speed;
    [SerializeField] private int enemyHP;
    [SerializeField] private int enemyAtkPow;

    [Header("Attack Details")]
    public float distanceFromPlayertoAttack;
    public float minRangeToPlayertoAttack;
    public Vector2 backOffVelocity;
    public Vector2 attackOffset;

    [Header("VFX Details")]
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
    public Enemy_BattleState enemyBattleState { get; private set; }
    public Enemy_AttackState enemyAttackState { get; private set; }
    public Enemy_DeadState enemyDeadState { get; private set; }


    private StateMachine stateMachine;
    public Animator anim { get; private set; }
    public bool alive { get; private set; }
    public Player player { get; private set; }
    public bool attacking { get; private set; }
    public bool b_facingRight { get => facingRight; }
    public Rigidbody2D rb { get; private set; }

    private bool facingRight = true;
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

        stateMachine = new StateMachine();

        enemyIdleState = new Enemy_IdleState(stateMachine, this, "Idle");
        enemyWalkState = new Enemy_WalkState(stateMachine, this, "Walk");
        enemyBattleState = new Enemy_BattleState(stateMachine, this, "Walk");
        enemyAttackState = new Enemy_AttackState(stateMachine, this, "Attack");
        enemyDeadState = new Enemy_DeadState(stateMachine, this, "Dead");
    }

    private void OnEnable()
    {
        alive = true;
        attacking = false;
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

            if (!player.alive)
                stateMachine.ChangeState(enemyIdleState);
        }
    }

    public void SetVelocity(float xValue, float yValue)
    {
        rb.velocity = new Vector2(xValue * speed, yValue * speed);
        HandleFlip();
    }

    public void BackOff()
    {
        rb.velocity = new Vector2(backOffVelocity.x * -DirectionToPlayer(), backOffVelocity.y);
    }

    private void HandleFlip()
    {
        if (attacking)
            return;

        if (DirectionToPlayer() > 0 && !facingRight)
            Flip();
        else if (DirectionToPlayer() < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        spriteRenTransform.Rotate(new Vector3(0, 180, 0));
    }

    public float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return (player.transform.position - transform.position).magnitude;
    }

    public int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.transform.position.x > transform.position.x ? 1 : -1;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.CircleCast(targetCheck.position, targetCheckRadius, Vector2.right, 0, targetLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
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

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        //Gizmos.DrawLine(targetCheck.position, targetCheck.position + Vector3.right * 2.0f);
    }

    public virtual void OnTakeDamage(int damage)
    {
        if (alive)
        {
            AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);

            HitEffect();

            enemyHP -= damage;
            GameplaySceneController.instance.AddScore(enemyHitGetPoint);

            if (enemyHP <= 0)
            {
                audioSource.Stop();

                col.enabled = false;
                alive = false;
                //isActivating = false;
                GameplaySceneController.instance.AddScore(enemyDeadGetPoint);
                stateMachine.ChangeState(enemyDeadState);
            }
        }
    }

    private void HitEffect()
    {
        if (hitEffectCoroutine != null)
            StopCoroutine(hitEffectCoroutine);

        hitEffectCoroutine = StartCoroutine(OnHitEffect());
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
