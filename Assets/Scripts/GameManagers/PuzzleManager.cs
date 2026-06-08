using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [SerializeField] private SpriteRenderer SpellBookRenderer;
    [SerializeField] private Sprite SpellBookOpenedSprite;

    [SerializeField] private SpriteRenderer pentagramRenderer;
    [SerializeField] private Sprite revealedSprite;

    [SerializeField] private SpriteRenderer statueRenderer;
    [SerializeField] private Sprite activatedStatueSprite;

    [SerializeField] private GameObject angelScrollPrefab;
    [SerializeField] private Transform angelScrollSpawnPoint;

    [SerializeField] private GameObject rosePrefab;

    [SerializeField] private GameObject alterRitualZone;

    private int litTableCandles = 0;

    private bool mirrorActive = false;
    private bool waitingForMirrorGame;
   

    private bool bookUnlocked = false;
    [SerializeField] private GameObject candle1Object;
    [SerializeField] private GameObject candle2Object;

    [SerializeField] private TestInteractable candle1;
    [SerializeField] private TestInteractable candle2;

    private TestInteractable activeMirror;
    private PlayerInteraction mirrorPlayer;

    public GameObject minigameVisual;

    [SerializeField] private TestInteractable[] ritualCandles;
    private int[] correctOrder = {0,1, 2, 3, 4 };
    private int currentStep = 0;
    private bool torchpuzzlesolved = false;

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

        // ROSE BUSH
        if (targetType == InteractableType.RoseBush)
        {
            if (heldItem != null)
            {
                Debug.Log("Your hands are full.");
                return;
            }

            Debug.Log("You pluck a rose.");
            AudioManager.instance.Play("RosePluck");


            Instantiate(rosePrefab, interactable.transform.position, Quaternion.identity);

            return;
        }


        // LIGHTER + TORCH
        if (heldItemType == ItemType.Lighter && targetType == InteractableType.Torch)
        {

            if(torchpuzzlesolved)
            {
                return;
            }

            if (interactable.HasBeenActivated)
            {
                Debug.Log("This torch is already lit.");
                return;
            }


            int torchIndex = System.Array.IndexOf(ritualCandles, interactable);

            if (torchIndex == -1)
            {
                Debug.LogError(interactable.name + " is not assigned in ritualCandles!");
                return;
            }
            Debug.Log("Torch index: " + torchIndex);

            if (torchIndex == correctOrder[currentStep])
            {
                interactable.SetLit();
                interactable.MarkActivated();
                currentStep++;

                Debug.Log("You lit the correct torch in the sequence.");

                if(currentStep >= correctOrder.Length)
                {
                    Debug.Log("pentagram is active");
                    pentagramRenderer.sprite = revealedSprite;
                    statueRenderer.sprite = activatedStatueSprite;
                    torchpuzzlesolved = true;

                    Instantiate(angelScrollPrefab, angelScrollSpawnPoint.position, Quaternion.identity);
                    alterRitualZone.SetActive(true);
                    AudioManager.instance.Play("PentagramReveal");
                }
            }
            else
            {
                Debug.Log("You lit the wrong torch. The sequence resets.");
                ResetTorchPuzzle();
                AudioManager.instance.Play("Wrong");
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
                SpellBookRenderer.sprite = SpellBookOpenedSprite;
                AudioManager.instance.Play("BookOpen");

                bookUnlocked = true;
               tableBook.canBeGrabbed = true;
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

            DialogueManager.Instance.StartDialogue(playerID, vampireMirrorText, InteractableType.Mirror);
                return;
            }

            if (!mirrorActive)
            {
                
                waitingForMirrorGame = true;
                mirrorPlayer = player;
                activeMirror = interactable;
                string[] mirrirIntro =
                {
                    "This Mirror seems to distort what is shown",
                    "Almost like it changes reality"
                };

                DialogueManager.Instance.StartDialogue(playerID, mirrirIntro, InteractableType.Mirror);
                


                return;
               
                

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

    public void StartMirrorPuzzle(PlayerInteraction player)
    {
        mirrorActive = true;

        tableBook.GetComponent<Collider2D>().enabled = false;

        AudioManager.instance.Play("MirrorLoop");

        MirrorGame.Instance.StartMirrorTimer(player);

        Debug.Log("The mirror puzzle is now active. Player 2 can interact with the mirror.");

        minigameVisual.SetActive(true);
       
    }
    public void MirrorPuzzleComplete()
    {
        mirrorActive = false;
        AudioManager.instance.Stop("MirrorLoop");
        Debug.Log("The mirror puzzle is now disabled");
        minigameVisual.SetActive(false);
        bookUnlocked = true;
        tableBook.canBeGrabbed = true;
        candle1Object.GetComponent<Collider2D>().enabled = false;
        candle2Object.GetComponent<Collider2D>().enabled = false;

        tableBook.GetComponent<Collider2D>().enabled = true;

    }
    public void MirrorPuzzleLost()
    {
        mirrorActive = false;
        AudioManager.instance.Stop("MirrorLoop");
        Debug.Log("The mirror puzzle is now disabled. You failed to solve it in time.");
        AudioManager.instance.Play("Wrong");
        candle1.MarkUnActivated();
        candle2.MarkUnActivated();
        candle1.SetUnlit();
        candle2.SetUnlit();

        candle1Object.SetActive(false);
        candle2Object.SetActive(false);
        minigameVisual.SetActive(false);
        tableBook.GetComponent<Collider2D>().enabled = true;


        litTableCandles = 0;

       
        if(activeMirror != null)
        {
            activeMirror.MirrorGameOff();
        }
    }
    private void ResetTorchPuzzle()
    {
        currentStep = 0;

        foreach (TestInteractable torch in ritualCandles)
        {
            Debug.Log("Resetting torch: " + torch.name);
            torch.MarkUnActivated();
            torch.SetUnlit();
        }
    }

    public bool IsWaitingForMirrorGameOn()
    {
        return waitingForMirrorGame;
    }

    public void BeginMirrorAfterDialogue()
    {
        waitingForMirrorGame = false;

        activeMirror.MirrorGameOn();
        StartMirrorPuzzle(mirrorPlayer);

        candle1Object.SetActive(true);
        candle2Object.SetActive(true);
    }
}