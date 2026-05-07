using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public InputActionAsset inputActions;

    public GameObject Vampire;
    public GameObject Nurse;
    public Transform spawnPointVampire;
    public Transform spawnPointNurse;
    void Start()
    {
        var p1 = PlayerInput.Instantiate(Vampire);
        var p2 = PlayerInput.Instantiate(Nurse);

       
        p1.transform.position = spawnPointVampire.position;
        p1.transform.rotation = spawnPointVampire.rotation;

        p2.transform.position = spawnPointNurse.position;
        p2.transform.rotation = spawnPointNurse.rotation;


        // Create separate action instances
        p1.actions = Instantiate(inputActions);
        p2.actions = Instantiate(inputActions);

        // 🔥 IMPORTANT: reassign control scheme AFTER setting actions
        p1.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        p2.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);

        // THEN assign maps
        p1.SwitchCurrentActionMap("VampireC");
        p2.SwitchCurrentActionMap("NurseC");
    }
}