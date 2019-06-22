using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    { 
        if (other.CompareTag("Player")) return;
        Destroy(other.gameObject);
    }
}