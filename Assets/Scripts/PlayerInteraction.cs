using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Hold Position")]
    [SerializeField] private Transform holdPosition;

    private List<Grabbable> nearbyGrabbables = new List<Grabbable>();
    private Grabbable heldItem;

    public Grabbable GetHeldItem()
    {
               return heldItem;
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //already holding something, do nothing (her kan man senere tilføje en cute animation eller text der siger man ik kan holde mere end en, eller sådan en grr lyd
        if (heldItem != null) return;

        //no item nearby? DO NOTHING
        if (nearbyGrabbables.Count == 0) return;

        //pick the first nearby item and grab it
        heldItem = nearbyGrabbables[0];
        heldItem.Grab(holdPosition);
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //not holding anything, do nothing
        if (heldItem == null) return;

        heldItem.Drop(transform.position); //item dropped where player is positioned, new position for the item now
        heldItem = null; //clear the held item reference
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable != null && !nearbyGrabbables.Contains(grabbable))
        {
            nearbyGrabbables.Add(grabbable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable != null && nearbyGrabbables.Contains(grabbable))
        {
            nearbyGrabbables.Remove(grabbable);
        }
    }

}
