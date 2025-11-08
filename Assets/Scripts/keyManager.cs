using UnityEngine;
using UnityEngine.UI;
public class keyManager : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode Key1 = KeyCode.Space;
    public KeyCode Key2 = KeyCode.P;
    public float fillSpeed1;
    public float decaySpeed1;
    public float fillSpeed2;
    public float decaySpeed2;

    [Header("References")]
    public Slider Slider1;
    public Slider Slider2;
    public Image fillImage1;
    public Image fillImage2;  

    private Color red = Color.red;
    private Color orange = new Color(1f, 0.5f, 0f);
    private Color green = Color.green;
    void Update()
    {
        UpdateHoldKeySlider(Slider1, fillImage1, KeyCode.Space, fillSpeed1, decaySpeed1);
        UpdateHoldKeySlider(Slider2, fillImage2, KeyCode.P, -fillSpeed2, -decaySpeed2);
    }

    public void UpdateHoldKeySlider(
        Slider slider,
        Image fillImage,
        KeyCode keyToHold,
        float fillSpeed,
        float decaySpeed)
    {
        if (slider == null || fillImage == null) return;

        // Update slider value based on key state
        if (Input.GetKey(keyToHold))
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
        else
        {
            slider.value -= decaySpeed * Time.deltaTime;
        }

        slider.value = Mathf.Clamp01(slider.value);    
        

        Color targetColor;

        if (slider.value < 0.5f)
        {
            targetColor = Color.Lerp(red, orange, slider.value * 2f);
        }
        else
        {
            targetColor = Color.Lerp(orange, green, (slider.value - 0.5f) * 2f);
        }

        fillImage.color = Color.Lerp(fillImage.color, targetColor, 10f * Time.deltaTime);
    }
}
