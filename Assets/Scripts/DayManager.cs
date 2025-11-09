using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DayManager : MonoBehaviour
{
    [SerializeField] private RawImage WinState;
    [SerializeField] private RawImage PAPA;
    public TutorialManager tutorialManager;
    public BossMovement Boss;
    public GameObject TaskManager;
    public TimeManager TimeManager;
    public keyManager KeyManager;
    public TMP_Text daytext;

    public int daycounter = 1;

    private void Start()
    {
        // Pause everything at start; tutorial will start automatically
        Boss.canMove = false;
        TimeManager.PauseTime();
        daytext.text = "DAY " + daycounter;
        tutorialManager.BeginTutorialDay(daycounter);
    }

    public void FinishDay()
    {
        if (daycounter < 5)
        {
            WinState.gameObject.SetActive(true);
            AudioManager.instance.PlayOneShot(FmodEvents.instance.Success, transform.position);
            AudioManager.instance.PlayOneShot(FmodEvents.instance.EndDayPhone, transform.position);
        }
            
        else
            PAPA.gameObject.SetActive(true);

        Boss.transform.position = Boss.points[0].position;
        KeyManager.Slider1.value = 1;
        KeyManager.decaySpeed1 += 0.015f;

        Boss.canMove = false;
        TimeManager.PauseTime();

        StartCoroutine(TurnOffWinM());
    }

    private IEnumerator TurnOffWinM()
    {
        yield return new WaitForSeconds(4f);
        WinState.gameObject.SetActive(false);

        // --- THIS IS THE CRUCIAL PART ---
        // Increment the day BEFORE tutorial
        daycounter++;
        daytext.text = "DAY " + daycounter;

        // Reset time to 9 AM
        TimeManager.SetHour(9);

        // Start tutorial for the new day
        tutorialManager.BeginTutorialDay(daycounter);
    }

    public void BeginNewDay()
    {
        Boss.canMove = true;
        Boss.moveInterval = Mathf.Max(0.5f, Boss.moveInterval - 0.5f);
        KeyManager.Slider1.value = 1;
        // Do NOT increment day here! It's handled in FinishDay
    }
}