using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonScript : MonoBehaviour
{
    private Button button;
    private bool hasHovered = false;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasHovered)
        {
            hasHovered = true;
            AudioManager.instance.PlayOneShot(FmodEvents.instance.Hover, new Vector3(0, 0, 0));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hasHovered = true;
    }

    private void OnClicked()
    {
        Debug.Log("Button clicked!");
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Click, new Vector3(0, 0, 0));
    }
}
