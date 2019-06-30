using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public static LevelHolder instance;

    public int level = 1;
    public int maxLevels = 10;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.SetInt("StandardLevel", level);
        PlayerPrefs.Save();
    }
}
