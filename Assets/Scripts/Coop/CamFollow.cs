using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;

    public float minY;
    public float maxY;
    private float fixedX;

    void Start()
    {
        fixedX = transform.position.x;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float y = Mathf.Clamp(
            target.position.y,
            minY,
            maxY
        );

        transform.position = new Vector3(
            fixedX,
            y,
            transform.position.z
        );
    }
}