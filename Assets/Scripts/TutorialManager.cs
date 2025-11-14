using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("Message Bubbles per Day")]
    [SerializeField] private GameObject[] day1Bubbles;
    [SerializeField] private GameObject[] day2Bubbles;
    [SerializeField] private GameObject[] day3Bubbles;
    [SerializeField] private GameObject[] day4Bubbles;
    [SerializeField] private GameObject[] day5Bubbles;

    [Header("References")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private keyManager keyManager;

    [Header("Tutorial Buttons (Images)")]
    [SerializeField] private List<GameObject> tutorialButtons;

    private List<GameObject> messageBubbles = new List<GameObject>();
    private int currentStep = 0; // index of the first bubble in the current pair
    private int currentDay = 1;

    private void Start()
    {
        tutorialCanvas.SetActive(false);
        BeginTutorialDay(currentDay);
    }

    public void BeginTutorialDay(int day)
    {
        timeManager.officeTaskManager.turn_off();
        currentDay = day;
        tutorialCanvas.SetActive(true);

        // Pause game logic
        timeManager.PauseTime();
        dayManager.Boss.canMove = false;
        keyManager.isTutFinished = false;

        // --- NEW: Disable all bubbles from any previous day ---
        foreach (var bubble in day1Bubbles) if (bubble) bubble.SetActive(false);
        foreach (var bubble in day2Bubbles) if (bubble) bubble.SetActive(false);
        foreach (var bubble in day3Bubbles) if (bubble) bubble.SetActive(false);
        foreach (var bubble in day4Bubbles) if (bubble) bubble.SetActive(false);
        foreach (var bubble in day5Bubbles) if (bubble) bubble.SetActive(false);

        // Clear current message list
        messageBubbles.Clear();

        // Add current day’s messages
        switch (day)
        {
            case 1: messageBubbles.AddRange(day1Bubbles); break;
            case 2: messageBubbles.AddRange(day2Bubbles); break;
            case 3: messageBubbles.AddRange(day3Bubbles); break;
            case 4: messageBubbles.AddRange(day4Bubbles); break;
            case 5: messageBubbles.AddRange(day5Bubbles); break;
        }

        foreach (var bubble in messageBubbles)
            bubble.SetActive(false);

        currentStep = 0;
        ShowNextPair();
    }

    private void ShowNextPair()
    {
        // Hide all first
        foreach (var b in messageBubbles)
            if (b) b.SetActive(false);

        // Show bubble 1
        if (currentStep < messageBubbles.Count)
            if (messageBubbles[currentStep])
                messageBubbles[currentStep].SetActive(true);

        // Show bubble 2
        if (currentStep + 1 < messageBubbles.Count)
            if (messageBubbles[currentStep + 1])
                messageBubbles[currentStep + 1].SetActive(true);

        // Activate tutorial buttons because bubbles are visible
        SetTutorialButtonsActive(true);
    }

    public void GetNextMessage()
    {
        currentStep += 2; // move to the next pair

        if (currentStep < messageBubbles.Count)
        {
            ShowNextPair();
        }
        else
        {
            EndTutorial();
        }
    }

    public void EndTutorial()
    {
        tutorialCanvas.SetActive(false);

        // Resume game logic
        timeManager.ResumeTime();
        dayManager.Boss.canMove = true;
        keyManager.isTutFinished = true;
        timeManager.officeTaskManager.StartDay(dayManager.daycounter - 1);
        dayManager.BeginNewDay(); // resume the same day
        SetTutorialButtonsActive(false);
    }

    private void Update()
    {
        if (tutorialCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                GetNextMessage();

            if (Input.GetKeyDown(KeyCode.X))
                EndTutorial();
        }
    }

    private void SetTutorialButtonsActive(bool active)
    {
        foreach (var btn in tutorialButtons)
            if (btn) btn.SetActive(active);
    }
}
