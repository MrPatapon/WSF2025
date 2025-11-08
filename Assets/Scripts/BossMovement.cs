using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BossMovement : MonoBehaviour
{
    [Header("Boss Points")]
    public Transform[] points;

    [Header("Movement Settings")]
    public float moveInterval = 5f;
    public float moveSpeed = 3f;
    [Range(0f, 1f)] public float baseForwardChance = 0.2f;
    [Range(0f, 1f)] public float baseBackwardChance = 0.2f;
    public float secondTriggerBonus = 0.1f;
    public keyManager playerHoldRef;
    [SerializeField] RawImage FailStateVape;
    
    private int currentIndex = 0;
    private bool isMoving = false;
    private float currentForwardChance;
    private bool lastSecondTriggerState = false;
    void Start()
    {
        transform.position = points[0].position;        
        StartCoroutine(MovementLoop());
    }
    private void Update()
    {
        if (playerHoldRef != null)
        {
            bool secondTriggerActive = playerHoldRef.SecondTriggerReached;
            
            if (secondTriggerActive && !lastSecondTriggerState)
            {
                currentForwardChance += secondTriggerBonus;
                Debug.Log("Szansa na z³apanie wzros³a");
                currentForwardChance = Mathf.Clamp01(currentForwardChance);
            }

            lastSecondTriggerState = secondTriggerActive;
        }
        if (Input.GetKey(KeyCode.Space)&& currentIndex == points.Length)
        {
            FailStateVape.gameObject.SetActive(true);
        }
    }

    private IEnumerator MovementLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            if (!isMoving)
                DecideNextMove();
        }
    }
    private void DecideNextMove()
    {
        float stayChance = 0.6f;
        float moveForwardChance = currentForwardChance; 
        float moveBackwardChance = baseBackwardChance;

        stayChance = 1f - moveForwardChance - moveBackwardChance;

        float rand = Random.value;

        int nextIndex = currentIndex;

        if (rand < moveForwardChance)
        {
            // do przodu
            if (currentIndex < points.Length)
                nextIndex++;
            else
                { //TODO: co jak jest na koñcu i chce iœæ dalej
                }
        }
        else if (rand < moveForwardChance + moveBackwardChance)
        {
            // do ty³u
            if (currentIndex > 0)
                nextIndex--;
            else
                { }
        }
        else
        {
           ;
        }

        if (nextIndex != currentIndex)
        {
            StartCoroutine(MoveToPoint(points[nextIndex]));
            currentIndex = nextIndex;
        }
    }

    private IEnumerator MoveToPoint(Transform target)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
}

