using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Camera roomCamera;
    public int roomID;

    
    private void OnTriggerStay2D(Collider2D other)
    {
      

        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            
            player.currentRoom = this;

            
        }

    }
}