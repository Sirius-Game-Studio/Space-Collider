using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Tooltip("How fast this object rotates on the Y axis.")] [SerializeField] private float y = 0;

    void Update()
    {
        transform.Rotate(new Vector3(0, y * Time.deltaTime, 0));
    }
}
