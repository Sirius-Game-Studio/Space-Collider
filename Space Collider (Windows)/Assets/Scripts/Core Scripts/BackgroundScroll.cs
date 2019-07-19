using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float speed = -1;
    [SerializeField] private float z = 36.86f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = initialPosition + Vector3.up * Mathf.Repeat(Time.time * speed, z);
        transform.position = new Vector3(0, transform.position.y, 0);
    }
}
