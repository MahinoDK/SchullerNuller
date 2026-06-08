using UnityEngine;

public class PlayerHeartInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private int thisPlayerID;
    [SerializeField] private Transform heartSpawnPoint;

    [SerializeField] private GameObject batHeartPrefab;
    [SerializeField] private GameObject angelHeartPrefab;

    [Header("Cooldown")]
    [SerializeField] private float kissCooldown = 0.5f;

    private float lastKissTime = -999f;

    public void Interact(PlayerInteraction player)
    {
        // only kiss if the interacting player has empty hands
        if (player.GetHeldItem() != null)
        {
            Debug.Log("Your hands are full.");
            return;
        }

        // prevent kiss/sound spam
        if (Time.time - lastKissTime < kissCooldown)
        {
            return;
        }

        int interactingPlayerID = player.GetComponent<PlayerMovement>().playerID;

        // Nurse interacts with vampire
        if (interactingPlayerID == 2 && thisPlayerID == 1)
        {
            lastKissTime = Time.time;

            Instantiate(batHeartPrefab, heartSpawnPoint.position, Quaternion.identity);
            AudioManager.instance.Play("Kiss");
        }

        // Vampire interacts with nurse
        if (interactingPlayerID == 1 && thisPlayerID == 2)
        {
            lastKissTime = Time.time;

            Instantiate(angelHeartPrefab, heartSpawnPoint.position, Quaternion.identity);
            AudioManager.instance.Play("Kiss");
        }
    }
}