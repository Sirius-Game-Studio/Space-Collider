using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct PowerupSettings
{
    [Header("Powerup Changes")]
    [Tooltip("Contains bullet spawns used by Multishot powerup.")] public GameObject[] multishotBulletSpawns;
    [Tooltip("The amount of damage added after collecting this powerup.")] public long increasedDamageValue;
    [Tooltip("The amount of armor received after collecting this powerup.")] public long shipArmorValue;
    [Tooltip("The amount of fire rate added after collecting this powerup.")] public float fasterFiringSubtractedValue;
    [Tooltip("The amount of speed added after collecting this powerup.")] public float fasterSpeedAddedValue;

    [Header("Powerup Time")]
    public float multishotTime;
    public float increasedDamageTime;
    public float fasterFiringTime;
    public float fasterSpeedTime;
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Cannot be lower than 1.")] [Range(1, 5)] public long maxLives = 3;
    [Tooltip("Cannot be lower than 1.")] [SerializeField] private long damage = 1;
    [Tooltip("Cannot be lower than 0.2.")] [SerializeField] private float fireRate = 0.5f;
    [Tooltip("Ccannot be lower than 0.")] [SerializeField] private float speed = 5;
    [SerializeField] private float invulnerabilityTime = 1.6f;
    [SerializeField] private PowerupSettings powerupSettings;

    [Header("UI")]
    [SerializeField] private Text shipArmorText = null;

    [Header("Miscellanous")]
    public long lives = 0;
    [Tooltip("Ship Armor.")] public long armor = 0;
    public bool invulnerable = false;

    [Header("Setup")]
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject explosion = null;
    [SerializeField] private AudioClip fireSound = null;

    private AudioSource audioSource;
    private GameController gameController;
    private bool hasMultishot = false, hasIncreasedDamage = false, hasfasterFiring = false, hasFasterSpeed = false;
    private bool doFadeEffect = false;
    private float nextShot = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        if (maxLives < 1) maxLives = 1; //Checks if maxLives is below 1
        lives = maxLives;
        armor = 0;
        invulnerable = false;
    }

    void Update()
    {
        if (maxLives < 1) maxLives = 1; //Checks if maxLives is below 1
        if (lives <= 0)
        {
            stopMultishot();
            stopIncreasedDamage();
            armor = 0;
            stopfasterFiring();
            stopFasterSpeed();
            gameController.gameOver = true;
            setKey("Deaths");
            if (gameController.isStandard) setKey("Loses");
            PlayerPrefs.Save();
            Destroy(gameObject);
        }
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float width = GetComponent<Collider>().bounds.extents.x;
        if (!gameController.gameOver && !gameController.won && !gameController.paused)
        {
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            movement = movement.normalized * speed * Time.deltaTime;
            transform.position += movement;
            if (Input.GetButton("Shoot") && nextShot >= fireRate)
            {
                bool foundBulletSpawns = false;
                nextShot = 0;
                foreach (Transform bulletSpawn in transform)
                {
                    if (bulletSpawn.CompareTag("BulletSpawn") && bulletSpawn.gameObject.activeSelf)
                    {
                        GameObject newBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                        newBullet.GetComponent<BulletHit>().damage = damage;
                        if (hasIncreasedDamage) newBullet.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0.5f, 0));
                        foundBulletSpawns = true;
                    }
                }
                if (!foundBulletSpawns)
                {
                    GameObject newBullet = Instantiate(bullet, transform.position + new Vector3(0, 1.05f, 0), transform.rotation);
                    newBullet.transform.position = new Vector3(newBullet.transform.position.x, newBullet.transform.position.y, 0);
                    if (newBullet.transform.rotation.x != -90) newBullet.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    newBullet.GetComponent<BulletHit>().damage = damage;
                    if (hasIncreasedDamage) newBullet.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0.5f, 0));
                    foundBulletSpawns = true;
                }
                if (foundBulletSpawns && audioSource)
                {
                    if (fireSound)
                    {
                        audioSource.PlayOneShot(fireSound, PlayerPrefs.GetFloat("SoundVolume"));
                    } else
                    {
                        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
                        audioSource.Play();
                    }
                }
            }
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, screenBounds.x * -1 + width, screenBounds.x - width), -7.375f, 0);
        if (!hasMultishot)
        {
            foreach (Transform bulletSpawn in transform)
            {
                if (bulletSpawn.CompareTag("BulletSpawn") && bulletSpawn.name != "FrontBulletSpawn")
                {
                    bulletSpawn.gameObject.SetActive(false);
                }
            }
        }
        if (nextShot < fireRate) nextShot += Time.deltaTime;
        if (armor > 0)
        {
            shipArmorText.text = "Ship Armor: " + armor + "/" + powerupSettings.shipArmorValue;
        } else
        {
            shipArmorText.text = "";
        }
        if (lives < 0) //Checks if lives are below 0
        {
            lives = 0;
        } else if (lives >= maxLives) //Checks if lives are above maxLives
        {
            lives = maxLives;
        }
        if (damage < 1) damage = 1; //Checks if damage is below 1
        if (fireRate < 0.2f) fireRate = 0.2f; //Checks if fireRate is below 0.2
        if (speed < 0) speed = 0; //Checks if speed is below 0
        if (armor < 0) //Checks if armor is below 0
        {
            armor = 0;
        } else if (armor >= powerupSettings.shipArmorValue) //Checks if armor is above shipArmorValue
        {
            armor = powerupSettings.shipArmorValue;
        }
    }

    public void onHit(long damage, bool instakill)
    {
        if (!invulnerable && !gameController.won)
        {
            if (!instakill)
            {
                if (armor <= 0)
                {
                    if (lives > 0)
                    {
                        --lives;
                        if (explosion)
                        {
                            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                            if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                        }
                        invulnerable = true;
                        StartCoroutine("fadeEffect");
                        Invoke("stopInvulnerability", invulnerabilityTime);
                    }
                } else
                {
                    if (damage > 0)
                    {
                        armor -= damage;
                    } else
                    {
                        --armor;
                    }
                    invulnerable = true;
                    StartCoroutine("fadeEffect");
                    Invoke("stopInvulnerability", 0.2f);
                }
            } else
            {
                lives = 0;
                if (explosion)
                {
                    GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                    if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                }
            }
        }
    }

    public void multishot()
    {
        if (!hasMultishot)
        {
            if (powerupSettings.multishotBulletSpawns.Length > 0)
            {
                hasMultishot = true;
                foreach (GameObject bulletSpawn in powerupSettings.multishotBulletSpawns)
                {
                    if (bulletSpawn.CompareTag("BulletSpawn")) bulletSpawn.SetActive(true);
                }
                gameController.showMessage("Multiple Bullets");
                CancelInvoke("stopMultishot");
                Invoke("stopMultishot", powerupSettings.multishotTime);
                gameController.addScore(5);
            } else
            {
                Debug.LogError("multishotBulletSpawns must have a element in order for Multishot powerup to work.");
            }
        } else if (hasMultishot)
        {
            CancelInvoke("stopMultishot");
            Invoke("stopMultishot", powerupSettings.multishotTime);
            gameController.addScore(5);
        }
    }

    public void increasedDamage()
    {
        if (!hasIncreasedDamage)
        {
            if (powerupSettings.increasedDamageValue > 0)
            {
                hasIncreasedDamage = true;
                damage += powerupSettings.increasedDamageValue;
                gameController.showMessage("Increased Bullet Damage");
                CancelInvoke("stopIncreasedDamage");
                Invoke("stopIncreasedDamage", powerupSettings.increasedDamageTime);
                gameController.addScore(5);
            } else
            {
                Debug.LogError("Negative values cannot be used to set the damage value.");
            }
        } else
        {
            CancelInvoke("stopIncreasedDamage");
            Invoke("stopIncreasedDamage", powerupSettings.increasedDamageTime);
            gameController.addScore(5);
        }
    }

    public void shipArmor()
    {
        if (armor <= 0)
        {
            if (powerupSettings.shipArmorValue > 0)
            {
                armor = powerupSettings.shipArmorValue;
                gameController.showMessage("You got Ship Armor!");
                gameController.addScore(10);
            } else
            {
                Debug.LogError("Negative values cannot be used to set the armor value.");
            }
        } else if (armor > 0 && armor < powerupSettings.shipArmorValue)
        {
            ++armor;
            if (armor < powerupSettings.shipArmorValue) gameController.showMessage("Ship Armor restored.");
            gameController.addScore(5);
        }
    }

    public void fasterFiring()
    {
        if (!hasfasterFiring)
        {
            if (powerupSettings.fasterFiringSubtractedValue < 0)
            {
                hasfasterFiring = true;
                fireRate += powerupSettings.fasterFiringSubtractedValue;
                gameController.showMessage("Faster Firing");
                CancelInvoke("stopfasterFiring");
                Invoke("stopfasterFiring", powerupSettings.fasterFiringTime);
                gameController.addScore(5);
            } else
            {
                Debug.LogError("Positive values cannot be used to set the fire rate value.");
            }
        } else
        {
            CancelInvoke("stopfasterFiring");
            Invoke("stopfasterFiring", powerupSettings.fasterFiringTime);
            gameController.addScore(5);
        }
    }

    public void fasterSpeed()
    {
        if (!hasFasterSpeed)
        {
            if (powerupSettings.fasterSpeedAddedValue > 0)
            {
                hasFasterSpeed = true;
                speed += powerupSettings.fasterSpeedAddedValue;
                gameController.showMessage("Faster Ship Speed");
                CancelInvoke("stopFasterSpeed");
                Invoke("stopFasterSpeed", powerupSettings.fasterSpeedTime);
                gameController.addScore(5);
            } else
            {
                Debug.LogError("Negative values cannot be used to set the speed value.");
            }
        } else
        {
            CancelInvoke("stopFasterSpeed");
            Invoke("stopFasterSpeed", powerupSettings.fasterSpeedTime);
            gameController.addScore(5);
        }
    }

    void stopMultishot()
    {
        hasMultishot = false;
        foreach (Transform bulletSpawn in transform)
        {
            if (bulletSpawn.CompareTag("BulletSpawn") && bulletSpawn.name != "FrontBulletSpawn")
            {
                bulletSpawn.gameObject.SetActive(false);
            }
        }
    }

    void stopIncreasedDamage()
    {
        if (hasIncreasedDamage)
        {
            hasIncreasedDamage = false;
            damage -= powerupSettings.increasedDamageValue;
        }
    }

    void stopfasterFiring()
    {
        if (hasfasterFiring)
        {
            hasfasterFiring = false;
            fireRate -= powerupSettings.fasterFiringSubtractedValue;
        }
    }

    void stopFasterSpeed()
    {
        if (hasFasterSpeed)
        {
            hasFasterSpeed = false;
            speed -= powerupSettings.fasterSpeedAddedValue;
        }
    }

    void stopInvulnerability()
    {
        if (invulnerable)
        {
            invulnerable = false;
            doFadeEffect = false;
            StopCoroutine("fadeEffect");
            setPlayerVisibility(true);
        }
    }

    IEnumerator fadeEffect()
    {
        if (body)
        {
            doFadeEffect = true;
            while (doFadeEffect)
            {
                setPlayerVisibility(false);
                yield return new WaitForSeconds(0.1f);
                setPlayerVisibility(true);
                yield return new WaitForSeconds(0.1f);
            }
            setPlayerVisibility(true);
        } else
        {
            doFadeEffect = false;
            setPlayerVisibility(true);
            StopCoroutine("fadeEffect");
            Debug.LogError("body must be set to a GameObject for the fade effect to work.");
        }
    }

    void setPlayerVisibility(bool state)
    {
        if (body && body.transform.parent.CompareTag("Player"))
        {
            if (state)
            {
                body.SetActive(true);
            } else
            {
                body.SetActive(false);
            }
        } else
        {
            Debug.LogError("body must be set to a GameObject tagged as Player in order to set visibility.");
        }
    }

    void setKey(string key)
    {
        if (key != "")
        {
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetString(key, "1");
            } else
            {
                long plus = long.Parse(PlayerPrefs.GetString(key));
                ++plus;
                PlayerPrefs.SetString(key, plus.ToString());
            }
            PlayerPrefs.Save();
        }
    }
}