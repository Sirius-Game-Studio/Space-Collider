using UnityEngine;

public class DestroyOnClear : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount <= 0) Destroy(gameObject);   
    }
}
