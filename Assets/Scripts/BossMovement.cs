using UnityEngine;
using UnityEngine.UI;

public class BossMovement : MonoBehaviour
{
    [Header("Boss Points")]
    public Transform[] points;

    [Header("Movement Settings")]
    public float moveInterval;
    public float moveSpeed; // can be used for rotation speed
    [Range(0f, 1f)] public float baseForwardChance;
    [Range(0f, 1f)] public float baseBackwardChance;
    public float secondTriggerBonus = 0.1f;
    public keyManager playerHoldRef;
    [SerializeField] RawImage FailStateVape;
    [SerializeField] Camera MainCamera;

    private int currentIndex = 0;
    private float currentForwardChance;
    private bool lastSecondTriggerState = false;
    private float nextMoveTime = 0f;
    public bool canMove;

    void Start()
    {
        if (points.Length == 0) return;
        transform.position = points[0].position;
        currentForwardChance = baseForwardChance;
    }

    void Update()
    {
        if (playerHoldRef != null)
        {
            bool secondTriggerActive = playerHoldRef.SecondTriggerReached;

            if (secondTriggerActive && !lastSecondTriggerState)
            {
                currentForwardChance += secondTriggerBonus;
                currentForwardChance = Mathf.Clamp01(currentForwardChance);
                Debug.Log("Forward chance increased: " + currentForwardChance);
            }

            lastSecondTriggerState = secondTriggerActive;
        }

        if (Time.time >= nextMoveTime)
        {
            if(canMove)
                DecideNextMove();
            nextMoveTime = Time.time + moveInterval;
        }

        if (Input.GetKey(KeyCode.LeftShift) && (currentIndex == points.Length || currentIndex == points.Length - 1))
        {
            FailStateVape.gameObject.SetActive(true);
        }
    }

    private void DecideNextMove()
    {
        float moveForwardChance = currentForwardChance;
        float moveBackwardChance = baseBackwardChance;

        float rand = Random.value;
        int nextIndex = currentIndex;

        if (MainCamera.transform.rotation.y > 1f)
            return;

        if (rand < moveForwardChance)
        {
            // Move forward with loop
            nextIndex = (currentIndex + 1) % points.Length;
        }
        else if (rand < moveForwardChance + moveBackwardChance)
        {
            // Move backward with loop
            nextIndex = (currentIndex - 1 + points.Length) % points.Length;
        }
        else
        {
            // Stay at current position
            nextIndex = currentIndex;
        }

        if (nextIndex != currentIndex)
        {
            Vector3 targetPos = points[nextIndex].position;
            transform.position = targetPos;

            // Rotate to face next point
            Vector3 direction = (targetPos - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            currentIndex = nextIndex;
            transform.rotation = Quaternion.Euler(0f, transform.position.x < 0 ? 90f : -90f, 0f);
            Debug.Log($"Teleported to point {currentIndex}");
        }
    }
}