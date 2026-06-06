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
            DialogueManager.Instance.StartDialogue(playerID, lockedPages);
            return;
        }

        DialogueManager.Instance.StartDialogue(playerID, unlockedPages);
    }

}
