using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 0;
    
    void Awake()
    {
        Destroy(gameObject, lifetime);
    }
}
