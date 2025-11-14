using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class keyManager : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode Key1 = KeyCode.LeftShift;
    public float fillSpeed1;
    public float decaySpeed1;

    [Header("References")]
    public Slider Slider1;
    public GameObject Slider1P;
    public Image fillImage1;
    public RawImage FailStateNoStamina;
    public BossMovement Boss;
    public TimeManager timeManager;

    [Header("Vape References")]
    [SerializeField] private Animation vapeAnimation;     // Legacy Animation component
    [SerializeField] private string vapeClipName = "Vape"; // Name of the clip in Animation
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private float holdPauseTime = 0.4f;
    [SerializeField] private float smoothPauseSpeed = 2f; // How fast it eases toward 0.4s

    private bool isHolding = false;
    public bool isTutFinished = false;
    private bool vapePlaying = false;
    private Coroutine smoothPauseRoutine;
    bool isAudioPlayed = false;
    bool isAudioPlayed1 = false;
    bool isAudioPlayed2 = false;
    bool isAudioPlayed3 = false;
    bool isAudioPlayed4 = false;

    [Header("Hold Timer Settings")]
    public Vector2 triggerTimeRange = new Vector2(4f, 5f);
    private float holdTime = 0f;
    private float nextTriggerTime = 0f;
    private bool firstTriggerDone = false;
    private bool secondTriggerDone = false;

    private Color orange = new Color(1f, 0.5f, 0f);
    public float bossRelation = 1.0f;

    void Update()
    {
        bossRelation = Mathf.Clamp01(bossRelation + Time.deltaTime * 0.005f);
        Slider1.maxValue = bossRelation;
        Slider1P.GetComponent<RectTransform>().localScale = new Vector3(bossRelation, 1.7f, 1.0f);

        UpdateHoldKeySlider(Slider1, fillImage1, Key1, fillSpeed1, decaySpeed1, Color.red + Color.gray, orange + Color.gray, Color.green + Color.gray);
        HandleHoldTimer(Key1);
        HandleVape(Key1);

        if (Slider1.value == 0)
        {
            FailStateNoStamina.gameObject.SetActive(true);
            AudioManager.instance.PlayOneShot(FmodEvents.instance.BossTriggered, transform.position);
            timeManager.PauseTime();
        }
           

        // Trigger vape release when coughing
        if (SecondTriggerReached && isHolding)
        {
            Debug.Log("?? Cough Trigger! Acting as release.");
            HandleVapeRelease();
        }
    }

    public void onMistake()
    {
        //AudioManager.instance.PlayOneShot(FmodEvents.instance.Fail, transform.position);
        AudioManager.instance.PlayOneShot(FmodEvents.instance.BossAlert, transform.position);
        Debug.Log("onMistake");
        bossRelation -= 0.06f;
    }

    public void UpdateHoldKeySlider(Slider slider, Image fillImage, KeyCode keyToHold,
        float fillSpeed, float decaySpeed, Color minColor, Color midColor, Color maxColor)
    {
        if (slider == null || fillImage == null) return;

        if (Input.GetKey(keyToHold) && Mathf.Abs(Camera.main.transform.eulerAngles.y) < 1f)
            slider.value += fillSpeed * Time.deltaTime;
        else if(isTutFinished)
            slider.value -= decaySpeed * Time.deltaTime;

        slider.value = Mathf.Clamp01(slider.value);

        Color targetColor = slider.value < 0.5f
            ? Color.Lerp(minColor, midColor, slider.value * 2f)
            : Color.Lerp(midColor, maxColor, (slider.value - 0.5f) * 2f);

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

            if (!firstTriggerDone && holdTime >= nextTriggerTime)
            {
                firstTriggerDone = true;    
                if(!isAudioPlayed)
                {
                    AudioManager.instance.PlayOneShot(FmodEvents.instance.SmallCough, transform.position);
                    isAudioPlayed = true;
                }
                
                
            }
               

            if (firstTriggerDone && !secondTriggerDone && holdTime >= nextTriggerTime + 0.5f)
            {
                secondTriggerDone = true;
                Debug.Log("????? Cough Trigger!");
                if(!isAudioPlayed1)
                {
                    AudioManager.instance.PlayOneShot(FmodEvents.instance.BossAlert, transform.position);
                    Boss.baseForwardChance += 0.05f;
                    AudioManager.instance.PlayOneShot(FmodEvents.instance.LargeCough, transform.position);
                    isAudioPlayed1 = true;
                }
                
            }
        }
        else
        {
            holdTime = 0f;
            firstTriggerDone = false;
            secondTriggerDone = false;
        }
    }

    private void HandleVape(KeyCode key)
    {
        if (vapeAnimation == null || !vapeAnimation[vapeClipName]) return;

        if (Mathf.Abs(Camera.main.transform.eulerAngles.y) > 1f)
            return;

        AnimationState state = vapeAnimation[vapeClipName];

        if (Input.GetKeyDown(key))
        {
            vapeAnimation.Play(vapeClipName);
            if(!isAudioPlayed3)
            {
                AudioManager.instance.PlayOneShot(FmodEvents.instance.Inhale, transform.position);
                isAudioPlayed3 = true;
            }
            
            vapePlaying = true;
            isHolding = true;

            if (smoothPauseRoutine != null)
                StopCoroutine(smoothPauseRoutine);
            smoothPauseRoutine = StartCoroutine(SmoothPauseAtTime(state, holdPauseTime));
        }

        if (Input.GetKeyUp(key))
        {
            HandleVapeRelease();
        }
    }

    private IEnumerator SmoothPauseAtTime(AnimationState state, float pauseTime)
    {
        // Smoothly slow down and approach the pause time
        while (isHolding && state.time < pauseTime)
        {
            state.speed = Mathf.Lerp(state.speed, 0f, Time.deltaTime * smoothPauseSpeed);
            yield return null;

            if (state.time >= pauseTime)
            {
                state.time = pauseTime;
                state.speed = 0f;
                vapeAnimation.Sample();
                break;
            }
        }

        // Ensure it stays paused while holding
        while (isHolding)
        {
            state.speed = 0f;
            state.time = pauseTime;
            vapeAnimation.Sample();
            yield return null;
        }
    }

    private void HandleVapeRelease()
    {
        if (vapeAnimation == null || !vapeAnimation[vapeClipName]) return;

        AnimationState state = vapeAnimation[vapeClipName];

        if (smoothPauseRoutine != null)
            StopCoroutine(smoothPauseRoutine);

        isHolding = false;
        vapePlaying = false;

        // Resume animation smoothly from the paused frame
        state.speed = 1f;
        vapeAnimation.Play(vapeClipName);        
            
        isAudioPlayed = false;
        isAudioPlayed1 = false;
        isAudioPlayed2 = false;
        isAudioPlayed3 = false;

        StartCoroutine(SmokeBurst());
    }

    private IEnumerator SmokeBurst()
    {

        smokeEffect.GetComponent<ParticleSystem>().startLifetime = 0.4f;
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Exhale, transform.position);
        yield return new WaitForSeconds(1f);
        smokeEffect.GetComponent<ParticleSystem>().startLifetime = 0;
    }

    public bool SecondTriggerReached => secondTriggerDone;
}