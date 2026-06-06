using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [Header("Interactable Info")]
    public InteractableType interactableType = InteractableType.None;

    public bool HasBeenActivated { get; private set; } = false;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Interact(PlayerInteraction player)
    {
        PuzzleManager.Instance.HandleInteraction(player, this);
    }

    public void SetLit()
    {
        if (animator != null)
        {
            animator.SetBool("isLit", true);
        }
    }

    public void SetUnlit()
    {
        if (animator != null)
        {
            animator.SetBool("isLit", false);
        }
    }

    public void ChangeColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }

    public void MarkActivated()
    {
        HasBeenActivated = true;
    }

    public void MarkUnActivated() 
    {
        HasBeenActivated = false;

        if(animator != null)
        {
            animator.SetBool("isLit", false);
        }
    }

    public void TriggerAnimation(string triggerName)
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
}