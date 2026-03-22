using UnityEngine;
using UnityEngine.InputSystem;//to talk to input system that controls the movement/actions of player InputActions
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInteraction playerInteraction;

    [Header("PlayermovementInputs")]
    [SerializeField] private float moveSpeed = 5f; //serialize private, only this script can access it but shows in inspecter anyways:3
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Animator")]
    private Animator animator;

    [Header("Hold Position")]
    [SerializeField] private Transform holdPosition; //where the player holds the item
    [SerializeField] private float holdDistance = 0.4f; //how far the player can hold the item

    private Vector2 lastFacingDirection = Vector2.down; //the last direction the player was facing, default is down

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //get component of rb attached to this = the player
        animator = GetComponent<Animator>(); //get component of animator attached to this = the player
        playerInteraction = GetComponent<PlayerInteraction>(); //get component of player interaction attached to this = the player
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed; //velocity of the player is the input multiplied by the speed

        UpdateHoldPosition(); //Call update the position of the hold position
    }

    public void Move(InputAction.CallbackContext context) //we find input actions on player, and Move is the name for the input action, and we get the context of the input action, which is the value of the input action
    {
        moveInput = context.ReadValue<Vector2>(); //we read the value of the input action, which is a vector2, and we store it in moveInput, not relevant for animation but for the holdPosition direction of item held

        animator.SetBool("isWalking", moveInput != Vector2.zero);

        if (moveInput != Vector2.zero) //if the moveInput is not zero, which means we are moving, we set the lastFacingDirection to the moveInput, which is the direction we are moving
        {
            lastFacingDirection = moveInput.normalized; //we normalize the moveInput to get the direction of movement, which is the lastFacingDirection, this is used for the holdPosition direction of item held
        }

        if (context.canceled) //let go off button... if the input action is canceled, we set the bool parameter of the animator to false, which means we are not walking anymore
        {
            //animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastFacingDirection.x);
            animator.SetFloat("LastInputY", lastFacingDirection.y);
        }

        animator.SetFloat("CurrentInputX", moveInput.x); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
        animator.SetFloat("CurrentInputY", moveInput.y); //we set the float parameter of the animator to the x value of the moveInput, which is the horizontal movement)
    }

    private void UpdateHoldPosition()
    {
        if (holdPosition == null) return; //if the hold position is null, we return, which means we don't update the hold position
        Vector2 directionToUse;

        if (moveInput != Vector2.zero) //if we are moving, we use the moveInput as the direction to use for the hold position
        {
            directionToUse = moveInput; //we use the moveInput as the direction to use for the hold position
        }
        else //if we are not moving, we use the lastFacingDirection as the direction to use for the hold position
        {
            directionToUse = lastFacingDirection; //we use the lastFacingDirection as the direction to use for the hold position
        }

        directionToUse = SnapToFourDirections(directionToUse);

        if (directionToUse == Vector2.down)
        {
            holdPosition.localPosition = new Vector3(0.50f, 0.4f, 0f);
        }
        else if (directionToUse == Vector2.up)
        {
            holdPosition.localPosition = new Vector3(0.50f, 0.4f, 0f);
        }
        else if (directionToUse == Vector2.right)
        {
            holdPosition.localPosition = new Vector3(0.25f, 0.4f, 0f);
        }
        else if (directionToUse == Vector2.left)
        {
            holdPosition.localPosition = new Vector3(-0.25f, 0.4f, 0f);
        }

        UpdateHeldItemSorting(directionToUse);
    }

     private Vector2 SnapToFourDirections(Vector2 direction)
      {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    private void UpdateHeldItemSorting(Vector2 direction)
    {
        if (playerInteraction == null) return;

        Grabbable heldItem = playerInteraction.GetHeldItem();
        if (heldItem == null) return;

        if (direction == Vector2.down)
        {
            heldItem.SetSortingOrder(2); // in front of player
        }
        else
        {
            heldItem.SetSortingOrder(0); // behind player
        }
    }
}
