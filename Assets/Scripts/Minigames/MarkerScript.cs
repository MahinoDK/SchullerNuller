using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    public bool insideTarget { get; private set; }
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MirrorTarget"))
        {
            
            insideTarget = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MirrorTarget"))
        {
            
            insideTarget = false;
        }
    }
}
