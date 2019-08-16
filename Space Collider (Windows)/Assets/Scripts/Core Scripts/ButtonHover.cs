using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private Color32 normalColor = new Color32(200, 200, 200, 255);
    [SerializeField] private Color32 hoverColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Text[] textsToShow = new Text[0];

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.color = normalColor;
        if (textsToShow.Length > 0)
        {
            foreach (Text t in textsToShow) if (t) t.enabled = false;
        }
    }

    public void OnMouseEnter()
    {
        text.color = hoverColor;
        if (textsToShow.Length > 0)
        {
            foreach (Text t in textsToShow) if (t) t.enabled = true;
        }
    }

    public void OnMouseExit()
    {
        text.color = normalColor;
        if (textsToShow.Length > 0)
        {
            foreach (Text t in textsToShow) if (t) t.enabled = false;
        }
    }
}
