using System.Collections;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    public bool fast = false;

    void Start()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        while (transform.childCount > 0)
        {
            if (!GameController.instance.paused)
            {
                if (transform.position.y <= 7.5f)
                {
                    if (!fast)
                    {
                        if (transform.childCount > 5)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime);
                        } else if (transform.childCount == 5)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.01f);
                        } else if (transform.childCount == 4)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.02f);
                        } else if (transform.childCount == 3)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.03f);
                        } else if (transform.childCount == 2)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.04f);
                        } else if (transform.childCount <= 1)
                        {
                            yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.05f);
                        }
                    } else
                    {
                        if (GameController.instance.enemyMoveTime >= 0.18f)
                        {
                            if (transform.childCount > 5)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0175f);
                            } else if (transform.childCount == 5)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.035f);
                            } else if (transform.childCount == 4)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0525f);
                            } else if (transform.childCount == 3)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.07f);
                            } else if (transform.childCount == 2)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0875f);
                            } else if (transform.childCount <= 1)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.105f);
                            }
                        } else
                        {
                            if (transform.childCount > 5)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0125f);
                            } else if (transform.childCount == 5)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.025f);
                            } else if (transform.childCount == 4)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0375f);
                            } else if (transform.childCount == 3)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.05f);
                            } else if (transform.childCount == 2)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.0625f);
                            } else if (transform.childCount <= 1)
                            {
                                yield return new WaitForSeconds(GameController.instance.enemyMoveTime - 0.05f);
                            }
                        }
                    }
                } else
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (transform.position.y <= 7.5f)
                {
                    Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
                    float[] widths = new float[transform.childCount];
                    foreach (Transform enemy in transform)
                    {
                        if (enemy && enemy.CompareTag("Enemy") && enemy.GetComponent<Collider>())
                        {
                            for (int i = 0; i < transform.childCount; i++) widths[i] = enemy.GetComponent<Collider>().bounds.extents.x;
                        }
                    }
                    transform.position += Vector3.right * speed;
                    foreach (Transform enemy in transform)
                    {
                        for (int i = 0; i < widths.Length; i++)
                        {
                            if (enemy.position.x <= screenBounds.x * -1 + widths[i] || enemy.position.x >= screenBounds.x - widths[i])
                            {
                                speed = -speed;
                                transform.position += Vector3.right * speed;
                                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
                            }
                        }
                    }
                } else
                {
                    transform.position += Vector3.down * speed;
                }
            } else
            {
                yield return null;
            }
        }
    }
}