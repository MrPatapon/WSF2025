using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int startHour = 7;
    [SerializeField] private int totalHours = 9;
    [SerializeField] private Vector2 hourDurationRange = new Vector2(60f, 90f);

    public int currentHour;
    private Coroutine timeRoutine;
    private DayManager dayManager;
    public OfficeTaskManager officeTaskManager;

    private bool timePaused = true;

    private void Start()
    {
        dayManager = GetComponent<DayManager>();
        currentHour = startHour;
        UpdateTimeText();
        StartNewDay();
    }

    public void StartNewDay()
    {
        if (timeRoutine != null)
            StopCoroutine(timeRoutine);

        currentHour = startHour;
        UpdateTimeText();
        timePaused = true; // paused until tutorial ends
        //officeTaskManager.StartDay(dayManager.daycounter-1);
    }

    public void ResumeTime()
    {
        if (timeRoutine != null)
            StopCoroutine(timeRoutine);

        timePaused = false;
        timeRoutine = StartCoroutine(TimeRoutine());
    }

    public void PauseTime()
    {
        timePaused = true;
        if (timeRoutine != null)
        {
            StopCoroutine(timeRoutine);
            timeRoutine = null;
        }
    }

    private IEnumerator TimeRoutine()
    {
        for (int i = 1; i < totalHours; i++)
        {
            float waitTime = Random.Range(hourDurationRange.x, hourDurationRange.y);
            float elapsed = 0f;

            while (elapsed < waitTime)
            {
                if (!timePaused)
                    elapsed += Time.deltaTime;
                yield return null;
            }

            if (timePaused)
                yield break;

            currentHour++;
            UpdateTimeText();
        }

        if (!timePaused && dayManager != null)
        {
            dayManager.FinishDay();
            officeTaskManager.EndDay();
        }
    }
        private void UpdateTimeText()
    {
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        string period = currentHour >= 12 ? "PM" : "AM";
        timeText.text = $"{displayHour} {period}";
    }
        public void SetHour(int hour)
        {
            currentHour = hour;
            UpdateTimeText();
        }
    }
