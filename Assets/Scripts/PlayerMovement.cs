using UnityEngine;
using UnityEngine.InputSystem;//to talk to input system that controls the movement/actions of player InputActions
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayermovementInputs")]
    [SerializeField] private float moveSpeed = 5f; //serialize private, only this script can access it but shows in inspecter anyways:3
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Animator")]
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //get component of rb attached to this = the player
        animator = GetComponent<Animator>(); //get component of animator attached to this = the player
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed; //velocity of the player is the input multiplied by the speed
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

        moveInput =context.ReadValue<Vector2>(); //we read the value of the input action, which is a vector2, and we store it in moveInput 
        //in inputactions it says we move with wasd and arrows

        animator.SetFloat("CurrentInputX", moveInput.x); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
        animator.SetFloat("CurrentInputY", moveInput.y); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
    }
}
