using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private int litTorches = 0;
    private int totalTorchesNeeded = 5;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleInteraction(PlayerInteraction player, TestInteractable interactable)
    {
        Grabbable heldItem = player.GetHeldItem();

        ItemType heldItemType = ItemType.None;

        if (heldItem != null)
        {
            heldItemType = heldItem.itemType;
        }

        InteractableType targetType = interactable.interactableType;

        // LIGHTER + TORCH
        if (heldItemType == ItemType.Lighter && targetType == InteractableType.Torch)
        {

            if (interactable.HasBeenActivated)
            {
                Debug.Log("This torch is already lit.");
                return;
            }

            Debug.Log("You light the torch.");

            interactable.SetLit();
            interactable.MarkActivated();

            litTorches++;

            if (litTorches >= totalTorchesNeeded)
            {
                Debug.Log("All torches are lit. The pentagram reveals a code.");
            }

            return;
        }

        // BLOOD VIAL + ALTAR
        if (heldItemType == ItemType.BloodVial && targetType == InteractableType.Altar)
        {
            if (interactable.HasBeenActivated)
            {
                Debug.Log("The altar has already been used.");
                return;
            }

            Debug.Log("You place the blood vial on the altar. You escaped!");
            interactable.ChangeColor(Color.red);
            interactable.MarkActivated();

            return;
        }

        // NOTHING USEFUL
        Debug.Log("You interact, but nothing happens.");
    }
}