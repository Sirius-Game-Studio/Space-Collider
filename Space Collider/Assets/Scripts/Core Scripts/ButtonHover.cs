using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private Color32 normalColor = new Color32(200, 200, 200, 255);
    [SerializeField] private Color32 hoverColor = new Color32(255, 255, 255, 255);
    [Tooltip("List of texts to show when hovering over the button.")] [SerializeField] private Text[] textsToShow;

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void OnMouseEnter()
    {
        text.color = hoverColor;
        if (textsToShow.Length > 0) foreach (Text t in textsToShow) t.enabled = true;
    }

    public void OnMouseExit()
    {
        text.color = normalColor;
        if (textsToShow.Length > 0) foreach (Text t in textsToShow) t.enabled = false;
    }
}
