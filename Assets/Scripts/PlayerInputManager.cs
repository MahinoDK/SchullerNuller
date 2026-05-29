 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{

    [SerializeField] private GameObject vamp;
    [SerializeField] private Transform[] spawnpoint;

  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current == null) return;
    }
}
