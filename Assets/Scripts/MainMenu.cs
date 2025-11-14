using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    [Header("Intensity Range")]
    [Range(0f, 100f)]
    public float minIntensity = 0.5f;

    [Range(0f, 100f)]
    public float maxIntensity = 1.5f;

    [Header("Flicker Settings")]
    [Range(0.1f, 50f)]
    public float flickerSpeed = 3f;

    [Tooltip("Higher value = more randomness in flicker speed")]
    [Range(0f, 1f)]
    public float speedRandomness = 0.2f;

    public Light _light;
    private float _targetIntensity;
    private float _currentSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("room 3");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        //_light = GetComponent<Light>();
        PickNewTarget();
    }

    void Update()
    {
        // Smoothly move toward target intensity
        _light.intensity = Mathf.Lerp(_light.intensity, _targetIntensity, Time.deltaTime * _currentSpeed);

        // If close enough, choose a new target intensity
        if (Mathf.Abs(_light.intensity - _targetIntensity) < 0.05f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        // Choose new random intensity
        _targetIntensity = Random.Range(minIntensity, maxIntensity);

        // Add some randomness to speed
        _currentSpeed = flickerSpeed + Random.Range(-speedRandomness, speedRandomness);
        _currentSpeed = Mathf.Max(0.01f, _currentSpeed); // avoid zero or negative speed
    }
}
