using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public Color normalColor = new Color(1f, 1f, 1f, 1f); // 白色、透明度100%
    public Color hoverColor = new Color(1f, 1f, 1f, 0.5f); // 白色、透明度50%
    public string nextSceneName;

    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null)
        {
            image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null)
        {
            image.color = normalColor;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set.");
        }
    }
}
