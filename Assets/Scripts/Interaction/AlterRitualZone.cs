using UnityEngine;
using System.Collections;

public class AltarRitualZone : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    private PlayerInteraction vampireInZone;
    private bool ritualActive = false;
    private bool ritualStarting = false;

    [SerializeField] private float ritualDuration = 30.0f;
    private float ritualProgress = 0.0f;

    public float RitualPercent()
    {
        return ritualProgress / ritualDuration;
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

        yield return new WaitForSeconds(1.0f);

        ritualStarting = false;

        if (player == vampireInZone && VampireHasSpellBook(player))
        {
            ritualActive = true;
            enemySpawner.StartSpawning();
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

        ritualProgress += Time.deltaTime;

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

    private void StopRitual()
    {
       
        ritualStarting = false;

        if (!ritualActive) return;
        ritualProgress = 0f;
        ritualActive = false;
        enemySpawner.StopAndResetSpawning();
    }

    private void CompleteRitual()
    {
        ritualActive = false;
        Debug.Log("Ritual complete! Portal should open now.");
        enemySpawner.StopAndResetSpawning();
        
        // instantiate portal to win game here

    }
}