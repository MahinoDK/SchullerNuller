using UnityEngine;
using System.Collections;

public class AltarRitualZone : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    public static AltarRitualZone Instance;
    private PlayerInteraction vampireInZone;
    private bool ritualActive = false;
    private bool ritualStarting = false;

    [SerializeField] private float ritualDuration = 100.0f;
    private float ritualProgress = 0.0f;

    [SerializeField] private GameObject ritualMinigameVisual;

    [SerializeField] private Transform progressFill;

    public GameObject portalSpawnPoint;
    public GameObject finishPortal;

    private bool ritualCompleted = false;
    public void Awake()
    {
        Instance = this;

        ritualProgress = 0f;
        UpdateProgressBar();
    }
    public float RitualPercent()
    {
        return ritualProgress / ritualDuration;
    }
    public bool IsRitualActive()
    {
        return ritualActive;
    }
    public bool IsVampireInZone()
    {
               return vampireInZone != null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerInteraction player = other.GetComponent<PlayerInteraction>();
        if (player == null) return;

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement == null) return;

        // change this if vampire has another ID
        if (movement.playerID != 1) return;

        vampireInZone = player;

        if (ritualCompleted) return;
        if (VampireHasSpellBook(player))
        {
            if (!ritualActive && !ritualStarting)
            {
                StartCoroutine(StartRitualAfterDelay(player));
            }
        }
        else
        {
            StopRitual();
        }
    }

    private IEnumerator StartRitualAfterDelay(PlayerInteraction player)
    {
        ritualStarting = true;

        ritualProgress = 0;
        UpdateProgressBar();

        yield return new WaitForSeconds(1.0f);

        ritualStarting = false;

        if (player == vampireInZone && VampireHasSpellBook(player))
        {
            ritualActive = true;
            enemySpawner.StartSpawning();
            
            if (ritualMinigameVisual != null)
            {
                ritualMinigameVisual.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInteraction player = other.GetComponent<PlayerInteraction>();

        if (player != null && player == vampireInZone)
        {
            StopRitual();
            vampireInZone = null;
        }
    }

    private void Update()
    {
        if (!ritualActive) return;

       

        if (vampireInZone == null || !VampireHasSpellBook(vampireInZone))
        {
            StopRitual();
        }

        if (ritualProgress >= ritualDuration)
        {
            CompleteRitual();
        }
    }

    private bool VampireHasSpellBook(PlayerInteraction player)
    {
        Grabbable heldItem = player.GetHeldItem();

        return heldItem != null && heldItem.itemType == ItemType.SpellBook;
    }

    public void StopRitual()
    {
       
        ritualStarting = false;

        if (!ritualActive) return;
        ritualProgress = 0f;
        UpdateProgressBar();
        ritualActive = false;

        if (ritualMinigameVisual != null)
        {
            ritualMinigameVisual.SetActive(false);
        }
        enemySpawner.StopAndResetSpawning();

        
    }

    public void CompleteRitual()
    {
        ritualActive = false;
        Debug.Log("Ritual complete! Portal should open now.");

        if  (ritualMinigameVisual != null)
        {
            ritualMinigameVisual.SetActive(false);
        }
        enemySpawner.StopAndResetSpawning();
        ritualCompleted = true;

        Instantiate(finishPortal, portalSpawnPoint.transform.position, Quaternion.identity);
        // instantiate portal to win game here

    }

    public void RitualHitSuccess()
    {
        if (!ritualActive) return;
        ritualProgress += 5.0f; // Adjust this value based on how much progress each hit should give

        Debug.Log("ritual progress: " + ritualProgress + "/" + ritualDuration);

        if (ritualProgress >= ritualDuration)
        {
            ritualProgress = ritualDuration;
            CompleteRitual();
        }
        UpdateProgressBar();
    }
    public void RitualHitFail()
    {
        if (!ritualActive) return;

        ritualProgress -= 3.0f; // Adjust this value based on how much progress each fail should take away

        if (ritualProgress < 0f)
        {
            ritualProgress = 0f;
        }
        UpdateProgressBar();
        Debug.Log("ritual progress: " + ritualProgress + "/" + ritualDuration);
    }

    private void UpdateProgressBar()
    {
        float percent = ritualProgress / ritualDuration;

        progressFill.localScale = new Vector3(percent, 1f, 1f);

        progressFill.localPosition = new Vector3((1f - percent) * 0.5f, 0f, 0f);

        Debug.Log("percent: " + percent);
    }
}