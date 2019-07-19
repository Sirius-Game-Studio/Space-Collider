using UnityEngine;

public class Mover : MonoBehaviour
{
    [Tooltip("How fast this object moves.")] public float speed = 0;
    [Tooltip("Where the object should move towards, if it's not using transform.forward.")] [SerializeField] private Vector2 movement = Vector2.zero;
    [Tooltip("Should this object move, using transform.forward?")] [SerializeField] private bool useForwardMovement = true;

    void Update()
    {
        if (useForwardMovement) //Moves the object using transform.forward
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        } else //Moves the object using set movement values
        {
            transform.position += new Vector3(movement.x, movement.y, 0) * speed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}