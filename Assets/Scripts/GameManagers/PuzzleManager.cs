using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private int litTorches = 0;
    private int totalTorchesNeeded = 5;
    [SerializeField] private SpriteRenderer pentagramRenderer;
    [SerializeField] private Sprite revealedSprite;

    private int litTableCandles = 0;

    private bool mirrorActive = false;

    private bool bookUnlocked = false;
    [SerializeField] private GameObject candle1Object;
    [SerializeField] private GameObject candle2Object;

    [SerializeField] private TestInteractable candle1;
    [SerializeField] private TestInteractable candle2;

    private TestInteractable activeMirror;

    public GameObject minigameVisual;

    [SerializeField]
    private Grabbable tableBook;

    public bool IsBookUnlocked()
    {
        return bookUnlocked;
    }

    public bool IsMirrorActive()
    {
        return mirrorActive;
    }

    private void Awake()
    {
        Instance = this;
        candle1Object.SetActive(false);
        candle2Object.SetActive(false);
        minigameVisual.SetActive(false);

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
                Debug.Log("All torches are lit. The pentagram is active");
                pentagramRenderer.sprite = revealedSprite;
            }

            return;

           

        }
        if (heldItemType == ItemType.Lighter && targetType == InteractableType.TableCandle)
        {
            if (interactable.HasBeenActivated)
            {
                return;
            }


            interactable.SetLit();
            interactable.MarkActivated();
            Debug.Log("You light the table candle.");
            litTableCandles++;

            if (litTableCandles >= 2)
            {
               Debug.Log("The book now Opens"); // �NDRE SPRITE HER MATHILDE MANGE TAK!

              
                MirrorPuzzleComplete();
            }
                return;
        }

        if (targetType == InteractableType.Mirror)
        {
            int playerID = player.GetComponent<PlayerMovement>().playerID;

            if (bookUnlocked)
            {
                return;
            }

            if (playerID != 2)
            {
                string[] vampireMirrorText =
                {
                    "The Mirror Remains Empty",
                    "No Reflection Stares Back At You",
            
                };

            DialogueManager.Instance.StartDialogue(playerID, vampireMirrorText);
                return;
            }

            if (!mirrorActive)
            {
                activeMirror = interactable;
                interactable.MirrorGameOn();
                StartMirrorPuzzle();
                candle1Object.SetActive(true);
                candle2Object.SetActive(true);
                

            }
            else
            {
                Debug.Log(MirrorGame.Instance);
                MirrorGame.Instance.CheckHit();
            }
                return;
        }

        // NOTHING USEFUL
        Debug.Log("You interact, but nothing happens.");
    }

    public void StartMirrorPuzzle()
    {
        mirrorActive = true;
        Debug.Log("The mirror puzzle is now active. Player 2 can interact with the mirror.");
        minigameVisual.SetActive(true);
       
    }
    public void MirrorPuzzleComplete()
    {
        mirrorActive = false;
        Debug.Log("The mirror puzzle is now disabled");
        minigameVisual.SetActive(false);
        bookUnlocked = true;
        tableBook.canBeGrabbed = true;
        candle1Object.GetComponent<Collider2D>().enabled = false;
        candle2Object.GetComponent<Collider2D>().enabled = false;
    }
    public void MirrorPuzzleLost()
    {
        mirrorActive = false;
        Debug.Log("The mirror puzzle is now disabled. You failed to solve it in time.");
        candle1.MarkUnActivated();
        candle2.MarkUnActivated();
        candle1.SetUnlit();
        candle2.SetUnlit();

        candle1Object.SetActive(false);
        candle2Object.SetActive(false);
        minigameVisual.SetActive(false);


        litTableCandles = 0;

        mirrorActive = false;
        if(activeMirror != null)
        {
            activeMirror.MirrorGameOff();
        }
    }
}