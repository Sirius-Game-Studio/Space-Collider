using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    [Tooltip("Amount of health this shield starts with.")] [SerializeField] private long health = 0;

    private long maxHealth = 0;

    void Awake()
    {
        maxHealth = health;
    }

    void Update()
    {
        if (health <= 0) Destroy(gameObject);
        foreach (Transform part in transform)
        {
            if (part.GetComponent<Renderer>())
            {
                if (health > Mathf.Floor(maxHealth) * 0.5)
                {
                    part.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 170, 255, 255));
                } else if (health <= Mathf.Floor(maxHealth) * 0.5)
                {
                    part.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 100, 200, 255));
                }
            }
        }
    }

    public void takeDamage(long damage)
    {
        if (damage > 0)
        {
            health -= damage;
        } else
        {
            --damage;
        }
    }

}
