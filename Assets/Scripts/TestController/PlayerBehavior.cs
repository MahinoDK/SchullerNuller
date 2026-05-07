using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 moveInput, InputAction.CallbackContext context)
    {
        rb.linearVelocity = moveInput * moveSpeed;

        animator.SetBool("isWalking", moveInput != Vector2.zero);

        if (context.canceled)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        animator.SetFloat("CurrentInputX", moveInput.x);
        animator.SetFloat("CurrentInputY", moveInput.y);
    }
    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;

        // Optional: stop animation cleanly
        animator.SetBool("isWalking", false);
    }
}