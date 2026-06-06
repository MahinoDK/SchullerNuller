using UnityEngine;

public class SpellBook : MonoBehaviour, IInteractable
{
    private Grabbable grabbable;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();

        grabbable.canBeGrabbed = false;
    }

    private void Update()
    {
        if (PuzzleManager.Instance.IsBookUnlocked())
        {
            grabbable.canBeGrabbed = true;
        }
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Book Interacted");
    }
}
