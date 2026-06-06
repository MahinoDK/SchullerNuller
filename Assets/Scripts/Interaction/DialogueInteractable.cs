using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [TextArea]
    public string[] dialoguePages;

    public InteractableType interactableType;
    public void Interact(PlayerInteraction player)
    {
        int playerID = player.GetComponent<PlayerMovement>().playerID;

        Debug.Log("Player ID " + playerID);

        DialogueManager.Instance.SetInteractableType(interactableType);
        DialogueManager.Instance.StartDialogue(playerID, dialoguePages);
    }
}
