using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MirrorGame : MonoBehaviour
{
    public static MirrorGame Instance;

   
    [SerializeField] private Transform marker;
    [SerializeField] private Transform target;
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;

    [SerializeField] private float speed = 1f;

    private bool insideTarget = false;

    private bool movingRight = true;

    [SerializeField] float timeLimit = 5f;
    private float currentTime;

    [SerializeField] private MarkerScript markerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        bool ritualActive = AltarRitualZone.Instance != null && AltarRitualZone.Instance.IsRitualActive();

        if (!PuzzleManager.Instance.IsMirrorActive() &&
            !ritualActive)
        {
            return;
        }

        if (PuzzleManager.Instance.IsMirrorActive())
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                PuzzleManager.Instance.MirrorPuzzleLost();
                return;
            }
        }
        
        if (movingRight)
        {
            marker.position += Vector3.right * speed * Time.deltaTime;

            if (marker.position.x >= rightBoundary.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            marker.position +=Vector3.left * speed * Time.deltaTime;
            if (marker.position.x <= leftBoundary.position.x)
            {
                movingRight = true;
            }
        }
    }

    public void CheckHit()
    {
        if (!markerScript.insideTarget)
        {
            if (PuzzleManager.Instance.IsMirrorActive())
            {
                PuzzleManager.Instance.MirrorPuzzleLost();
            }
            else if (AltarRitualZone.Instance.IsRitualActive())
            {
                AltarRitualZone.Instance.RitualHitFail();
            }

            return;
        }

        Debug.Log("Hit target!");
        MoveTarget();
       
        
        
        if (AltarRitualZone.Instance != null && AltarRitualZone.Instance.IsRitualActive())
        {
            AltarRitualZone.Instance.RitualHitSuccess();
        }
    }

    private void MoveTarget()
    {
        float randomX = Random.Range(leftBoundary.position.x +0.1f,rightBoundary.position.x -0.1f);

        target.position = new Vector3(randomX,target.position.y,target.position.z);

        currentTime = timeLimit;
    }
    public void StartMirrorTimer()
    {
        currentTime = timeLimit;
    }
}
