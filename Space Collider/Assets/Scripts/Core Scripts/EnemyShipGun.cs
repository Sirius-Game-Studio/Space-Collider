using System.Collections;
using UnityEngine;

public class EnemyShipGun : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Amount of damage dealt to the player. (only affects armor)")] [SerializeField] private long damage = 1;
    [Tooltip("How fast this enemy shoots.")] [SerializeField] private float fireRate = 0.3375f;
    [Tooltip("The speed at which fired bullets travel at (cannot use positive values).")] [SerializeField] private float bulletSpeed = -12.5f;

    [Header("Setup")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip fireSound;

    private AudioSource audioSource;
    private GameController gameController;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        if (!gameController.isStandard)
        {
            if (gameController.difficulty <= 1)
            {
                fireRate += 0.0125f;
            } else if (gameController.difficulty >= 3)
            {
                fireRate -= 0.025f;
            }
        }
        StartCoroutine("shoot");
    }

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
        if (bulletSpeed >= 0) bulletSpeed = -12.5f; //Checks if bulletSpeed is 0 or above
    }

    IEnumerator shoot()
    {
        while (!gameController.gameOver && !gameController.won)
        {
            if (!gameController.paused)
            {
                yield return new WaitForSeconds(fireRate);
                if (!gameController.paused && !gameController.gameOver && !gameController.won)
                {
                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, 0));
                    newBullet.GetComponent<EnemyBulletHit>().damage = damage;
                    newBullet.GetComponent<Mover>().speed = bulletSpeed;
                    if (newBullet.transform.rotation.x != -90) newBullet.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    if (audioSource && fireSound) audioSource.PlayOneShot(fireSound, PlayerPrefs.GetFloat("SoundVolume"));
                }
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
