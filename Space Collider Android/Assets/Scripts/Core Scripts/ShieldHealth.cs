using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    [SerializeField] private long maxHealth = 0;
    [SerializeField] private Color32 shieldColor = new Color32(0, 100, 255, 255);
    [SerializeField] private Color32 damagedColor = new Color32(0, 75, 170, 255);

    private long health = 0;

    void Awake()
    {
        health = maxHealth;
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
                    part.GetComponent<Renderer>().material.SetColor("_Color", shieldColor);
                } else if (health <= Mathf.Floor(maxHealth) * 0.5)
                {
                    part.GetComponent<Renderer>().material.SetColor("_Color", damagedColor);
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