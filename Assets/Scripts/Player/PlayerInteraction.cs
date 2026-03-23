using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Hold Position")]
    [SerializeField] private Transform holdPosition;

    private List<Grabbable> nearbyGrabbables = new List<Grabbable>(); //grabbale
    private List<IInteractable> nearbyInteractables = new List<IInteractable>(); //interactable
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

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (nearbyInteractables.Count == 0) return;
        //interact with the first nearby interactable, -> can expand this later to choose which one to interact with if there are multiple
        nearbyInteractables[0].Interact(this);        //we send the player interaction script as a parameter, so the interactable can access the player's held item if needed, and other info about the player
        //nearbyInteractables[0].SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable != null && !nearbyGrabbables.Contains(grabbable))
        {
            nearbyGrabbables.Add(grabbable);
        }

        MonoBehaviour[] behaviours = other.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IInteractable interactable && !nearbyInteractables.Contains(interactable))
            {
                nearbyInteractables.Add(interactable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable != null && nearbyGrabbables.Contains(grabbable))
        {
            nearbyGrabbables.Remove(grabbable);
        }

        MonoBehaviour[] behaviours = other.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IInteractable interactable && nearbyInteractables.Contains(interactable))
            {
                nearbyInteractables.Remove(interactable);
            }
        }
    }

}
