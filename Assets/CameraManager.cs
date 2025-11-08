using UnityEngine;
using System.Collections;

public class CameraHoverRotate : MonoBehaviour
{
    [Header("3D Objects")]
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject rightObject;

    [Header("Rotation Settings")]
    [SerializeField] private float cameraAngle = 45f;       // Degrees to rotate
    [SerializeField] private float rotationDuration = 1f;   // Time to rotate
    [SerializeField] private float defaultYAngle = 0f;      // Default camera rotation

    private Coroutine rotationRoutine;

    private bool isOverLeft = false;
    private bool isOverRight = false;

    void Update()
    {
        // Check if the mouse is over the left or right object
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
        int targetDirection = 0;

        if (isOverLeft && !isOverRight) targetDirection = -1;
        else if (isOverRight && !isOverLeft) targetDirection = 1;
        else targetDirection = 0; // Neither or both hovered ? return to default

        float targetY = defaultYAngle + targetDirection * cameraAngle;

        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotateToAngle(targetY));
    }

    private IEnumerator RotateToAngle(float targetY)
    {
        float elapsed = 0f;
        float startY = transform.eulerAngles.y;

        Quaternion startRot = Quaternion.Euler(0f, startY, 0f);
        Quaternion targetRot = Quaternion.Euler(0f, targetY, 0f);

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
