using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class keyManager : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode Key1 = KeyCode.LeftShift;
    public KeyCode Key2 = KeyCode.P;
    public float fillSpeed1;
    public float decaySpeed1;
    public float fillSpeed2;
    public float decaySpeed2;

    [Header("References")]
    public UnityEngine.UI.Slider Slider1;
    public UnityEngine.UI.Slider Slider2;
    public UnityEngine.UI.Image fillImage1;
    public UnityEngine.UI.Image fillImage2;
    public RawImage FailStateNoStamina;

    [Header("Hold Timer Settings")]
    public Vector2 triggerTimeRange = new Vector2(4f, 5f);
    private float holdTime = 0f;
    private float nextTriggerTime = 0f;
    private bool firstTriggerDone = false;
    private bool secondTriggerDone = false;

    private Color orange = new Color(1f, 0.5f, 0f);

    public float bossRelation=1.0f;

    public void onMistake()
    {
        Debug.Log("onMistake");
        bossRelation -= 0.3f;
    }
    void Update()
    {
        bossRelation = Mathf.Clamp01(bossRelation+Time.deltaTime*0.02f);
        Slider1.maxValue = bossRelation;
        Slider1.GetComponent<RectTransform>().localScale = new Vector3(bossRelation, Slider1.GetComponent<RectTransform>().localScale.y,  1.0f);
        UpdateHoldKeySlider(Slider1, fillImage1, KeyCode.LeftShift, fillSpeed1, decaySpeed1, Color.green, orange, Color.red);
        HandleHoldTimer(KeyCode.LeftShift);
        if (Slider1.value == 0)
        {
            FailStateNoStamina.gameObject.SetActive(true);
        }
        //UpdateHoldKeySlider(Slider2, fillImage2, KeyCode.P, -fillSpeed2, -decaySpeed2,Color.red,orange,Color.green);
    }



    public void UpdateHoldKeySlider(
        UnityEngine.UI.Slider slider,
        UnityEngine.UI.Image fillImage,
        KeyCode keyToHold,
        float fillSpeed,
        float decaySpeed,
        Color minColor,
        Color midColor,
        Color maxColor)
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
            targetColor = Color.Lerp(minColor, midColor, slider.value * 2f);
        }
        else
        {
            targetColor = Color.Lerp(midColor, maxColor, (slider.value - 0.5f) * 2f);
        }

        fillImage.color = Color.Lerp(fillImage.color, targetColor, 10f * Time.deltaTime);
    }
    private void HandleHoldTimer(KeyCode keyToHold)
    {
        if (Input.GetKey(keyToHold))
        {
            holdTime += Time.deltaTime;

            if (holdTime == Time.deltaTime)
            {
                nextTriggerTime = Random.Range(triggerTimeRange.x, triggerTimeRange.y);
                firstTriggerDone = false;
                secondTriggerDone = false;
            }

            // First trigger (after random 4–5s)
            if (!firstTriggerDone && holdTime >= nextTriggerTime)
            {
               // Debug.Log("?? UGH");
                firstTriggerDone = true;
            }

            // Second trigger (1s after first)
            if (firstTriggerDone && !secondTriggerDone && holdTime >= nextTriggerTime + 0.5f)
            {
               // Debug.Log("?? KASZEL");
                secondTriggerDone = true;
            }
        }
        else
        {
            //Debug.Log("?? UFFF");
            holdTime = 0f;
            firstTriggerDone = false;
            secondTriggerDone = false;
        }

    }
    public bool SecondTriggerReached => secondTriggerDone;
}
