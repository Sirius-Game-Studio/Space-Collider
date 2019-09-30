using System.Collections;
using UnityEngine;

public class EnemyShipGun : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The amount of damage dealt to the player's Ship Armor.")] [SerializeField] private long damage = 1;
    [SerializeField] private float fireRate = 0.3375f;
    [Tooltip("The speed at which fired bullets travel at (cannot use positive values).")] [SerializeField] private float bulletSpeed = -12.5f;

    [Header("Setup")]
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private AudioClip fireSound = null;

    private AudioSource audioSource;
    private float nextShot = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!GameController.instance.isCampaignLevel)
        {
            if (GameController.instance.difficulty <= 1) // Easy
            {
                fireRate += 0.025f;
            } else if (GameController.instance.difficulty >= 3) //Hard & NIGHTMARE!
            {
                fireRate -= 0.025f;
            }
        }
    }

    void Update()
    {
        if (!GameController.instance.gameOver && !GameController.instance.won && !GameController.instance.paused)
        {
            if (nextShot < fireRate)
            {
                nextShot += Time.deltaTime;
            } else
            {
                nextShot = 0;
                bool foundBulletSpawns = false;
                foreach (Transform bulletSpawn in transform)
                {
                    if (bulletSpawn.CompareTag("BulletSpawn") && bulletSpawn.gameObject.activeSelf)
                    {
                        GameObject newBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                        newBullet.transform.position = new Vector3(newBullet.transform.position.x, newBullet.transform.position.y, 0);
                        newBullet.GetComponent<EnemyBulletHit>().damage = damage;
                        newBullet.GetComponent<Mover>().speed = bulletSpeed;
                        foundBulletSpawns = true;
                    }
                }
                if (!foundBulletSpawns)
                {
                    GameObject newBullet = Instantiate(bullet, transform.position - new Vector3(0, 0.0875f, 0), transform.rotation);
                    newBullet.transform.position = new Vector3(newBullet.transform.position.x, newBullet.transform.position.y, 0);
                    if (newBullet.transform.rotation.x != -90) newBullet.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    newBullet.GetComponent<EnemyBulletHit>().damage = damage;
                    newBullet.GetComponent<Mover>().speed = bulletSpeed;
                    foundBulletSpawns = true;
                }
                if (foundBulletSpawns && audioSource)
                {
                    if (fireSound)
                    {
                        audioSource.PlayOneShot(fireSound);
                    } else
                    {
                        audioSource.Play();
                    }
                }
            }
        }
        if (damage < 1) damage = 1; //Checks if damage is below 1
        if (bulletSpeed >= 0) bulletSpeed = -12.5f; //Checks if bulletSpeed is 0 or above
    }
}