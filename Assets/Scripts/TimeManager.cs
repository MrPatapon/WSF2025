using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int startHour = 7;
    [SerializeField] private int totalHours = 8;
    [SerializeField] private Vector2 hourDurationRange = new Vector2(1f, 2f);

    private int currentHour;
    void Start()
    {
        currentHour = startHour;
        UpdateTimeText();
        StartCoroutine(TimeRoutine());
    }

    private IEnumerator TimeRoutine()
    {
        for (int i = 1; i < totalHours; i++)
        {
            float waitTime = Random.Range(hourDurationRange.x, hourDurationRange.y);
            yield return new WaitForSeconds(waitTime);

            currentHour++;
            UpdateTimeText();
        }
    }
    private void UpdateTimeText()
    {
        // Handle wrap-around past 12 (for AM/PM formatting if needed)
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        string period = currentHour >= 12 ? "PM" : "AM";

        timeText.text = $"{displayHour} {period}";
    }
}
