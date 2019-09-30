using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
    void Start()
    {
        if (!GameController.instance.isCampaignLevel && GameController.instance.difficulty >= 4) StartCoroutine(main());
    }

    IEnumerator main()
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