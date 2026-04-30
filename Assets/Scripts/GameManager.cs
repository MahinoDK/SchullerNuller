using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PlayerBehavior characterA;
    public PlayerBehavior characterB;

    private PlayerBehavior current;

    void Start()
    {
        current = characterA;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        current.Move(input, context);
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log("Switching character");
        current.StopMovement();

        current = (current == characterA) ? characterB : characterA;
    }
}