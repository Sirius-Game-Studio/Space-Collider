using System.Collections;
using UnityEngine;

/*
Blink:
Randomly disappears and then reappears (time varies).
*/

public class Blink : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        if (!CompareTag("Enemy"))
        {
            Debug.LogError("GameObject " + name + " is not tagged as Enemy!");
            return;
        }
        if (gameController.isStandard) return;
        if (!gameController.isStandard && gameController.difficulty < 4) return;
        StartCoroutine(ability());
    }

    IEnumerator ability() //Ability code
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1));
            foreach (Transform obj in transform)
            {
                if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = false;
            }
            yield return new WaitForSeconds(Random.Range(0.75f, 1.5f));
            foreach (Transform obj in transform)
            {
                if (obj.GetComponent<Renderer>()) obj.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
