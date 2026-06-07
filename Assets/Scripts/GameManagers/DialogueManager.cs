using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager Instance;
    [Header("Player 1")]
    public GameObject Player1Panel;
    public TMP_Text Player1Text;
    public GameObject SharedPlayer1Panel;
    public TMP_Text SharedPlayer1Text;
    public Image Item1;
    public Image Item1F; //SplitScreenItem
    [Header("Player 2")]
    public GameObject Player2Panel;
    public TMP_Text Player2Text;
    public Image Item2;

    private InteractableType player1Type;
    private InteractableType player2Type;
    private string[] player1Pages;
    private int player1Page;

    private string[] player2Pages;
    private int player2Page;

    private bool player1DialogueOpen;
    private bool player2DialogueOpen;

    

    public bool IsDialogueOpen(int playerID)
    {
        if (playerID == 1)
            return player1DialogueOpen;

        return player2DialogueOpen;
    }
    private void Awake()
    {
        Instance = this;
         
    }

    private void Start()
    {
       Player1Panel.SetActive(false);
       Player2Panel.SetActive(false);
       SharedPlayer1Panel.SetActive(false);
    }

    public void ShowDialogue(int playerID, string Text)
    {
        if (playerID == 1)
        {
            
            Player1Panel.SetActive(true);
            Player1Text.text = Text;
            
        }
        else
        {
            Player2Panel.SetActive(true);
            Player2Text.text = Text;
          
        }
    }

    public void HideDialogue(int playerID)
    {
        if (playerID == 1)
        {
            Player1Panel.SetActive(false);
            SharedPlayer1Panel.SetActive(false);
            Item1.gameObject.SetActive(false);
            Item1F.gameObject.SetActive(false);
        }
        else
        {
            Player2Panel.SetActive(false);
            Item2.gameObject.SetActive(false);

        }

    }

    public void StartDialogue(int playerID, string[] pages, InteractableType type)
    {
        if (playerID == 1)
        {
            player1Type = type;
            player1Pages = pages;
            player1Page = 0;
            player1DialogueOpen = true;
            if (type == InteractableType.BookShelf)
            {
                Debug.Log("Activating Item1 for Player 1");
                Item1.gameObject.SetActive(true);
            }
        }
        else
        {
            player2Type = type;
            player2Pages = pages;
            player2Page = 0;
            player2DialogueOpen = true;
            if (type == InteractableType.BookShelf)
            {
                Item2.gameObject.SetActive(true);
            }
        }

        ShowCurrentPage(playerID);
    }
    private void ShowCurrentPage(int playerID)
    {
        if (playerID == 1)
        {
            string pageText = player1Pages[player1Page];

            if (CameraManager.Instance.IsSplitScreenActive())
            {
                Player1Panel.SetActive(true);
                Player1Text.text = pageText;

                if (player1Type == InteractableType.BookShelf)
                {
                    Item1F.gameObject.SetActive(true);
                }
            }
            else
            {
                SharedPlayer1Panel.SetActive(true);
                SharedPlayer1Text.text = pageText;

                if (player1Type == InteractableType.BookShelf)
                {
                    Item1.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            string pageText = player2Pages[player2Page];

            Player2Panel.SetActive(true);
            Player2Text.text = pageText;
            if (player2Type == InteractableType.BookShelf)
            {
                Item2.gameObject.SetActive(true);
            }
        }
    }

    public void NextPage(int playerID)
    {
        if (playerID == 1)
        {
            player1Page++;

            if (player1Page >= player1Pages.Length)
            {
                HideDialogue(1);
                player1DialogueOpen = false;
                return;
            }

            ShowCurrentPage(1);
        }
        else
        {
            player2Page++;

            if (player2Page >= player2Pages.Length)
            {
                HideDialogue(2);
                player2DialogueOpen = false;
                return;
            }

            ShowCurrentPage(2);
        }
    }
    public void RefreshDialogueUI(int playerID)
    {
        if (!IsDialogueOpen(playerID))
            return;

        HideDialogue(playerID);

        ShowCurrentPage(playerID);
    }
}
