using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [Header("Item Info")]
    public ItemType itemType = ItemType.None; //Public in inspector u can asign the SPECIFIC item to the desired object so each object all have this script but assigned different item type!!!!



    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    private bool angelScrollDialogueShown = false;
    private Animator animator;
    public bool canBeGrabbed = true; //flag to control if the item can be grabbed, default to true but can be set to false for items that start as ungrabbable (like the spellbook)

    private float lastScrollSoundTime = -999f;
    [SerializeField] private float scrollSoundCooldown = 0.5f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
    }

    public void Grab(Transform holdPosition)
        {
            //turn off physics for the object held
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }

        // turn off collider
        if (col != null)
        {
            col.enabled = false;
        }

        //attacj to the player to hold and move with them
             transform.SetParent(holdPosition);
             transform.localPosition = Vector3.zero;


        if(itemType == ItemType.angelScroll && !angelScrollDialogueShown)
        {
            Debug.Log("Holding angelscroll starting dialogue");
            angelScrollDialogueShown=true;
            PlayerInteraction player = holdPosition.GetComponentInParent<PlayerInteraction>();

            if(player != null)
            {
                Debug.Log("Commencing dialogue");
                int playerID = player.GetComponent<PlayerMovement>().playerID;

                string[] scrollText =
                {
                    "The sacred healing spell. Heals the living but vanquishes the undead.",
                    "Only descendants of the goddess race can unleash its power"
                };

                DialogueManager.Instance.StartDialogue(playerID, scrollText, InteractableType.None);
            }
        }
       }


    public void UseHeldItemAnimation(PlayerInteraction player)
    {
        if (animator == null) return;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (itemType == ItemType.Lighter)
        {
            animator.SetTrigger("Use");
            AudioManager.instance.Play("LighterUse");
        }

        if (itemType == ItemType.angelScroll)
        {
            if (playerMovement.playerID == 2) // nurse ID
            {
                animator.SetTrigger("Use");
                if (Time.time - lastScrollSoundTime > scrollSoundCooldown)
                {
                    AudioManager.instance.Play("ScrollUse");
                    lastScrollSoundTime = Time.time;
                }
            }
            else
            {
                Debug.Log("Only the nurse can use this.");
            }
        }
    }

    public void Drop(Vector3 dropPosition)
    {
        //detach from the player
        transform.SetParent(null);
        transform.position = dropPosition;

        //turn physics back on
        if (rb != null)
        {
            rb.simulated = true;
        }

        //turn collider back on
        if (col != null)
        {
            col.enabled = true;
        }

        if (spriteRenderer != null)
        {
            SetSortingOrder(0); //reset sorting order when dropped, you can adjust this value as needed
        }
    }
    public void SetSortingOrder(int order)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
