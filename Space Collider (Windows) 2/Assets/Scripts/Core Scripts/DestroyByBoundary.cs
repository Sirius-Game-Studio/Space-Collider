using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (CompareTag("Boundary") && !other.CompareTag("Player")) Destroy(other.gameObject);
    }
}