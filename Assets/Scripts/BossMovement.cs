using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BossMovement : MonoBehaviour
{
    [Header("Boss Points")]
    public Transform[] points;

    [Header("Movement Settings")]
    public float moveInterval;
    public float moveSpeed;
    [Range(0f, 1f)] public float baseForwardChance;
    [Range(0f, 1f)] public float baseBackwardChance;
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
        currentForwardChance = baseForwardChance; // ? THIS IS CRUCIAL
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
        if (Input.GetKey(KeyCode.LeftShift)&& currentIndex == points.Length)
        {
            FailStateVape.gameObject.SetActive(true);
        }
    }
    private IEnumerator MovementLoop()
    {
        while (true)
        {
            Debug.Log("Waiting for next move...");
            yield return new WaitForSeconds(moveInterval);
            DecideNextMove();
        }
    }

    private void DecideNextMove()
    {
        float moveForwardChance = currentForwardChance; // should be 1 for 100%
        float moveBackwardChance = baseBackwardChance;  // e.g., 0
        //float stayChance = 1f - moveForwardChance - moveBackwardChance;

        float rand = Random.value;
        int nextIndex = currentIndex;

        if (rand < moveForwardChance)
        {
            // Move forward
            if (currentIndex < points.Length - 1)
                nextIndex++;
            else
            {
                // At the end, do nothing or handle separately
                Debug.Log("At last point, cannot move forward");
            }
        }
        else if (rand < moveForwardChance + moveBackwardChance)
        {
            // Move backward
            if (currentIndex > 0)
                nextIndex--;
        }
        else
        {
            // Stay
        }

        if (nextIndex != currentIndex && !isMoving)
        {
            Debug.Log($"Moving from {currentIndex} ? {nextIndex}");
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
        float distance = Vector3.Distance(startPos, endPos);

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed / distance;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
        Debug.Log("Arrived at point: " + currentIndex);
    }
}

