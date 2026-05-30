using UnityEngine;

public class DoorMechanics : MonoBehaviour
{

    public DoorMechanics linkedDoor;
    public Vector2 exitOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        PlayerMovement player = collision.GetComponentInParent<PlayerMovement>();
        if (player == null) 
            return;

       

        player.transform.position = (Vector2)linkedDoor.transform.position + linkedDoor.exitOffset;

       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
