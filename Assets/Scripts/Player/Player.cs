using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject bowParent;
    [SerializeField] private int playerMaxHP;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip keyPickupSound;

    [Header("Weapon Settings")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrowSpawnOffSet;


    public StateMachine stateMachine { get; private set; }
    public bool alive { get; private set; }
    public Vector2 moveInput { get; private set; }


    public Player_IdleState playerIdleState { get; private set; }
    public Player_WalkState playerWalkState { get; private set; }
    public Player_DeadState playerDeadState { get; private set; }


    private PlayerInputSet input;
    public Animator anim { get; private set; }

    private Rigidbody2D rb;
    private Vector2 mousePosition;
    private Camera cam;
    private bool ableToInteract = false;
    private bool hasKingKey = false;
    private Interactable currentInteractable;
    private int currentHP;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine();

        playerIdleState = new Player_IdleState(stateMachine, this, "idle");
        playerWalkState = new Player_WalkState(stateMachine, this, "walk");
        playerDeadState = new Player_DeadState(stateMachine, this, "dead");

        input = new PlayerInputSet();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        stateMachine.Initialized(playerIdleState);
        currentHP = playerMaxHP;
        GameController.instance.UpdateHealthPoint((float)currentHP / playerMaxHP);
        SetActiveInteractable(false);
    }

    private void OnEnable()
    {
        alive = true;
        input.Enable();

        input.PlayerControl.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.PlayerControl.Move.canceled += ctx => moveInput = Vector2.zero;

        input.PlayerControl.Aim.performed += value => mousePosition = cam.ScreenToWorldPoint(value.ReadValue<Vector2>());

        input.PlayerControl.Interact.performed += interact => OnInteract();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        Movement();
        HandleMouse();

        stateMachine.currentState.Update();
    }

    private void Movement()
    {
        if (alive)
            SetVelocity(moveInput.x * movementSpeed, moveInput.y * movementSpeed);
    }

    public void SetVelocity(float xValue, float yValue)
    {
        rb.velocity = new Vector2 (xValue, yValue);
    }

    private void HandleMouse()
    {
        if (alive)
        {
            bowParent.transform.up = new Vector2(transform.position.x, transform.position.y) - mousePosition;

            if (input.PlayerControl.Shoot.WasPerformedThisFrame())
                Instantiate(arrowPrefab, bowParent.transform.position + (bowParent.transform.up * arrowSpawnOffSet), bowParent.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Key":
                AudioSource.PlayClipAtPoint(keyPickupSound, transform.position);
                GameController.instance.SetActiveKeyIcon(true);
                hasKingKey = true;
                Destroy(collision.gameObject);
                break;
            case "Heal":
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                HealingBox healingBox = collision.GetComponent<HealingBox>();
                Heal(healingBox.GetHealPoint());
                Destroy(collision.gameObject);
                break;
            case "Interactable":
                currentInteractable = collision.GetComponent<Interactable>();
                if (hasKingKey)
                    SetActiveInteractable(true);
                else
                    GameController.instance.ShowKingRoomText();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
            SetActiveInteractable(false);
    }

    private void Heal(int healPoint)
    {
        currentHP = Mathf.Clamp(currentHP + healPoint, 0, playerMaxHP);
        GameController.instance.UpdateHealthPoint((float)currentHP / playerMaxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        GameController.instance.UpdateHealthPoint((float)currentHP / playerMaxHP);

        if (currentHP <= 0)
        {
            alive = false;
            bowParent.SetActive(false);
            stateMachine.ChangeState(playerDeadState);
        }
    }

    private void SetActiveInteractable(bool active)
    {
        if (!active)
            currentInteractable = null;
        ableToInteract = active;
        GameController.instance.SetActiveInteractText(active);
    }

    private void OnInteract()
    {
        if (ableToInteract)
            currentInteractable?.Interacted();
    }

    public void SetEnableInput(bool active)
    {
        if (active)
            input.Enable();
        else
            input.Disable();
    }

    private void OnDead()
    {
        GameController.instance.OnGameOver();
    }
}
