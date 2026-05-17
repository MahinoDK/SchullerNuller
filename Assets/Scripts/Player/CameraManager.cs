using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public PlayerMovement player1;
    public PlayerMovement player2;

    public RoomScript[] rooms;

    private RoomScript currentActiveRoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.currentRoom == null ||
            player2.currentRoom == null)
        {
            Debug.Log("A player room is null");
            return;
        }

        Debug.Log(
            "P1: " + player1.currentRoom.name +
            " | P2: " + player2.currentRoom.name +
            " | Active: " +
            (currentActiveRoom != null
                ? currentActiveRoom.name
                : "NULL")
        );

        if (player1.currentRoom == player2.currentRoom)
        {
            if (currentActiveRoom != player1.currentRoom)
            {
                Debug.Log("ACTIVATING NEW ROOM");

                ActivateRoom(player1.currentRoom);
            }
        }
    }

    public void ActivateRoom(RoomScript room)
    {
        Debug.Log("ActivateRoom called: " + room.name);

        foreach (RoomScript r in rooms)
        {
            if (r != null && r.roomCamera != null)
            {
                Debug.Log("Disabling: " + r.roomCamera.name);

                r.roomCamera.gameObject.SetActive(false);
            }
        }

        Debug.Log("Enabling: " + room.roomCamera.name);

        room.roomCamera.gameObject.SetActive(true);

        currentActiveRoom = room;
    }
}
