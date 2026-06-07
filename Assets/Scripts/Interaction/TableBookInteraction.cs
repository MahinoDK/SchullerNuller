using UnityEngine;

public class TableBookInteraction : MonoBehaviour, IInteractable
{

    [TextArea]
    public string[] lockedPages;

    [TextArea]
    public string[] unlockedPages;


    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Book interaction triggered");

        int playerID = player.GetComponent<PlayerMovement>().playerID;

        if (!PuzzleManager.Instance.IsBookUnlocked())
        {
            DialogueManager.Instance.StartDialogue(playerID, lockedPages, InteractableType.SpellBook);
            return;
        }

        DialogueManager.Instance.StartDialogue(playerID, unlockedPages, InteractableType.SpellBook);
    }

}
