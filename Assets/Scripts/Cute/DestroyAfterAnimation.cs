using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
