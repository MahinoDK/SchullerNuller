using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [Header("Required Item")]
    [SerializeField] private ItemType requiredItem = ItemType.None; //The item required to interact with this object, set in inspector

    [Header("Color Change")]
    [SerializeField] private Color changedColor = Color.green; //The color to change to when interacted with the required item testItem

    private SpriteRenderer spriteRenderer;
    private bool hasChanged = false; //To prevent multiple interactions changing the color multiple times

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Interact(PlayerInteraction player)
    {
        Grabbable heldItem = player.GetHeldItem();

        //if this object needs no item
        if(requiredItem == ItemType.None)
        {
            Debug.Log("You press the item to the block! ...But nothing happenes.");
            return;
        }

        //if player not holding anything
        if(heldItem == null)
        {
            Debug.Log("You press your hands to the block! ...But nothing happenes.");
            return;
        }

        //if player holding the required item and the color hasn't changed yet
        if(heldItem.itemType == requiredItem)
        {
            if(!hasChanged && spriteRenderer != null)
            {
                spriteRenderer.color = changedColor;
                hasChanged = true;
            }
            Debug.Log("You press the item to the block... It Changed color!");
        }
        else
        {
        Debug.Log("You press the item to the block... But nothing happenes.");
        }
    }
}
