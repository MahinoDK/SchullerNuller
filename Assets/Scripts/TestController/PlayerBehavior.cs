using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context) //we find input actions on player, and Move is the name for the input action, and we get the context of the input action, which is the value of the input action
    {
        animator.SetBool("isWalking", true);

        if (context.canceled) //let go off button... if the input action is canceled, we set the bool parameter of the animator to false, which means we are not walking anymore
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>(); //we read the value of the input action, which is a vector2, and we store it in moveInput 
        //in inputactions it says we move with wasd and arrows

        animator.SetFloat("CurrentInputX", moveInput.x); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
        animator.SetFloat("CurrentInputY", moveInput.y); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
    }
}
