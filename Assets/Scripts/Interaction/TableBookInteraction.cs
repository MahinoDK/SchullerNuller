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
        if (playerID == 1 && PuzzleManager.Instance.IsBookUnlocked())
        {
            string[] vampUnlockPages =
            {
                "These symbols give off some magical powers",
                "I bet I can use them in a spell"
            };
            DialogueManager.Instance.StartDialogue(playerID, vampUnlockPages, InteractableType.SpellBook);
            return;
        }

        DialogueManager.Instance.StartDialogue(playerID, unlockedPages, InteractableType.SpellBook);
    }

}
