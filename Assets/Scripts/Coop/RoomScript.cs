using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Camera staticCamera;
    public Camera followCamera;

    public CamFollow followScript;

    public List<PlayerMovement> playersInRoom = new();


    private void Start()
    {
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player != null && !playersInRoom.Contains(player))
        {
            playersInRoom.Add(player);
            player.currentRoom = this;
        }
        UpdateFollowTarget();

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            playersInRoom.Remove(player);
        }
        UpdateFollowTarget();
    }

    private void UpdateFollowTarget()
    {
        if(playersInRoom.Count == 1)
        {
            followScript.target = playersInRoom[0].transform;
        }
        else
        {
            followScript.target = null;
        }
        
    }

    public void RegisterPlayer(PlayerMovement player)
    {
        if (!playersInRoom.Contains(player))
        {
            playersInRoom.Add(player);
            player.currentRoom = this;
        }

        UpdateFollowTarget();
    }
    public bool HasSinglePlayer()
    {
        return playersInRoom.Count == 1;
    }
    public PlayerMovement GetSinglePlayer()
    {
        if (playersInRoom.Count == 1)
            return playersInRoom[0];

        return null;
    }
    public bool ContainsPlayer(PlayerMovement player)
    {
        return playersInRoom.Contains(player);
    }
}