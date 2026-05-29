using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public InputActionAsset inputActions;

    public GameObject Vampire;
    public GameObject Nurse;
    public Transform spawnPointVampire;
    public Transform spawnPointNurse;

    public CameraManager cameraManager;

    public RoomScript startingRoom;
    void Start()
    {
        var p1 = PlayerInput.Instantiate(Vampire);
        var p2 = PlayerInput.Instantiate(Nurse);

       
        p1.transform.position = spawnPointVampire.position;
        p1.transform.rotation = spawnPointVampire.rotation;

        p2.transform.position = spawnPointNurse.position;
        p2.transform.rotation = spawnPointNurse.rotation;

        cameraManager.player1 = p1.GetComponent<PlayerMovement>();

        cameraManager.player2 = p2.GetComponent<PlayerMovement>(); 

        
        cameraManager.player1.currentRoom = startingRoom;
        cameraManager.player2.currentRoom = startingRoom;

        startingRoom.RegisterPlayer(cameraManager.player1);
        startingRoom.RegisterPlayer(cameraManager.player2);

        

        cameraManager.ActivateRoom(startingRoom);
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