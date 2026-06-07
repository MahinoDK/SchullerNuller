using System.Collections;
using UnityEngine;

public class GhostMovement : MonoBehaviour 
{

    [SerializeField] private float moveSpeed = 1.5f;
    bool isPurified = false;
    public Animator anim;

    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.InstanceID); 

        foreach(PlayerMovement player in players)
        {
            if(player.playerID == 1)
            {
                target = player.transform;
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if (!isPurified)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        Vector3 scale = transform.localScale;

        if (target.position.x > transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

   public void Purify()
    {
        isPurified = true;
        StartCoroutine(PurifyCoroutine());
    }

    private IEnumerator PurifyCoroutine()
    {
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(1f);
        Debug.Log("Ghost purified!");
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (!AltarRitualZone.Instance.IsRitualActive()) return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player == null) return;

        if (player.playerID != 1) return;

        Debug.Log("Ghost reached Vampire");
        AltarRitualZone.Instance.StopRitual();
        
    }
}
