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

        if(interactableType == InteractableType.BookShelf)
        {
            if(playerID == 2)
            {
                string[] nurseBooktext =
                {
                    "The language of this book is before my time!",
                    "I'd better not think too much about it"
                };

                DialogueManager.Instance.StartDialogue(playerID, nurseBooktext, interactableType);
                return;
               
            }
            
        }


        Debug.Log("Using Inspector");
        DialogueManager.Instance.StartDialogue(playerID, dialoguePages, interactableType);
    }
}
