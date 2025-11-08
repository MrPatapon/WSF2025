using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int startHour = 7;
    [SerializeField] private int totalHours = 9;
    [SerializeField] private Vector2 hourDurationRange = new Vector2(60f, 90f); // seconds

    public int currentHour;
    private Coroutine timeRoutine;

    private DayManager dayManager;

    private void Start()
    {
        dayManager = GetComponent<DayManager>();
        StartNewDay();
    }

    public void StartNewDay()
    {
        // stop previous coroutine if running
        if (timeRoutine != null)
            StopCoroutine(timeRoutine);

        currentHour = startHour;
        UpdateTimeText();
        timeRoutine = StartCoroutine(TimeRoutine());
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

        // when the day ends
        if (dayManager != null)
            dayManager.FinishDay();
    }

    private void UpdateTimeText()
    {
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        string period = currentHour >= 12 ? "PM" : "AM";
        timeText.text = $"{displayHour} {period}";
    }
}