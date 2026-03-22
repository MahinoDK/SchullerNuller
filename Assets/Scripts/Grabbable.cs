using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
    public void SetSortingOrder(int order)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
