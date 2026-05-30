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

    [SerializeField]
    private GameObject interactPrompt; //reference to the interact prompt UI element
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
        Debug.Log("Interact button pressed");
        if (!context.performed) return;

        int playerID = GetComponent<PlayerMovement>().playerID; //get the player ID from the player movement script, to know which dialogue to show for which player
        if (heldItem != null) //if holding an item we can trigger animation to!
        {
            heldItem.UseHeldItemAnimation();
        }

        if (DialogueManager.Instance.IsDialogueOpen(playerID))
        {
            DialogueManager.Instance.NextPage(playerID);
            return;
        }

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
            interactPrompt.SetActive(true); //show the interact prompt when entering the trigger of a grabbable item
            nearbyGrabbables.Add(grabbable);
        }

        MonoBehaviour[] behaviours = other.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IInteractable interactable && !nearbyInteractables.Contains(interactable))
            {
                interactPrompt.SetActive(true); //show the interact prompt when entering the trigger of an interactable item
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
            interactPrompt.SetActive(false); //hide the interact prompt when exiting the trigger of a grabbable item
        }

        MonoBehaviour[] behaviours = other.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IInteractable interactable && nearbyInteractables.Contains(interactable))
            {
                nearbyInteractables.Remove(interactable);
                interactPrompt.SetActive(false); //hide the interact prompt when exiting the trigger of an interactable item
            }
        }
    }

}
