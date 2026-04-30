using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCoop : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    public void Move(Vector2 input, InputAction.CallbackContext context)
    {
        rb.linearVelocity = input * moveSpeed;

        animator.SetBool("isWalking", input != Vector2.zero);

        if (context.canceled)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }

        animator.SetFloat("CurrentInputX", input.x);
        animator.SetFloat("CurrentInputY", input.y);
    }
}
