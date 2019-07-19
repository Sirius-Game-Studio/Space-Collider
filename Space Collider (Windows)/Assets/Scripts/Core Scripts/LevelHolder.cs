using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public static LevelHolder instance;

    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevels = 13;

    void Awake()
    {
        PlayerPrefs.SetInt("StandardLevel", level);
        PlayerPrefs.SetInt("MaxCampaignLevels", maxLevels);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
