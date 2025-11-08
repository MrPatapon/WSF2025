using UnityEngine;
using System.Collections;

public class CameraManager: MonoBehaviour
{
    [Header("3D Objects")]
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject rightObject;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationDuration = 1f;   // Time to rotate

    [Header("Default Rotation")]
    [SerializeField] private Vector3 defaultRotation = new Vector3(15f, 0f, 0f);

    [Header("Hover Rotations")]
    [SerializeField] private Vector3 leftRotation = new Vector3(0f, -45f, 0f);
    [SerializeField] private Vector3 rightRotation = new Vector3(0f, 45f, 0f);

    private Coroutine rotationRoutine;

    private bool isOverLeft = false;
    private bool isOverRight = false;

    void Start()
    {
        transform.rotation = Quaternion.Euler(defaultRotation); // Set starting rotation
    }

    void Update()
    {
        isOverLeft = IsMouseOverObject(leftObject);
        isOverRight = IsMouseOverObject(rightObject);

        UpdateRotation();
    }

    private bool IsMouseOverObject(GameObject obj)
    {
        if (obj == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == obj)
                return true;
        }

        return false;
    }

    private void UpdateRotation()
    {
        Vector3 targetRot;

        if (isOverLeft && !isOverRight) targetRot = leftRotation;
        else if (isOverRight && !isOverLeft) targetRot = rightRotation;
        else targetRot = defaultRotation;

        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotateToRotation(targetRot));
    }

    private IEnumerator RotateToRotation(Vector3 targetEuler)
    {
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(targetEuler);

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Lerp(startRot, targetRot, smoothT);
            yield return null;
        }

        transform.rotation = targetRot;
        rotationRoutine = null;
    }
}