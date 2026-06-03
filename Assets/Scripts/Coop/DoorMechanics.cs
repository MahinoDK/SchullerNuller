using UnityEngine;

public class DoorMechanics : MonoBehaviour
{

    public DoorMechanics linkedDoor;
    public Vector2 exitOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        PlayerMovement player = collision.GetComponentInParent<PlayerMovement>();
        if (player == null) return;

        player.transform.position = (Vector2)linkedDoor.transform.position + linkedDoor.exitOffset; 
    }

}
