using UnityEngine;
using System.Collections;

public class GraveDropZone : MonoBehaviour
{
    [SerializeField] private TestInteractable graveInteractable;
    [SerializeField] private GameObject lighterPrefab;
    [SerializeField] private Transform lighterSpawnPoint;

    private bool hasReceivedRose = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasReceivedRose)
            return;

        Grabbable grabbable = other.GetComponent<Grabbable>();
        if (grabbable == null)
            return;

        if (grabbable.itemType == ItemType.Rose)
        {
            hasReceivedRose = true;
            Debug.Log("Rose received at the grave!");

            graveInteractable.TriggerAnimation("Rise");
            AudioManager.instance.Play("GraveRise");

            StartCoroutine(SpawnLighterAfterDelay());

        }
    }

    private IEnumerator SpawnLighterAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);

        Instantiate(
            lighterPrefab,
            lighterSpawnPoint.position,
            Quaternion.identity
        );
        AudioManager.instance.Play("Arigato");
        AudioManager.instance.Play("LighterSpawn");
    }
}
