using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void InteractableDelegate();
    private InteractableDelegate interactabelCallback;
    private InteractableDelegate afterFinishedEventCallback;

    [SerializeField] private bool canBeInteracted = false;
    [SerializeField] private Collider2D interactableTriggerCollider;

    private Animator anim;

    public bool b_canBeInteracted { get { return canBeInteracted; } set { canBeInteracted = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void InitializeInteractable(InteractableDelegate interactCallback, InteractableDelegate afterEventCallback)
    {
        interactabelCallback = interactCallback;
        afterFinishedEventCallback = afterEventCallback;
    }

    public void Interacted()
    {
        anim.SetTrigger("Interact");

        if (interactableTriggerCollider)
            interactableTriggerCollider.enabled = false;

        interactabelCallback?.Invoke();
    }

    public void FinishedEventAfterInteract()
    {
        Debug.LogWarning("door is opened!!");
        afterFinishedEventCallback?.Invoke();
    }
}
