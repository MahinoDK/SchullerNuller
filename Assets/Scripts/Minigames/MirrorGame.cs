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

    [SerializeField] private MarkerScript markerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       if (!PuzzleManager.Instance.IsMirrorActive()) return;

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
        if (markerScript.insideTarget)
        {
            Debug.Log("Success!");
            MoveTarget();

        }
        else
        {
            Debug.Log("Missed! Try again.");
            PuzzleManager.Instance.MirrorPuzzleLost();
            
        }
    }

    private void MoveTarget()
    {
        float randomX = Random.Range(leftBoundary.position.x +0.1f,rightBoundary.position.x -0.1f);

        target.position = new Vector3(randomX,target.position.y,target.position.z);
    }

}
