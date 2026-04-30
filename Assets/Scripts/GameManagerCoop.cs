using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerCoop : MonoBehaviour
{
   
    public PlayerBehavior characterA;
    public PlayerBehavior characterB;

    public void OnMoveA(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        characterA.Move(input, context);
    }

    public void OnMoveB(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        characterB.Move(input, context);
    }
}
    