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
        // Prevent game from starting before tutorial
        Boss.canMove = false;
        TimeManager.PauseTime();
        tutorialManager.BeginTutorialDay(daycounter);
    }

    public void FinishDay()
    {
        if (daycounter < 5)
            WinState.gameObject.SetActive(true);
        else
            PAPA.gameObject.SetActive(true);

        Boss.transform.position = Boss.points[0].position;
        KeyManager.Slider1.value = 1;
        KeyManager.decaySpeed1 += 0.015f;
        StartCoroutine(TurnOffWinM());

        Boss.canMove = false;
        TimeManager.PauseTime();
        daycounter += 1;
        // Do not advance the day automatically; tutorial controls it
    }

    private IEnumerator TurnOffWinM()
    {
        yield return new WaitForSeconds(4);
        WinState.gameObject.SetActive(false);
        tutorialManager.BeginTutorialDay(daycounter);
    }

    public void BeginNewDay()
    {
        Boss.canMove = true;
        Boss.moveInterval = Mathf.Max(0.5f, Boss.moveInterval - 0.5f);
        KeyManager.Slider1.value = 1;
        daytext.text = "DAY " + daycounter.ToString();
    }
}
