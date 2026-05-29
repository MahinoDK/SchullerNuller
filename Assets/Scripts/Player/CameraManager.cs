using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public PlayerMovement player1;
    public PlayerMovement player2;

    public RoomScript[] rooms;

    private RoomScript currentActiveRoom;
    private bool isSplitScreenActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (player1.currentRoom == player2.currentRoom)
        {
            if (currentActiveRoom != player1.currentRoom)
            {
                ActivateRoom(player1.currentRoom);
            }
        }
        else 
        {
                ActivateSplitScreen();
        }

    }

    public void ActivateRoom(RoomScript room)
    {
        
        foreach (RoomScript r in rooms)
        {
            r.staticCamera.gameObject.SetActive(false);
            r.followCamera.gameObject.SetActive(false);
        }

        room.staticCamera.rect =
            new Rect(0, 0, 1, 1);

        room.staticCamera.gameObject.SetActive(true);

        currentActiveRoom = room;
        isSplitScreenActive = false;
    }

    public void ActivateSplitScreen()
    {

        currentActiveRoom = null;
        foreach (RoomScript room in rooms)
        {
            room.staticCamera.gameObject.SetActive(false);
            room.followCamera.gameObject.SetActive(false);
        }

        RoomScript p1Room = player1.currentRoom;
        RoomScript p2Room = player2.currentRoom;

        p1Room.followCamera.rect =
            new Rect(0.1f, 0.5f, 0.8f, 0.5f);

        p2Room.followCamera.rect =
            new Rect(0.1f, 0f, 0.8f, 0.5f);

        p1Room.followCamera.gameObject.SetActive(true);
        p2Room.followCamera.gameObject.SetActive(true);

        isSplitScreenActive = true;
    }
}
