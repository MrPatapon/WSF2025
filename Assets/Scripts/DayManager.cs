using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DayManager : MonoBehaviour
{
    [SerializeField] private RawImage WinState;
    [SerializeField] private RawImage PAPA;
    public BossMovement Boss;
    public GameObject TaskManager;
    public TimeManager TimeManager;
    public keyManager KeyManager;
    public TMP_Text daytext;

    private int daycounter = 1;

    public void FinishDay()
    {
        
        if (daycounter < 5)
            WinState.gameObject.SetActive(true);
        else
            PAPA.gameObject.SetActive(true);
        Boss.transform.position = Boss.points[0].position;
        StopCoroutine(Boss.MovementLoop());
        KeyManager.Slider1.value = 1;
        KeyManager.decaySpeed1 += 0.015f;
        StartCoroutine(BeginNewDay());
    }

    private IEnumerator BeginNewDay()
    {
        yield return new WaitForSeconds(3f);
        WinState.gameObject.SetActive(false);
        StartCoroutine(Boss.MovementLoop());
        Boss.moveInterval -= 0.5f;
        KeyManager.Slider1.value = 1;
        daycounter++;
        TimeManager.StartNewDay();
    }
}