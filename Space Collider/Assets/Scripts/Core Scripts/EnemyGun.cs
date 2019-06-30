using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Amount of damage dealt to the player (only affects armor).")] [SerializeField] private long damage = 1;
    [Tooltip("The speed at which fired bullets travel at (cannot use positive values).")] [SerializeField] private float bulletSpeed = -12.5f;
    [Tooltip("Should this alien group shoot faster?")] public bool fast = false;

    [Header("Setup")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip fireSound;

    private AudioSource audioSource;
    private GameController gameController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        bulletSpeed -= gameController.bulletSpeedIncrement.x;
        StartCoroutine(shoot());
    }

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
        if (bulletSpeed >= 0) bulletSpeed = -12.5f; //Checks if bulletSpeed is 0 or above
    }

    IEnumerator shoot()
    {
        while (transform.childCount > 0 && !gameController.gameOver && !gameController.won)
        {
            if (!gameController.paused)
            {
                if (transform.position.y <= 7.5f)
                {
                    if (!fast)
                    {
                        if (transform.childCount > 5)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x));
                        } else if (transform.childCount == 5)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x - 0.02f));
                        } else if (transform.childCount == 4)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x - 0.04f));
                        } else if (transform.childCount == 3)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x - 0.06f));
                        } else if (transform.childCount == 2)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x - 0.08f));
                        } else if (transform.childCount <= 1)
                        {
                            yield return new WaitForSeconds(Random.Range(0.5f, gameController.fireRate.x - 0.1f));
                        }
                    } else
                    {
                        if (transform.childCount > 5)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.035f));
                        } else if (transform.childCount == 5)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.07f));
                        } else if (transform.childCount == 4)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.105f));
                        } else if (transform.childCount == 3)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.14f));
                        } else if (transform.childCount == 2)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.175f));
                        } else if (transform.childCount <= 1)
                        {
                            yield return new WaitForSeconds(Random.Range(0.25f, gameController.fireRate.x - 0.21f));
                        }
                    }
                    if (transform.childCount > 0 && !gameController.gameOver && !gameController.won && !gameController.paused)
                    {
                        //Gets children and picks a random one for shooting
                        bool foundBulletSpawns = false;
                        Transform[] enemies = GetComponentsInChildren<Transform>();
                        Transform enemy = enemies[Random.Range(0, enemies.Length)];
                        foreach (Transform bulletSpawn in enemy.parent.transform)
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
                            GameObject newBullet = Instantiate(bullet, enemy.position - new Vector3(0, 1, 0), enemy.rotation);
                            newBullet.transform.position = new Vector3(newBullet.transform.position.x, newBullet.transform.position.y, 0);
                            if (newBullet.transform.rotation.x != 90) newBullet.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            newBullet.GetComponent<EnemyBulletHit>().damage = damage;
                            newBullet.GetComponent<Mover>().speed = bulletSpeed;
                            foundBulletSpawns = true;
                        }
                        if (foundBulletSpawns && audioSource && fireSound) audioSource.PlayOneShot(fireSound, PlayerPrefs.GetFloat("SoundVolume"));
                    }
                } else
                {
                    yield return new WaitForEndOfFrame();
                }
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}