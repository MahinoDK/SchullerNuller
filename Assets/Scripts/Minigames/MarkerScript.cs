using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    public bool insideTarget { get; private set; }
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MirrorTarget"))
        {
            Debug.Log("Marker entered target area.");
            insideTarget = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MirrorTarget"))
        {
            Debug.Log("Marker exited target area.");
            insideTarget = false;
        }
    }
}
